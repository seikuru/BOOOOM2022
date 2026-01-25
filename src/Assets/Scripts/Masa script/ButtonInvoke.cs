using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInvoke : MonoBehaviour
{
    [SerializeField] Button button;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            button.onClick.Invoke();
    }

    [Header("�����Đ����� AudioSource �Q")]
    [SerializeField]
    private List<AudioSource> audioSources = new List<AudioSource>();

    [Header("�Đ��J�n�܂ł̗\�񎞊ԁi�b�j")]
    [SerializeField]
    private double scheduleOffset = 0.1;

    // ���݂̍Đ��J�nDSP����
    private double currentStartDspTime;

    /// <summary>
    /// Button �Ȃǂ���Ăԓ����Đ�
    /// </summary>
    public void PlayAllScheduled()
    {
        // ���łɍĐ����Ȃ�~�߂�i�݌v����ō폜�j
        StopAllScheduled();

        // ������ DSP ���Ԃ��擾
        currentStartDspTime = AudioSettings.dspTime + scheduleOffset;

        // �S AudioSource �𓯈� DSP ���ԂōĐ��\��
        foreach (var src in audioSources)
        {
            if (src == null || src.clip == null)
                continue;

            src.PlayScheduled(currentStartDspTime);
        }
    }

    /// <summary>
    /// �S��~�i�����j
    /// </summary>
    public void StopAllScheduled()
    {
        foreach (var src in audioSources)
        {
            if (src == null)
                continue;

            src.Stop();
        }
    }

    public void WarmUpAudio()
    {
        foreach (var src in audioSources)
        {
            if (src == null || src.clip == null)
                continue;

            src.volume = 0f;
            src.Play();
            src.Stop();
            src.volume = 1f;
        }
    }
}
