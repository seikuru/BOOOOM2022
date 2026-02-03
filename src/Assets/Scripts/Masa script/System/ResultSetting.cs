using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ResultSetting : MonoBehaviour
{
    /// リザルト画面のフェードアウト処理を管理するクラス
    /// TimelineまたはImage色変更の2つの方法でフェードアウトを実行可能
    
    [SerializeField] Image fede;
    [SerializeField] PlayableDirector roading;
    [SerializeField] UnityEvent unityEvent;
    [SerializeField] bool CheckPlayable = true;

    /// <summary>
    /// フェードアウト処理を開始
    /// CheckPlayableの値に応じてTimeline再生または色変更フェードを選択
    /// </summary>
    public void FedeOut()
    {
        if (CheckPlayable)
        {
            // Timeline使用の場合
            roading.Play();
            StartCoroutine(FedePlayable());
        }
        else
        {
            // Image色変更の場合
            StartCoroutine(FedeColorChenge());
        }        
    }

    /// <summary>
    /// Timeline再生によるフェードアウト処理
    /// Timeline完了後にイベントを実行
    /// </summary>
    IEnumerator FedePlayable()
    {
        yield return null;

        // Timeline再生が完了するまで待機
        yield return new WaitWhile(() => roading.state == PlayState.Playing);

        // 完了後のイベントを実行
        unityEvent.Invoke();

        // ChangeColorが終わった後の処理をここに書く
        Debug.Log(" FedePlayable完了");
        yield break;
    }

    /// <summary>
    /// Image色変更によるフェードアウト処理
    /// アルファ値を徐々に1.0まで増加させて画面を暗転
    /// </summary>
    IEnumerator FedeColorChenge()
    {
        // アルファ値が1.0になるまでループ
        while (fede.color.a < 1.0f)
        {
            var currentColor = fede.color;
            // フレームレート非依存でアルファ値を増加
            currentColor.a += Time.smoothDeltaTime;

            fede.color = currentColor;
            yield return null;
        }

        // 完了後のイベントを実行
        unityEvent.Invoke();

        // ChangeColorが終わった後の処理をここに書く
        Debug.Log("フェードアウト完了");

        yield break;
    }
}
