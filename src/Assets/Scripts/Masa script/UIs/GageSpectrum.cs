using UnityEngine;
using UnityEngine.UI;

public class GageSpectrum : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private int Width = 256;
    [SerializeField] private int Height = 32;

    [SerializeField] private int UseRangeWidthMin = 1;
    [SerializeField] private int UseRangeWidthMax = 51;

    [SerializeField] private int UseSegmentIndex = 5;

    [SerializeField] Color32 SpectrumColor32 = Color.blue;
    [SerializeField] Color32 EmptyColor32 = Color.clear;

    [SerializeField] MusicType musicType;

    public void OpenSpectrumCheck(MusicType type)
    {
        if(musicType == type)
            isOpen = true;
    }

    private bool isOpen;

    private Texture2D texture;
    private Sprite sprite;

    private int UseRange = 10;
   
    // 前回の高さ
    private int[] BeforeIndex;
    // ピクセルバッファ
    Color32[] Colorbuffer;

    void Awake()
    {
        isOpen = false;

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

        Colorbuffer = new Color32[UseRange * Height];
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


    public void TextureUpdateRange(float[] Values)
    {
        if (!isOpen)
            return;

        int value = 0,count = 0, index = 0;

        for (int x = UseRangeWidthMin; x < UseRangeWidthMax; x++)
        {
            value += Mathf.Clamp((int)(Values[x] * Height * x), 0, Height - 1);
           
            count++;

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

            BeforeIndex[index] = value;

            index++;
            value = 0;
        }

        texture.SetPixels32(Colorbuffer);
        texture.Apply(false);
    }
}
