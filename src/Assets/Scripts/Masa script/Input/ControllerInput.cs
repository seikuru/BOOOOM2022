using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : ControllerTableInput
{
    /// コントローラー入力を管理するクラス
    /// テーブルの回転、爆弾の投擲(内側/外側)、爆破ボタン、下投げなどの入力処理を統合管理

    [Header("ControllerInput")]
    [Space]
    [SerializeField] ValueContainer valueContainer; // コントローラのパラメータ取得クラス

    // タッチされていない状態を示す定数
    static readonly int NoTorchValue = -999;

    // 現在の回転角度
    float angle = 0;

    // 前フレームの入力角度を保持(内側・外側別)
    HashSet<int> InputAnglesInside, InputAnglesOutside;

    // 外部から角度を取得するプロパティ
    public float GetAngle => angle;

    /// <summary>
    /// タッチ入力処理のメイン関数
    /// 基底クラスのInputOperateをオーバーライドしてマルチタッチ処理を実装
    /// </summary>
    protected override void InputOperate()
    {
        // アングルを加算
        angle -= valueContainer.get_rad();
        //Debug.Log(angle);

        // radリセット処理
        valueContainer.reset_rad();

        if (Time.timeScale == 0f)
        {
            return;
        }

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

    /// <summary>
    /// 指定された側(内側/外側)の爆弾投擲処理
    /// 前フレームと比較して新規入力のみを投擲
    /// </summary>
    /// <param name="angleSet">前フレームの入力角度セット</param>
    /// <param name="thorwInputs">今フレームの入力角度リスト</param>

    private void ThorwBombSide(ref HashSet<int> angleSet, List<int> thorwInputs)
    {
        foreach (int thorwRad in thorwInputs)
        {
            if (ThorwRadCheck(angleSet, thorwRad))
                ShotTable(thorwRad, InputAnglesInside == angleSet);
        }

        BeforeAngleSave(ref angleSet, thorwInputs);
    }

    /// <summary>
    /// 投擲角度が有効かチェック
    /// NoTorchValueまたは前フレームで既に入力済みの場合はfalse
    /// </summary>
    /// <returns>投擲可能ならtrue</returns>
    private bool ThorwRadCheck(HashSet<int> beforeAngles, int rad)
    {
        // タッチされていない場合
        if (rad == NoTorchValue)
            return false;

        // 初回入力の場合
        if (beforeAngles == null)
            return true;

        // 前フレームに含まれていない新規入力のみtrue
        return !beforeAngles.Contains(rad);
    }

    /// <summary>
    /// 今フレームの入力角度を保存
    /// 次フレームで新規入力判定に使用
    /// </summary>
    private void BeforeAngleSave(ref HashSet<int> angleSet, List<int> inputs)
    {
        angleSet = new();

        // 有効な入力のみセットに追加
        foreach (var input in inputs)
        {
            if(input != NoTorchValue)
                angleSet.Add(input);
        }
    }
}
