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
    [SerializeField] protected int currentButtonIndex = 0;

    /// <summary>
    /// ƒ{ƒ^ƒ“‚ÌIndex‚ðˆÚ“®‚³‚¹‚é
    /// </summary>
    /// <param name="buttonIndexAdd">ˆÊ’u</param>
    public virtual void ButtonSelectMove(int buttonIndexAdd)
    {
        currentButtonIndex += buttonIndexAdd;

        if (currentButtonIndex < 0)
            currentButtonIndex = 0;

        if(currentButtonIndex >= Buttons.Length)
            currentButtonIndex = Buttons.Length - 1;

         //audioSE?.PlayOneShot(audioSE.clip);

        if(cursol != null)
            cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;

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
}
