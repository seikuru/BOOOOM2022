using UnityEditor;
using UnityEngine;
using System.IO;

public class TextureCreate : EditorWindow
{
    private Color startColor = Color.white;
    private Color endColor = new Color(1, 1, 1, 0);

    private Color startColor2 = Color.white;
    private Color endColor2 = new Color(1, 1, 1, 0);

    private bool topToBottom = true;
    private const int Width = 256;
    private const int Height = 256;

    [MenuItem("Tools/Gradient Texture Creator")]
    public static void Open()
    {
        GetWindow<TextureCreate>("Gradient Texture Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("1 x 256 Vertical Gradient Texture", EditorStyles.boldLabel);

        startColor = EditorGUILayout.ColorField("Start Color", startColor);
        endColor = EditorGUILayout.ColorField("End Color", endColor);

        topToBottom = EditorGUILayout.Toggle("Top To Bottom", topToBottom);

        GUILayout.Space(10);

        if (GUILayout.Button("Create Texture"))
        {
            CreateTexture1();
        }

        GUILayout.Label("256 x 256 Texture", EditorStyles.boldLabel);

        startColor2 = EditorGUILayout.ColorField("Start Color", startColor2);
        endColor2 = EditorGUILayout.ColorField("End Color", endColor2);
        GUILayout.Space(10);

        if (GUILayout.Button("Create Texture"))
        {
            CreateTexture2();
        }
    }

    private void CreateTexture1()
    {
        Texture2D texture = new Texture2D(
            1,
            Height,
            TextureFormat.RGBA32,
            false
        );

        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < Height; y++)
        {
            float t = topToBottom
                ? 1f - (float)y / (Height - 1)
                : (float)y / (Height - 1);

            Color c = Color.Lerp(startColor, endColor, t);
            texture.SetPixel(0, y, c);
        }

        texture.Apply();

        string path = EditorUtility.SaveFilePanelInProject(
            "Save Gradient Texture",
            "VerticalGradient",
            "png",
            "Select save location"
        );

        if (string.IsNullOrEmpty(path))
            return;

        byte[] pngData = texture.EncodeToPNG();
        File.WriteAllBytes(path, pngData);

        AssetDatabase.Refresh();

        // Import Settings ‚ðŽ©“®’²®
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = 1;
            importer.filterMode = FilterMode.Bilinear;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        DestroyImmediate(texture);
    }

    private void CreateTexture2()
    {
        Texture2D texture = new Texture2D(
            Width,
            Height,
            TextureFormat.RGBA32,
            false
        );

        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;

        float Senter = (Height - 1) / 2;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float distance = Vector2.Distance(new(Senter, Senter), new(x, y));

                Color c = endColor2;

                if(distance * 2 <= Senter)
                    c = startColor2;
                else if(distance <= Senter)
                {
                    float t = Mathf.Clamp01(distance / Senter);

                    c = Color.Lerp(startColor2,endColor2, t);
                }
               
                texture.SetPixel(x, y, c);
            }
        }

        texture.Apply();

        string path = EditorUtility.SaveFilePanelInProject(
            "Save Gradient Texture",
            "VerticalGradient",
            "png",
            "Select save location"
        );

        if (string.IsNullOrEmpty(path))
            return;

        byte[] pngData = texture.EncodeToPNG();
        File.WriteAllBytes(path, pngData);

        AssetDatabase.Refresh();

        // Import Settings ‚ðŽ©“®’²®
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = 1;
            importer.filterMode = FilterMode.Bilinear;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        DestroyImmediate(texture);
    }
}