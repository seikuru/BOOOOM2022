using UnityEngine;
using UnityEngine.UI;

public class GageSpectrum : MonoBehaviour
{
    /// 特定MusicType用のスペクトラムゲージ表示クラス
    /// 指定した周波数帯域のみを抽出して表示
    /// 
    [SerializeField] private Image image;

    // 元スペクトラムの想定サイズ
    [SerializeField] private int Width = 256;
    [SerializeField] private int Height = 32;

    // 使用する周波数帯域の範囲
    [SerializeField] private int UseRangeWidthMin = 1;
    [SerializeField] private int UseRangeWidthMax = 51;

    [SerializeField] private int UseSegmentIndex = 5;// 何分割で平均化するか

    [SerializeField] Color32 SpectrumColor32 = Color.blue;
    [SerializeField] Color32 EmptyColor32 = Color.clear;

    [SerializeField] MusicType musicType; // このゲージが対応するMusicType

    private bool isOpen;

    // 描画用テクスチャ
    private Texture2D texture;

    // UI表示用スプライト
    private Sprite sprite;

    // 実際に使用する横幅（平均化後）
    private int UseRange = 10;
   
    // 前回の高さ
    private int[] BeforeIndex;

    // ピクセルバッファ
    Color32[] Colorbuffer;

    /// <summary>
    /// 指定されたMusicTypeが一致した場合、スペクトラム表示を解放する
    /// </summary>
    public void OpenSpectrumCheck(MusicType type)
    {
        if (musicType == type)
            isOpen = true;
    }

    private void Awake()
    {
        // 初期状態では非表示
        isOpen = false;

        // 使用帯域幅を平均化後の解像度に変換
        UseRange = (UseRangeWidthMax - UseRangeWidthMin) / UseSegmentIndex;

        // Texture 作成
        texture = new Texture2D(
            UseRange,
            Height,
            TextureFormat.RGBA32,
            false
        );

        texture.filterMode = FilterMode.Point; // にじみ防止
        texture.wrapMode = TextureWrapMode.Clamp;

        // ピクセルバッファ確保
        Colorbuffer = new Color32[UseRange * Height];

        // 各列の前回描画高さを保持
        BeforeIndex = new int[UseRange];

        // BeforeIndex 初期化
        for (int x = 0; x < UseRange; x++)
            BeforeIndex[x] = -1;

        // 初期化（透明）
        for (int i = 0; i < Colorbuffer.Length; i++)
            Colorbuffer[i] = EmptyColor32;

        texture.SetPixels32(Colorbuffer);
        texture.Apply(false);

        // Sprite 作成
        sprite = Sprite.Create(
            texture,
            new Rect(0, 0, UseRange, Height),
            new Vector2(0.5f, 0.5f),
            1f // Pixels Per Unit（UIでは1でOK）
        );

        image.sprite = sprite;
    }

    /// <summary>
    /// 指定した範囲のスペクトラム値を平均化して描画更新
    /// </summary>
    public void TextureUpdateRange(float[] Values)
    {
        // 未解放状態では更新しない
        if (!isOpen)
            return;

        int value = 0,count = 0, index = 0;

        // 指定された周波数帯域のみを使用
        for (int x = UseRangeWidthMin; x < UseRangeWidthMax; x++)
        {
            // スペクトラム値を高さに変換して加算
            value += Mathf.Clamp((int)(Values[x] * Height * x), 0, Height - 1);
           
            count++;

            // セグメント単位で平均化
            if (count < UseSegmentIndex)
            {
                continue;
            }

            count = 0;
            value = value / UseSegmentIndex;

            int before = BeforeIndex[index];
            
            if (before < value)
            {
                // 伸びる
                for (int y = before + 1; y <= value; y++)
                {
                    Colorbuffer[index + y * UseRange] = SpectrumColor32;
                }
            }
            else if (before > value)
            {
                // 縮む
                for (int y = before; y > value; y--)
                {
                    Colorbuffer[index + y * UseRange] = EmptyColor32;
                }
            }

            // 今回の高さを保存
            BeforeIndex[index] = value;

            index++;
            value = 0;
        }

        texture.SetPixels32(Colorbuffer);
        texture.Apply(false);
    }
}
