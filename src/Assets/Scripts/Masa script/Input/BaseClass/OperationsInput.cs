using UnityEngine;

public class OperationsInput : MonoBehaviour
{
    /// 操作入力の基底クラス
    /// プレイヤーの基本アクション（回転、爆弾投擲）を管理し、継承先で具体的な入力処理を実装

    [Header("Operation")]
    [Space]
    [SerializeField] bomb BombClass; // 爆弾生成・投擲を管理するクラス
    [SerializeField] PlayerAction playerAction; // プレイヤーの回転アクションを管理するクラス

    void Update()
    {
        InputOperate();
    }

    /// <summary>
    /// 入力処理の仮想メソッド
    /// 継承先のクラスでオーバーライドして具体的な入力処理を実装
    /// </summary>
    protected virtual void InputOperate()
    {
        return;
    }

    /// <summary>
    /// プレイヤーのY軸回転を設定
    /// PlayerActionクラスのRotateBody_Yメソッドのラッパー
    /// </summary>
    /// <param name="angle">設定するY軸角度</param>
    protected void RotateAngle_Y(float angle) => playerAction.RotateBody_Y(angle);

    /// <summary>
    /// 下方向への爆弾投擲
    /// プレイヤーの足元に爆弾を設置する際に使用
    /// </summary>
    protected void ThrowUnderBomb()
    {
        BombClass.InstantiateUnder();
    }

    /// <summary>
    /// 方向指定での爆弾投擲
    /// 指定された方向にプレイヤーを向かせ、その方向に爆弾を投擲
    /// </summary>
    /// <param name="percentage">投擲力の強さ（0.0～1.0の範囲）</param>
    /// <param name="direction">投擲する方向ベクトル</param>
    protected void ThrowBomb(float percentage, Vector3 direction)
    {
        // 向いている方向に回転を合わせて投擲（direction方向に向ける）
        Quaternion lookDirection = Quaternion.LookRotation(direction, Vector3.up); // 方向からクォータニオンを生成

        playerAction.RecordingModelRotate(lookDirection); // プレイヤーモデルを段階的に回転

        BombClass.InstantiateBomb(percentage, direction, lookDirection); // 爆弾を生成・投擲
    }
}
