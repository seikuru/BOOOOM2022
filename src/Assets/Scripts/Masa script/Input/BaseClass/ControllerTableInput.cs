using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class ControllerTableInput : OperationsInput
{
    /// コントローラテーブル（外部から通信）を使用した操作入力クラス
    /// テーブルで回転操作と投擲操作を同時に制御
    /// 外側のテーブルで回転、内側のテーブルで投擲方向を決定
    /// ボタンは別途、入力を取得
    /// パラメータはValueContainerクラスから取得

    [Header("ControllerTable")]
    [Space]
    [SerializeField] Transform FollowPointTransform; //カメラ制御用Transform

    [SerializeField] float ThrowPowerInside = 0.5f; // 内側の投擲力

    [SerializeField] float ThrowPowerOutside = 1f; // 外側の投擲力

    protected float UpAngle = 0; // アップ時点の角度（回転基準値）

    bool IsPushing;

    /// <summary>
    /// 値の範囲を別の範囲にマッピング
    /// 距離を投擲力にマッピングする際に使用
    /// </summary>
    /// <param name="value">マッピング対象の値</param>
    /// <param name="min1">元の範囲の最小値</param>
    /// <param name="max1">元の範囲の最大値</param>
    /// <param name="min2">新しい範囲の最小値</param>
    /// <param name="max2">新しい範囲の最大値</param>
    /// <returns>マッピング後の値</returns>
    float MappingClamp(float value, float min1, float max1, float min2, float max2)
    {
        float mapping = (value - min1) * (max2 - min2) / (max1 - min1) + min2;

        return mapping;
    }

    /// <summary>
    /// オブジェクト有効化時の処理
    /// UIテーブルの初期回転を設定
    /// </summary>
    private void OnEnable()
    {
        IsPushing = false;

        //　Startで呼ぶ想定だが、OnEnableでも問題なさそうならそのままで
        StartUIRotate(); // UIテーブルの初期回転を設定
    }

    // <summary>
    /// UIテーブルの初期回転を設定
    /// プレイヤーの現在の回転に合わせてテーブルを初期化
    /// </summary>
    void StartUIRotate()
    {

        Transform parent = FollowPointTransform.parent;

        // プレイヤーのY軸角度を取得
        float yAngle = parent.localEulerAngles.y;
        // プレイヤーのY軸角度を取得
        //float yAngle = FollowPointTransform.localEulerAngles.y;

        // 負の角度を正の角度に変換
        if (yAngle < 0)
            yAngle += 360f;

        // テーブルのZ軸回転を設定（画面上での回転表現）
        //OutSideTableRect.eulerAngles = new(0, 0, yAngle);

        UpAngle = yAngle; // 基準角度として保存
    }

    /// <summary>
    /// テーブル回転処理
    /// テーブルの回転操作に基づいてプレイヤーを回転させる
    /// </summary>
    /// <param name="angle">回転アングルの値</param>
    protected void RotateTable(float angle)
    {
        // マウスアップ時点の角度に今回の回転分を加えた絶対角度を計算
        float absoluteAngle = UpAngle + angle;

        // テーブル（OutSideTableRect）のZ軸回転に反映（画面上の回転）
        //Quaternion targetRotation = Quaternion.Euler(0f, 0f, absoluteAngle);
        //OutSideTableRect.rotation = targetRotation;

        // オブジェクトにY軸回転を適用(現在は逆方向)
        RotateAngle_Y(-absoluteAngle);
    }

    /// <summary>
    /// 投擲処理
    /// 内側テーブルでのタッチ位置に基づいて爆弾を投擲
    /// 距離は現在は固定、方向パラメータで投擲方向を決定
    /// </summary>
    /// <param name="ThorwRad">投擲アングルの値</param>
    /// <param name="IsInside">内側の入力かどうか(内側ならtrue)</param>
    protected void ShotTable(int ThorwRad ,bool IsInside)
    {
        float rad = ThorwRad * Mathf.Deg2Rad;

        // あとはそのまま使用
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        /*
        // 中心部分の入力を受け付けるか
        // 中心との距離によって中心の処理に切り替え
        if (SenterTapFlag && distansPow < TableRectRangePow(SenterRange))
        {
            ThrowUnderBomb();// 足元に爆弾を設置
            return;
        }
        */

        // 距離に応じて強さを一定範囲内になるように計算
        // (今回は投げる力は一定にしておく)
        float powerRange = IsInside ? ThrowPowerInside : ThrowPowerOutside;

        // この2D方向を3D空間のローカル方向として解釈
        Vector3 localDirection = new Vector3(dir.x, 0f, dir.y);

        // プレイヤーのY軸方向を考慮した回転（ローカル → ワールド）
        Vector3 worldDirection = FollowPointTransform.rotation * localDirection;

        // powerRange がの大きさによってY軸方向の影響を変化させる
        // Y軸方向が、小さくなる程、worldDirection を水平に近づける
        // worldDirection.y *= powerRange;

        // 正規化して力の方向を調整して補正
        worldDirection = worldDirection.normalized;

        // 投擲
        ThrowBomb(powerRange, worldDirection);
    }
    /// <summary>
    /// 足元に爆弾を設置
    /// </summary>
    protected void ShotUnder() => ThrowUnderBomb();

    /// <summary>
    /// 爆弾を爆発させるボタンが押されているかチェックする
    /// bool変数を挟んで押したときに一回だけ起動するように
    /// </summary>
    /// <param name="isPush">押してあるなら true</param>
    protected void DestroyButtonCheck(bool isPush)
    {
        if(isPush == true && IsPushing == false)
        {
            IsPushing = true;
            BombsDestroy();
        }
        else if(isPush == false && IsPushing == true)
        {
            IsPushing = false;
        }
    }
}
