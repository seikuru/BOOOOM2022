using UnityEngine;
using UnityEngine.UI;

public class GamingImage : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] float Speed = 0.2f;
    /// <summary>
    /// F‘Š‚ğ‰ñ“]‚³‚¹‚éi0`1‚Åˆêüj
    /// </summary>
    public Color RotateHue(Color color, float deltaHue)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);

        h += deltaHue;
        if (h > 1f) h -= 1f;
        if (h < 0f) h += 1f;

        return Color.HSVToRGB(h, s, v);
    }

    // Update is called once per frame
    void Update()
    {
        _image.color = RotateHue(_image.color, Time.deltaTime * Speed);
    }
}
