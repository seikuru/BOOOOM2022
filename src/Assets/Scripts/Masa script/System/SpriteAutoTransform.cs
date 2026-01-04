using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class SpriteAutoTransform : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private int Width = 256;
    [SerializeField] private  int Height = 32;

    [SerializeField] Color SpectrumColor = Color.blue;
    [SerializeField] Color EmptyColor = Color.clear;

    private Texture2D texture;
    private Sprite sprite;

    private int[] BeforeIndex;

    void Awake()
    {
        // Texture 作成
        texture = new Texture2D(
            Width,
            Height,
            TextureFormat.RGBA32,
            false
        );

        texture.filterMode = FilterMode.Point; // にじみ防止（重要）
        texture.wrapMode = TextureWrapMode.Clamp;

        // 初期化（透明）
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                texture.SetPixel(x, y, EmptyColor);
        
        texture.Apply();

        // Sprite 作成
        sprite = Sprite.Create(
            texture,
            new Rect(0, 0, Width, Height),
            new Vector2(0.5f, 0.5f),
            1f // Pixels Per Unit（UIでは1でOK）
        );

        image.sprite = sprite;

        BeforeIndex = new int[Width];
    }

    public void TextureUpdate(float[] Values)
    {
        // 初期化（透明）
        for (int x = 0; x < Width; x++)
        {
            float value_float = Values[x] * Height;
            int Index =(int)(value_float);

            if (BeforeIndex[x] == Index)
            {
                continue;
            }
                
            else if(BeforeIndex[x] < Index)
            {
                for (int y = BeforeIndex[x]; y < Index; y++)
                {
                    texture.SetPixel(x, y, SpectrumColor);
                }
            }

            else if(BeforeIndex[x] > Index)  
            {
                for (int y = BeforeIndex[x]; y >= Index; y--)
                {
                    texture.SetPixel(x, y, EmptyColor);
                }   
            }
               
            BeforeIndex[x] = Index;
        }

        texture.Apply();
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

    void TextureUpdate3()
    {
        // 初期化（透明）
        for (int x = 0; x < Width; x++)
        {
            float alpha = x / (float)(Width - 1);

            texture.SetPixel(x, 0, new Color(1f, 1f, 1f, alpha));
        }
        texture.Apply();
    }

    float time = 0;
    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        if (time > 1f)
        {
            time = 0;
            float[] f = new float[Width];

            for (int x = 0; x < Width; x++)
            {
                f[x] = UnityEngine.Random.Range(0, 1f);
            }

            TextureUpdate(f);
            //TextureUpdate3();
        }
    }
}
