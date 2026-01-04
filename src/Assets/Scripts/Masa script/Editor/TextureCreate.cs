using UnityEditor;
using UnityEngine;
using System.IO;

public class TextureCreate : EditorWindow
{
    private Color startColor = Color.white;
    private Color endColor = new Color(1, 1, 1, 0);
    private bool topToBottom = true;

    private const int Width = 1;
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
            CreateTexture();
        }
    }

    private void CreateTexture()
    {
        Texture2D texture = new Texture2D(
            Width,
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
}