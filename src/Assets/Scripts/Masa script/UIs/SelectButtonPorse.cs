using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SelectButtonPorse : SelectButton
{
    [SerializeField]
    PlayableDirector UIDirector;

    [SerializeField]
    TimelineAsset[] timelines;

    protected override void ButtonSelectoverride()
    {
        if(UIDirector != null)// && UIDirector.playableAsset != timelines[currentButtonIndex])
        {
            UIDirector.Play(timelines[currentButtonIndex]);
        }
    }
}
