using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : ControllerTableInput
{
    [Header("ControllerInput")]
    [Space]
    [SerializeField] ValueContainer valueContainer; // コントローラのパラメータ取得クラス

    static readonly int NoTorchValue = -999;

    float angle = 0;

    HashSet<int> InputAnglesInside, InputAnglesOutside;

    public float GetAngle => angle;
    /// <summary>
    /// タッチ入力処理のメイン関数
    /// 基底クラスのInputOperateをオーバーライドしてマルチタッチ処理を実装
    /// </summary>
    protected override void InputOperate()
    {
        // アングルを加算
        angle -= valueContainer.get_rad();

        // radリセット処理
        valueContainer.reset_rad();

        // テーブル回転処理
        RotateTable(angle);

        // 爆弾を爆破する処理
        bool IsPush = valueContainer.get_button() == 1;
        DestroyButtonCheck(IsPush);

        // 下投げ処理
        if (valueContainer.get_under())
            ShotUnder();

        // テーブル内側の投げ処理
        ThorwBombSide(ref InputAnglesInside,valueContainer.get_inRad());

        // テーブル外側の投げ処理
        ThorwBombSide(ref InputAnglesOutside, valueContainer.get_outRad());
        
        /*
        // 爆弾を投擲する処理
        int ThorwRad = valueContainer.get_Trad();
        // 押されていない数値ならreturn
        if (ThorwRad != NoTorchValue)
            ShotTable(ThorwRad);
        */
    }

    void ThorwBombSide(ref HashSet<int> angleSet, List<int> thorwInputs)
    {
        foreach (var thorwRad in thorwInputs)
        {
            if (ThorwRadCheck(ref angleSet, thorwRad))
                ShotTable(thorwRad);
        }

        BeforeAngleSave(ref angleSet, thorwInputs);
    }

    bool ThorwRadCheck(ref HashSet<int> beforeAngles, int rad)
    {
        if (rad == NoTorchValue)
            return false;

        if (beforeAngles == null)
            return true;

        return !beforeAngles.Contains(rad);
    }

    void BeforeAngleSave(ref HashSet<int> angleSet, List<int> inputs)
    {
        angleSet = new();

        foreach (var input in inputs)
        {
            if(input == NoTorchValue)
                angleSet.Add(input);
        }
    }
}
