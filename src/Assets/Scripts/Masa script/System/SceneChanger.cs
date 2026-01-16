using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string TitleSceneName = "Title";
    [SerializeField] string MainGaneSceneName = "MainGame";
    [SerializeField] float waitTime = 0.3f;

    bool IsChange;//複数回遷移防止

    Coroutine AsyncCoroutine;

    private void Start()
    {
        IsChange = false;
        canActivateScene = false;
        //Time.timeScaleが変更されていた場合元に戻す
        //if (SceneManager.GetActiveScene().name == TitleSceneName)
            Time.timeScale = 1.0f;
    }

    /// <summary>
    /// 非同期処理でシーン移行までに時間を稼ぐ
    /// </summary>
    /// <param name="waitTime">稼ぐ時間</param>
    /// <param name="sceneName">移行するシーン名</param>
    /// <returns>シーン移行する</returns>
    private IEnumerator WaitForSecondCoroutine(float waitTime, string sceneName)
    {
        Debug.Log("待機開始");
        IsChange = true;

        yield return new WaitForSeconds(waitTime);

        Debug.Log("経過！");
        IsChange = false;

        // 遷移
        SceneManager.LoadSceneAsync(sceneName);
    }

    /// <summary>
    /// 指定のシーンに遷移
    /// </summary>
    public void SceneChangeString(string sceneName) 
    {
        if (IsChange) return;

        if (Time.timeScale == 0)
            SceneManager.LoadSceneAsync(sceneName);
        else
            StartCoroutine(WaitForSecondCoroutine(waitTime, sceneName));
       
    }

    /// <summary>
    /// 同じシーンに再度遷移させる。
    /// </summary>
    public void SceneRerodeSame()
    {
        if (IsChange) return;

        if (Time.timeScale == 0)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        else
            StartCoroutine(WaitForSecondCoroutine(waitTime, SceneManager.GetActiveScene().name));
    }  

    /// <summary>
    /// タイトルに遷移させる。
    /// </summary>
    public void SceneChangeTitle()
    {
        if (IsChange) return;

        if(Time.timeScale == 0)
            SceneManager.LoadSceneAsync(TitleSceneName);
        else
        StartCoroutine(WaitForSecondCoroutine(waitTime, TitleSceneName));
    }

    /// <summary>
    /// メインゲームに遷移させる。
    /// </summary>
    public void SceneChangeMainGame()
    {
        if (IsChange) return;

        if (Time.timeScale == 0)
            SceneManager.LoadSceneAsync(MainGaneSceneName);
        else
            StartCoroutine(WaitForSecondCoroutine(waitTime, MainGaneSceneName));
    }

    public void SceneChangeAsync(string sceneName)
    {
        if (IsChange) return;

        AsyncCoroutine = StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        IsChange = true;
        // 非同期ロード開始
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        // 90% で止める（Unity仕様）
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            /*
            // progress は 0 ～ 0.9 までしか来ない
            float progress = Mathf.Clamp01(async.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;
            */

            // 90% 到達＝ロード完了
            if (async.progress >= 0.9f)
            {
                IsChange = false;

                // ここで演出を入れられる
                // フェード完了待ち、一定時間待つ等
                yield return new WaitForSeconds(waitTime);

                async.allowSceneActivation = true;
            }

            yield return null; // フリーズ防止（最重要）
        }
    }

    bool canActivateScene = false;
    
    public void OnPressContinue()
    {
        canActivateScene = true;
    }


    public void SceneChangeAsyncActivate(string sceneName)
    {
        if (IsChange) return;

        AsyncCoroutine = StartCoroutine(LoadSceneAsyncActivate(sceneName));
    }

    IEnumerator LoadSceneAsyncActivate(string sceneName)
    {
        IsChange = true;
        // 非同期ロード開始
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        // 90% で止める（Unity仕様）
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            /*
            // progress は 0 ～ 0.9 までしか来ない
            float progress = Mathf.Clamp01(async.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;
            */

            Debug.Log(async.progress);

            // 90% 到達＝ロード完了
            if (async.progress >= 0.9f)
            {
                IsChange = false;

                // ここで演出を入れられる
                // フェード完了待ち、外からの入力を待つ
                yield return new WaitUntil(() => canActivateScene);

                async.allowSceneActivation = true;
            }

            yield return null; // フリーズ防止（最重要）
        }
    }
}
