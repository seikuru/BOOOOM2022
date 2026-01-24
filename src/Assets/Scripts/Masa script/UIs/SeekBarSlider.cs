using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeekBarSlider : MonoBehaviour
{
    [SerializeField]
    RectTransform sliderKnobRT;

    [SerializeField]
    RectTransform SeekBarBodyRT;

    [SerializeField] CountDownSeekBar seekBar;

    [SerializeField] RectMask2D SeekBodyMask2D;
    [SerializeField] RectMask2D SeekBarMask2D;

    [SerializeField] float StartPaddingBody = 1900f;
    [SerializeField] float StartPaddingSpectrum = 1884f;

    private void Update()
    {
        float seekBarClamp = seekBar.GetCurrentTimeClamp();

        SeekBodyUpdate(1f - seekBarClamp);
        SpectrumMaskUpdate(1f - seekBarClamp);
    }

    public void SeekBodyUpdate(float clampValue)
    {
        float KnobWidth = (SeekBarBodyRT.rect.width / 2 - sliderKnobRT.rect.width / 2);
        //Debug.Log(SeekBarBodyRT.position);
        sliderKnobRT.position = 
            new Vector3(
            SeekBarBodyRT.position.x + (KnobWidth * clampValue * 2 - KnobWidth),
            sliderKnobRT.position.y,
            sliderKnobRT.position.z);

        var currentPadding = SeekBodyMask2D.padding;

        currentPadding.z = StartPaddingBody - (clampValue * StartPaddingBody);

        SeekBodyMask2D.padding = currentPadding;
    }

    public void SpectrumMaskUpdate(float clampValue)
    {
        var currentPadding = SeekBarMask2D.padding;

        currentPadding.z = StartPaddingSpectrum - (clampValue * StartPaddingSpectrum);

        SeekBarMask2D.padding = currentPadding;
    }
}
