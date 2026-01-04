using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class SpriteAutoTransform : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private int Width = 256;
    [SerializeField] private int Height = 32;

    [SerializeField] Color32 SpectrumColor32 = Color.blue;
    [SerializeField] Color32 EmptyColor32 = Color.clear;

    private Texture2D texture;
    private Sprite sprite;

    // 前回の高さ
    private int[] BeforeIndex;
    // ピクセルバッファ
    Color32[] Colorbuffer;

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

        image.sprite = sprite;
    }

    /// <summary>
    /// 値配列（0～1）を受け取り、棒グラフを差分更新
    /// </summary>
    public void TextureUpdate(float[] Values)
    {
        // 初期化（透明）
        for (int x = 0; x < Width; x++)
        {
            int index = Mathf.Clamp((int)(Values[x] * Height), 0, Height - 1);
            int before = BeforeIndex[x];

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

            BeforeIndex[x] = index;
        }

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

    float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            time = 0;
            //TextureUpdate3();

            float[] values = new float[Width];

            for (int x = 0; x < Width; x++) 
                values[x] = UnityEngine.Random.Range(0, 1f);
            
            TextureUpdate(values);
        }
    }
}
