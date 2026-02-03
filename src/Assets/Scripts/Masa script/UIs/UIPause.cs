using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class UIPause : MonoBehaviour
{   
    /// ポーズ画面を管理するクラス
    /// TImeLineで管理する
    
    [SerializeField]
    PlayableDirector playableDirector;

    [SerializeField]
    TimelineAsset EnableTimelineAsset;

    [SerializeField]
    BGMControll BGMcontroll;

    /// <summary>
    /// ポーズ画面をTimeLineを実行
    /// </summary>
    public void EnableTimeline()
    {
        playableDirector.Play(EnableTimelineAsset);
    }

    /// <summary>
    /// TimeLineを実行
    /// </summary>
    /// <param name="disableTimeline">動かしたいTImeLine</param>
    public void DisableTimeline(TimelineAsset disableTimeline)
    {
        playableDirector.Play(disableTimeline);
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    public void BGMPause() => BGMcontroll.BGMPause();

    /// <summary>
    /// 一時停止解除
    /// </summary>
    public void BGMUnPause() => BGMcontroll.BGMUnPause();
       
    /// <summary>
    /// タイムスケールの値を指定
    /// </summary>
    /// <param name="scale">スケール値</param>
    public void SetTimeScale(float scale) => Time.timeScale = scale;

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            EnableTimeline();
        }
#endif
    }
}
