using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    /// シーン移行を管理するクラス

    [SerializeField] string TitleSceneName = "Title_2";
    [SerializeField] string MainGaneSceneName = "MainGame";
    [SerializeField] string EasyName = "ichi_yama";
    [SerializeField] string HardName = "Ame_Hard";
    [SerializeField] float waitTime = 0.3f;

    // 一つ前のシーンの名前の文字列
    static string beforeName;

    bool IsChange;//複数回遷移防止

    // 非同期処理の変数
    Coroutine AsyncCoroutine;

    // シーン移行のためのbool変数
    bool canActivateScene = false;

    /// <summary>
    /// シーン移行のためのbool変数を変更
    /// </summary>
    public void OnPressContinue()
    {
        canActivateScene = true;
    }

    private void Start()
    {
        // シーン名を保存
        if (SceneManager.GetActiveScene().name == HardName)
            beforeName = HardName;
        if (SceneManager.GetActiveScene().name == EasyName)
            beforeName = EasyName;

        IsChange = false;
        canActivateScene = false;
        
        // タイムスケールを元に戻す
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

    /// <summary>
    /// 指定した名前のシーンに遷移させる
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    public void SceneChangeAsync(string sceneName)
    {
        if (IsChange) return;

        AsyncCoroutine = StartCoroutine(LoadSceneAsync(sceneName));
    }

    /// <summary>
    /// ひとつ前に遷移していたシーンに遷移させる
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    public void SceneChengeRetry()
    {
        if (IsChange || beforeName == "") return;

        AsyncCoroutine = StartCoroutine(LoadSceneAsync(beforeName));
    }

    /// <summary>
    /// 非同期処理によってシーン移行処理を9割で止めた後に一定時間後に移行する
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <returns>シーン遷移</returns>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        IsChange = true;
        // 非同期ロード開始
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        // 90% で止める（Unity仕様）
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
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


    /// <summary>
    /// ひとつ前に遷移していたシーンに遷移させる
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    public void SceneChangeAsyncActivate(string sceneName)
    {
        if (IsChange) return;

        AsyncCoroutine = StartCoroutine(LoadSceneAsyncActivate(sceneName));
    }

    /// <summary>
    /// 非同期処理によってシーン移行処理を9割で止めた後にbool変数で移行する
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <returns>シーン移行</returns>
    private IEnumerator LoadSceneAsyncActivate(string sceneName)
    {
        IsChange = true;
        // 非同期ロード開始
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        // 90% で止める（Unity仕様）
        async.allowSceneActivation = false;

        while (!async.isDone)
        {

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
