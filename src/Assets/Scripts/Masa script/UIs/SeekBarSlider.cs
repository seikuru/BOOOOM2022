using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBarSlider : MonoBehaviour
{
    [SerializeField]
    RectTransform sliderKnobRT;

    [SerializeField]
    RectTransform SeekBarBodyRT;

    [SerializeField] CountDownSeekBar seekBar;

    private void Update()
    {
        SeekBerUpdate(1f - seekBar.GetCurrentTimeClamp());
    }

    public void SeekBerUpdate(float clampValue)
    {
        float KnobWidth = (SeekBarBodyRT.rect.width / 2 - sliderKnobRT.rect.width / 2);
        Debug.Log(SeekBarBodyRT.position);
        sliderKnobRT.position = 
            new Vector3(
            SeekBarBodyRT.position.x + (KnobWidth * clampValue * 2 - KnobWidth),
            sliderKnobRT.position.y,
            sliderKnobRT.position.z);
    }
}
