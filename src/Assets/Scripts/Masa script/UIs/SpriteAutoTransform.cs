using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAutoTransform : MonoBehaviour
{
    /// スペクトラム表示用スプライトを動的に生成・更新するクラス
    /// Texture2Dを動的変更して棒グラフスペクトラムを描画

    [SerializeField] private Image image;
    [SerializeField] private int Width = 256;
    [SerializeField] private int Height = 32;

    [SerializeField] Color32 SpectrumColor32 = Color.blue;
    [SerializeField] Color32 EmptyColor32 = Color.clear;

    [SerializeField] float Interval = 0.2f;
    [SerializeField] AudioCulcurator audioCulcurator;

    [SerializeField] GageSpectrum[] gageSpectrums;// 各ゲージへのスペクトラム連動

    // 描画用テクスチャ
    private Texture2D texture;

    // UI表示用スプライト
    private Sprite sprite;

    // 前回の高さ
    private int[] BeforeIndex;

    // ピクセルバッファ
    Color32[] Colorbuffer;

    // 更新タイマー
    float time = 0;

    // MusicType解放通知キュー
    static Queue<MusicType> openTypes;

    /// <summary>
    /// 外部からMusicType解放を通知
    /// </summary>
    public static void OpenTypeSetting(MusicType type) => openTypes.Enqueue(type);

    void Awake()
    {
        // Texture 作成
        texture = new Texture2D(
            Width,
            Height,
            TextureFormat.RGBA32,
            false
        );

        texture.filterMode = FilterMode.Point; // にじみ防止
        texture.wrapMode = TextureWrapMode.Clamp;

        Colorbuffer = new Color32[Width * Height];
        BeforeIndex = new int[Width];

        // BeforeIndex 初期化
        for (int x = 0; x < Width; x++)
            BeforeIndex[x] = -1;

        // 初期化（透明）
        for (int i = 0; i < Colorbuffer.Length; i++)
            Colorbuffer[i] = EmptyColor32;

        texture.SetPixels32(Colorbuffer);
        texture.Apply(false);

        // Sprite 作成
        sprite = Sprite.Create(
            texture,
            new Rect(0, 0, Width, Height),
            new Vector2(0.5f, 0.5f),
            1f // Pixels Per Unit（UIでは1でOK）
        );

        // Imageに反映
        image.sprite = sprite;

        // MusicType解放通知用キュー初期化
        openTypes = new();
    }

    /// <summary>
    /// 値配列（0～1）を受け取り、棒グラフを差分更新
    /// </summary>
    public void TextureUpdate(float[] Values)
    {
        // 各X列ごとにスペクトラム高さを計算
        for (int x = 0; x < Width; x++)
        {
            int index = Mathf.Clamp((int)(Values[x] * Height * x), 0, Height - 1);
            int before = BeforeIndex[x];

            // 前回と同じ高さなら更新なし
            if (before == index)    
                continue;         
                
            else if(before < index)
            {
                // 伸びる
                for (int y = before + 1; y <= index; y++)
                {
                    Colorbuffer[x + y * Width] = SpectrumColor32;
                }
            }

            else if(before > index)  
            {
                // 縮む
                for (int y = before; y > index; y--)
                {
                    Colorbuffer[x + y * Width] = EmptyColor32;
                }   
            }

            // 今回の高さを保存
            BeforeIndex[x] = index;
        }

        // ピクセル反映
        texture.SetPixels32(Colorbuffer);
        texture.Apply(false);
    }

    void TextureUpdate2()
    {
        // 初期化（透明）
        for (int x = 0; x < 16; x++)
        {
            int random = UnityEngine.Random.Range(0, 255);
            for (int xx = 0; xx < 16; xx++)
            {
                texture.SetPixel(x * xx, 0, new Color(
                1,
                1,
                1,
                random));
            }
                
        }
        texture.Apply();
    }

    // デバッグ用：横方向アルファグラデーション
    void TextureUpdate3()
    {
        for (int x = 0; x < Width; x++)
        {
            byte a = (byte)(x * 255 / (Width - 1));
            Colorbuffer[x] = new Color32(255, 255, 255, a);
        }

        texture.SetPixels32(Colorbuffer);
        texture.Apply(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        // 一定間隔でスペクトラム更新
        if (time > Interval)
        {
            time = 0;
            float[] values = new float[Width];

            if (audioCulcurator != null)
                audioCulcurator.GetSpectrum(ref values);
            // ないならランダムな値
            else
                for (int x = 0; x < Width; x++)
                    values[x] = UnityEngine.Random.Range(0, 1f);

            // スペクトラム描画更新
            TextureUpdate(values);

            // 各ゲージへ値を反映
            foreach (var gage in gageSpectrums)
            {
                gage.TextureUpdateRange(values);
            }
        }

        // MusicType解放イベント処理
        while (openTypes != null && openTypes.Count != 0)
        {
            var type = openTypes.Dequeue();

            OpenSpectrums(type);
        }
    }

    /// <summary>
    /// 指定MusicTypeに対応するスペクトラムを解放
    /// </summary>
    void OpenSpectrums(MusicType type)
    {
        foreach (var gage in gageSpectrums)
        {
            gage.OpenSpectrumCheck(type);
        }
    }
}
