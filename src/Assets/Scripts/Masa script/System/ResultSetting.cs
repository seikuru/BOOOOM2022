using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ResultSetting : MonoBehaviour
{
    [SerializeField] Image fede;
    [SerializeField] PlayableDirector roading;
    [SerializeField] UnityEvent unityEvent;
    [SerializeField] bool CheckPlayable = true;
    public void FedeOut()
    {
        if (CheckPlayable)
        {
            roading.Play();
            StartCoroutine(FedePlayable());
        }        
        else
            StartCoroutine(FedeColorChenge());
    }

    IEnumerator FedePlayable()
    {
        yield return null;

        yield return new WaitWhile(() => roading.state == PlayState.Playing);

        unityEvent.Invoke();

        // ChangeColorが終わった後の処理をここに書く
        Debug.Log(" FedePlayable完了");

        yield break;
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
