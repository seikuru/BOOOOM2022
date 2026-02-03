using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SymbolEmission : MonoBehaviour
{
    /// シンボル開放時にエミッションとして画面を光らせる処理
    
    [SerializeField] Image image;
    [SerializeField] float max_a = 40;
    [SerializeField] MusicType[] types;
    [SerializeField] Color[] typeColor;
    [SerializeField] float FadeinTime = 0.6f;

   /// <summary>
   /// MusicTypeから色に変換して非同期処理を実行する
   /// </summary>
   /// <param name="type">MusicType</param>
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

    /// <summary>
    /// 画面を光らせる処理
    /// </summary>
    /// <param name="color">光らせる色</param>
    /// <returns>光らせて元に戻す</returns>
    private IEnumerator PlayEmission(Color color)
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
