using UnityEngine;
using UnityEngine.Playables;

public class ControllerUIInput : MonoBehaviour
{
    /// UI操作用のコントローラー入力管理クラス
    /// 回転入力によるUI選択移動と、各種ボタンによる決定処理を管理

    [SerializeField] ValueContainer valueContainer; // コントローラのパラメータ取得クラス
    [SerializeField] SelectButton selectButton;
    [SerializeField] PlayableDirector playableDirector;
    [SerializeField] ControllerInput controllerInput;

    /// <summary>
    /// 決定ボタンの種類
    /// </summary>
    public enum TapInput
    {
        UnderButton,// 下投げボタン
        DestroyButton,// 爆破ボタン
        EscButton,// エスケープボタン
    }

    [SerializeField, Header("入力チェック")]
    public bool InputCheck = false;

    [SerializeField, Header("決定ボタン")]
    TapInput tapInput;

    [SerializeField, Header("選択するための回転量")]
    float AngleLimit = 20f;

    // 回転角度関連の変数
    float angle;              // 現在の回転角度
    float beforeAngle;        // 前フレームの回転角度
    float angleValue;         // 累積回転量
    float currentAngleValue;  // 今フレームの回転差分

    // 前フレームの決定ボタン状態
    bool beforeEnter;

    /// <summary>
    /// 外部から現在の回転差分を取得
    /// </summary>
    public float GetAngle() => currentAngleValue;

    /// <summary>
    /// 入力チェックの有効/無効を設定
    /// </summary>
    public void SetInputCheck(bool check) => InputCheck = check;

    private void Start()
    {
        AngleReset();
        beforeEnter = false;
    }

    private void Update()
    {
        // 入力チェックが無効の場合は角度をリセットして終了
        if (!InputCheck)
        {
            AngleReset();
            return;
        }

        // Timeline再生中は入力を受け付けない
        if (playableDirector != null && playableDirector.state == PlayState.Playing)
            return;

        InputOperateUI();
    }

    /// <summary>
    /// 角度関連の変数を初期化
    /// </summary>
    private void AngleReset()
    {
        angle = 0;
        beforeAngle = 0;
        angleValue = 0;
    }

    /// <summary>
    /// タッチ入力処理のメイン関数
    /// 回転入力による選択移動と決定ボタンの処理
    /// </summary>
    private void InputOperateUI()
    {
        // ControllerInputが無効の場合は直接ValueContainerから取得
        if (controllerInput == null || controllerInput.enabled == false)
        {
            // アングルを加算
            angle -= valueContainer.get_rad();
            // radリセット処理
            valueContainer.reset_rad();
        }
        else 
        {
            // ControllerInputから角度を取得
            angle = controllerInput.GetAngle;
        }

        // 回転量をチェックして累積
        AngleCheck();

        // 累積回転量が閾値を超えたら選択移動
        if (AngleLimit < Mathf.Abs(angleValue))
        {
            int nextAddIndex = 0;

            // 回転方向に応じて移動方向を決定
            if (angleValue > 0)
                nextAddIndex = 1;

            if (angleValue < 0)
                nextAddIndex = -1;

            // 累積値をリセット(少し残す)
            angleValue *= 0.01f;

            // 選択ボタンを移動
            if (nextAddIndex != 0)
                selectButton.ButtonSelectMove(nextAddIndex);
        }

        // 決定ボタンの入力チェック
        bool Enter = EnterTap();

        // 押された瞬間のみ反応(エッジ検出)
        if (Enter && !beforeEnter)
            selectButton.EnterButton();

        beforeEnter = Enter;
        beforeAngle = angle;
    }

    /// <summary>
    /// 回転量の変化をチェックして累積
    /// 回転方向が反転した場合は累積値をリセット
    /// </summary>
    private void AngleCheck()
    {
        // 角度に変化がなければ終了
        if (beforeAngle == angle)
            return;

        // 今フレームの回転差分を計算
        currentAngleValue = beforeAngle - angle;

        // 前回と同じ方向(符号)かチェック
        bool sameSign = angleValue == 0f ||
            (currentAngleValue > 0f && angleValue > 0f) ||
            (currentAngleValue < 0f && angleValue < 0f);

        // 回転方向が反転したら累積値をリセット
        if (sameSign == false)
            angleValue = 0;

        // 回転量を累積
        angleValue += currentAngleValue;
        /*
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
        }*/
    }

    /// <summary>
    /// 設定された決定ボタンの入力状態を取得
    /// </summary>
    /// <returns>ボタンが押されていればtrue</returns>
    private bool EnterTap()
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
        else if(tapInput == TapInput.EscButton)
        {
            IsPush = valueContainer.get_esc() == 1;
        }
            return IsPush;
    }
}
