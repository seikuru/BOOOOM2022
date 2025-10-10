using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : ControllerTableInput
{
    [Header("ControllerInput")]
    [Space]
    [SerializeField] ValueContainer valueContainer; // コントローラのパラメータ取得クラス

    static readonly int NoTorchValue = -999;
    /// <summary>
    /// タッチ入力処理のメイン関数
    /// 基底クラスのInputOperateをオーバーライドしてマルチタッチ処理を実装
    /// </summary>
    protected override void InputOperate()
    {
        // テーブル回転処理
        float angle = valueContainer.get_rad();

        // radリセット処理
        valueContainer.reset_rad();

        RotateTable(angle);

        // 爆弾を投擲する処理
        int ThorwRad = valueContainer.get_Trad();
        // 押されていない数値ならreturn
        if (ThorwRad != NoTorchValue)
            ShotTable(ThorwRad);

        // 爆弾を爆破する処理
        bool IsPush = valueContainer.get_button() == 1;

        DestroyButtonCheck(IsPush);
    }
}
