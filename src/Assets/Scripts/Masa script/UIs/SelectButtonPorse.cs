using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SelectButtonPorse : SelectButton
{
    /// SelectButtonにTimeLineを拡張した派生クラス。ポーズ画面に使う

    [SerializeField]
    PlayableDirector UIDirector;

    [SerializeField]
    TimelineAsset[] timelines;

    /// <summary>
    /// TimeLineを実行
    /// </summary>
    protected override void ButtonSelectoverride()
    {
        if(UIDirector != null)// && UIDirector.playableAsset != timelines[currentButtonIndex])
        {
            UIDirector.Play(timelines[currentButtonIndex]);
        }
    }
}
