using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButtonPorse : SelectButton
{
    [SerializeField]
    Animator PorseAnimator;

    [SerializeField]
    string IntegerName;

    protected override void ButtonSelectoverride()
    {
        if(PorseAnimator != null)
        {
            PorseAnimator.SetInteger(IntegerName, currentButtonIndex);
        }
    }
}
