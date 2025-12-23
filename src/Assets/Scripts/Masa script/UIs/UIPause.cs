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


    public void EnableTimeline()
    {
        playableDirector.Play(EnableTimelineAsset);
    }

    public void DisableTimeline(TimelineAsset disableTimeline)
    {
        playableDirector.Play(disableTimeline);
    }

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
