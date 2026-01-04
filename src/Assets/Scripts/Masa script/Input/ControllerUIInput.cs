using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ControllerUIInput : MonoBehaviour
{
    [SerializeField] ValueContainer valueContainer; // コントローラのパラメータ取得クラス
    [SerializeField] SelectButton selectButton;
    [SerializeField] PlayableDirector playableDirector;

    public enum TapInput
    {
        UnderButton,
        DestroyButton,
    }

    [SerializeField, Header("入力チェック")]
    public bool InputCheck = false;

    [SerializeField, Header("決定ボタン")]
    TapInput tapInput;

    [SerializeField, Header("選択するための回転量")]
    float AngleLimit = 20f;

    float angle, beforeAngle, AngleValue;
    bool BeforeEnter;

    public float GetAngle() => angle;

    public void SetInputCheck(bool b) => InputCheck = b;

    private void Start()
    {
        angle = 0;
        beforeAngle = 0;
        AngleValue = 0;
        BeforeEnter = false;
    }

    private void Update()
    {
        if (!InputCheck)
            return;

        if (playableDirector != null && playableDirector.state == PlayState.Playing)
            return;

        InputOperateUI();
    }

    /// <summary>
    /// タッチ入力処理のメイン関数
    /// </summary>
    void InputOperateUI()
    {
        // アングルを加算
        angle -= valueContainer.get_rad();
        // radリセット処理
        valueContainer.reset_rad();

        AngleCheck();

        if (AngleLimit < Mathf.Abs(AngleValue))
        {
            int nextAddIndex = 0;

            if (AngleLimit > 0)
                nextAddIndex = 1;

            if (AngleLimit < 0)
                nextAddIndex = -1;

            AngleValue *= 0.01f;

            if (nextAddIndex != 0)
                selectButton.ButtonSelectMove(nextAddIndex);
        }

        bool Enter = EnterTap();

        if (Enter && !BeforeEnter)
            selectButton.EnterButton();

        BeforeEnter = Enter;

        beforeAngle = angle;
    }

    void AngleCheck()
    {
        if (beforeAngle == angle)
            return;

        if (beforeAngle < angle)
        {
            if (AngleValue < 0)
                AngleValue = 0;

            AngleValue += Mathf.Abs(angle - beforeAngle);
        }

        if (beforeAngle > angle)
        {
            if (AngleValue > 0)
                AngleValue = 0;

            AngleValue -= Mathf.Abs(beforeAngle - angle);
        }
    }

    bool EnterTap()
    {
        bool IsPush = false;

        if (tapInput == TapInput.UnderButton)
        {
            IsPush = valueContainer.get_under();
        }
        else if (tapInput == TapInput.DestroyButton)
        {
            IsPush = valueContainer.get_button() == 1;
        }

        return IsPush;
    }
}
