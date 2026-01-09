using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResultSetting : MonoBehaviour
{
    [SerializeField] Image fede;

    [SerializeField] UnityEvent unityEvent;

    public void FedeOut()
    {
        StartCoroutine(FedeColorChenge());
    }

    IEnumerator FedeColorChenge()
    {
        while (fede.color.a < 1.0f)
        {
            var currentColor = fede.color;
            currentColor.a += Time.smoothDeltaTime;

            fede.color = currentColor;
            yield return null;
        }

        unityEvent.Invoke();

        // ChangeColorが終わった後の処理をここに書く
        Debug.Log("フェードアウト完了");

        yield break;
    }
}
