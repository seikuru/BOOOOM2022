using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectButton : MonoBehaviour
{
    [SerializeField] protected Button[] Buttons;
    [SerializeField] protected GameObject cursol;
    [SerializeField] protected AudioSource audioSE;
    [SerializeField] protected AudioClip audioClipUp;
    [SerializeField] protected AudioClip audioClipDown;
    [SerializeField] protected int currentButtonIndex = 0;

    static readonly int ResetIndex = -99;

    /// <summary>
    /// ƒ{ƒ^ƒ“‚ÌIndex‚ðˆÚ“®‚³‚¹‚é
    /// </summary>
    /// <param name="buttonIndexAdd">ˆÊ’u</param>
    public virtual void ButtonSelectMove(int buttonIndexAdd)
    {
        if (Buttons.Length < 2)
            return;

        if (audioSE != null && buttonIndexAdd != ResetIndex)
        {
            if (buttonIndexAdd < 0 && audioClipUp != null)
            {
                audioSE.PlayOneShot(audioClipUp);
            }
            if (buttonIndexAdd > 0 && audioClipDown != null) 
            {
                audioSE.PlayOneShot(audioClipDown);
            }
        }

        //Debug.Log(currentButtonIndex);
        currentButtonIndex += buttonIndexAdd;
        bool sameCheck = false;

        if (currentButtonIndex < 0)
        {
            currentButtonIndex = 0; sameCheck = true;
        }

        if(currentButtonIndex >= Buttons.Length)
        {
            currentButtonIndex = Buttons.Length - 1; sameCheck = true;
        }
        //Debug.Log(currentButtonIndex);

        if (cursol != null)
            cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;

        if(!sameCheck)
            ButtonSelectoverride();
    }

    protected virtual void ButtonSelectoverride()
    {
        return;
    }

    public void EnterButton()
    {
        ButtonInvoke();
    }

    private void ButtonInvoke()
    {
        Buttons[currentButtonIndex].onClick.Invoke();
    }

    public void SetIndex(int num) => currentButtonIndex = num;

    protected virtual void DebugInput()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ButtonSelectMove(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ButtonSelectMove(-1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ButtonInvoke();
        }
#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        DebugInput();
#endif
    }
}
