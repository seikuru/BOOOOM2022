using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string TitleSceneName = "Title";
    [SerializeField] string MainGaneSceneName = "MainGame";
    [SerializeField] float waitTime = 0.3f;

    bool IsChange;//複数回遷移防止

    private void Start()
    {
        IsChange = false;

        //Time.timeScaleが変更されていた場合元に戻す
        if (SceneManager.GetActiveScene().name == TitleSceneName)
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
}
