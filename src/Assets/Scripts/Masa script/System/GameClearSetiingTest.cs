using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClearSetiingTest : MonoBehaviour
{
    [SerializeField]
    Image BlackOutImage;

    [SerializeField]
    float BrackOutWaitTime = 1f;
    [SerializeField,Range(0,1f)]
    float BrackOutAlpha = 0.5f;

    [SerializeField]
    GameObject[] enabledObjects;

    Coroutine coroutine = null;


    public void GameClear()
    {
        Time.timeScale = 0;
        if(coroutine ==  null)
        {
            coroutine = StartCoroutine(BlackOut());
        }
    }

    IEnumerator BlackOut()
    {
        // ChangeColorの完了まで待機
        yield return StartCoroutine(ChangeColor(BrackOutWaitTime));

        // ChangeColorが終わった後の処理をここに書く
        Debug.Log("フェードアウト完了");

        ObjectsEnabled();
    }

    public IEnumerator ChangeColor(float duration)
    {
        BlackOutImage.enabled = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            BlackOutImage.color = new Color(0, 0, 0, t * BrackOutAlpha);

            yield return null;
        }

        BlackOutImage.color = new Color(0, 0, 0, BrackOutAlpha);
    }

    void ObjectsEnabled()
    {
        foreach(GameObject obj in enabledObjects)
        {
            obj.SetActive(true);
        }
    }
}
