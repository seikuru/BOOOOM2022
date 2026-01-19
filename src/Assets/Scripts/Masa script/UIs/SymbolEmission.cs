using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SymbolEmission : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float max_a = 40;
    [SerializeField] MusicType[] types;
    [SerializeField] Color[] typeColor;
    [SerializeField] float FadeinTime = 0.6f;

    // Update is called once per frame
    public void SetEmission(MusicType type)
    {
        int i = 0;
        for (; i < types.Length; i++)
        {
            if (types[i] == type)
                break;
        }

        if (types.Length == i)
            return;

        StartCoroutine(PlayEmission(typeColor[i]));
    }

    IEnumerator PlayEmission(Color color)
    {
        Color c = color;
        image.color = typeColor[0];

        float count = 0f;

        while (count <= FadeinTime)
        {
            count += Time.fixedDeltaTime;

            float lerp = Mathf.Clamp01(1 - count / FadeinTime);

            c.a = lerp * max_a / 255;
            image.color = c;
            
            yield return null;
        }

        image.color = new(0,0,0,0);
    }
}
