using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class UIPause : MonoBehaviour
{
    [SerializeField]
    PlayableDirector playableDirector;

    [SerializeField]
    TimelineAsset EnableTimelineAsset;

    [SerializeField]
    BGMControll BGMcontroll;

    public void EnableTimeline()
    {
        playableDirector.Play(EnableTimelineAsset);
    }

    public void DisableTimeline(TimelineAsset disableTimeline)
    {
        playableDirector.Play(disableTimeline);
    }

    // ˆêŽž’âŽ~
    public void BGMPause() => BGMcontroll.BGMPause();

    // ˆêŽž’âŽ~‰ðœ
    public void BGMUnPause() => BGMcontroll.BGMUnPause();
       
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
