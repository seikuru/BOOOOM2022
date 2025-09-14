using UnityEngine;

public class UITableInput : OperationsInput
{
    /// UIテーブル（仮想ジョイスティック）を使用した操作入力クラス
    /// 円形のUIテーブルで回転操作と投擲操作を同時に制御
    /// 外側のテーブルで回転、内側のテーブルで投擲方向と力を決定
    
    [Header("UITable")]
    [Space]
    [SerializeField] RectTransform OutSideTableRect;// 外側テーブル（回転操作用）のRectTransform

    [SerializeField] RectTransform InSideTableRect;// 内側テーブル（投擲操作用）のRectTransform

    [SerializeField] float OutSideRange = 512f; // 外側テーブルの有効範囲（半径）

    [SerializeField] float InSideRange = 400f; // 内側テーブルの有効範囲（半径）

    [SerializeField] float SenterRange = 100f; // 中心部分の範囲（下方向投擲用）

    [SerializeField] float MapingClampMin = 0f; // 投擲力マッピングの最小値

    [SerializeField] float MapingClampMax = 2f; // 投擲力マッピングの最大値

    [SerializeField] bool SenterTapFlag = true; // 中心タップでの下方向投擲有効フラグ

    [SerializeField] Transform PlayerBodyTransform; // プレイヤー本体のTransform

    protected float MouseUpangle = 0; // マウスアップ時点の角度（回転基準値）

    protected float WidthClampMax = Screen.width; // 画面幅の最大値
    protected float HeightClampMax = Screen.height; // 画面高さの最大値

    /// <summary>
    /// オブジェクト有効化時の処理
    /// 画面サイズを更新し、UIテーブルの初期回転を設定
    /// </summary>
    private void OnEnable()
    {
        WidthClampMax = Screen.width; // 現在の画面幅を取得
        HeightClampMax = Screen.height; // 現在の画面高さを取得

        //　Startで呼ぶ想定だが、OnEnableでも問題なさそうならそのままで
        StartUIRotate(); // UIテーブルの初期回転を設定
    }

    /// <summary>
    /// 範囲の二乗値を計算
    /// 距離比較時の平方根計算を省略するための最適化
    /// </summary>
    /// <param name="range">範囲の値</param>
    /// <returns>範囲の二乗値</returns>
    float TableRectRangePow(float range)
    {
        return range * range;
    }

    /// <summary>
    /// 2点間の2D方向ベクトルを計算
    /// </summary>
    /// <param name="from">開始点</param>
    /// <param name="to">終了点</param>
    /// <returns>2D方向ベクトル</returns>
    Vector2 GetDirection2D(Vector3 from, Vector3 to)
    {
        return new Vector2(to.x - from.x, to.y - from.y);
    }

    /// <summary>
    /// 2点間の距離の二乗を計算
    /// 平方根計算を省略して処理速度を向上
    /// </summary>
    /// <param name="vector_Senter">中心点</param>
    /// <param name="vector_Touch">タッチ点</param>
    /// <returns>距離の二乗値</returns>
    float TableDistansePow(Vector3 vector_Senter, Vector3 vector_Touch)
    {
        Vector2 Direction2D = GetDirection2D(vector_Senter, vector_Touch);

        return Direction2D.x * Direction2D.x + Direction2D.y * Direction2D.y;
    }

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
    float MapingClamp(float value, float min1, float max1, float min2, float max2)
    {
        return (value - min1) * (max2 - min2) / (max1 - min1) + min2;
    }

    /// <summary>
    /// 入力位置が外側テーブルの有効範囲内かを判定
    /// </summary>
    /// <param name="input">入力位置</param>
    /// <returns>範囲内の場合true</returns>
    protected bool IsInOuterTableRect(Vector3 input)
    {
        return TableDistansePow(input, OutSideTableRect.position) < TableRectRangePow(OutSideRange);
    }

    // <summary>
    /// UIテーブルの初期回転を設定
    /// プレイヤーの現在の回転に合わせてテーブルを初期化
    /// </summary>
    void StartUIRotate()
    {
        // プレイヤーのY軸角度を取得
        float yAngle = PlayerBodyTransform.localEulerAngles.y; 

        // 負の角度を正の角度に変換
        if (yAngle < 0)
            yAngle += 360f;

        // テーブルのZ軸回転を設定（画面上での回転表現）
        OutSideTableRect.eulerAngles = new(0, 0, yAngle);

        MouseUpangle = yAngle; // 基準角度として保存
    }

    /// <summary>
    /// テーブル回転処理
    /// ドラッグ操作に基づいてテーブルとプレイヤーを回転させる
    /// </summary>
    /// <param name="DownPosition">ドラッグ開始位置</param>
    /// <param name="StayPosition">現在のドラッグ位置</param>
    protected void RotateTable(Vector3 DownPosition, Vector3 StayPosition)
    {
        // テーブル中心からタッチ開始位置までの距離を測り、一定範囲外なら処理をしない
        if (IsInOuterTableRect(DownPosition) == false)
            return;

        // 開始位置から中心へのベクトルを取得
        Vector2 dirA = GetDirection2D(DownPosition, OutSideTableRect.position);

        // 現在位置から中心へのベクトルを取得
        Vector2 dirB = GetDirection2D(StayPosition, OutSideTableRect.position);

        // ベクトルAからBへの角度差
        float angle = Vector2.SignedAngle(dirA, dirB);

        // マウスアップ時点の角度に今回の回転分を加えた絶対角度を計算
        float absoluteAngle = MouseUpangle + angle;

        // テーブル（OutSideTableRect）のZ軸回転に反映（画面上の回転）
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, absoluteAngle);
        OutSideTableRect.rotation = targetRotation;

        // オブジェクトにY軸回転を適用(現在は逆方向)
        RotateAngle_Y(-absoluteAngle);
    }

    /// <summary>
    /// 投擲処理
    /// 内側テーブルでのタッチ位置に基づいて爆弾を投擲
    /// 距離で力を決定し、方向で投擲方向を決定
    /// </summary>
    /// <param name="TouchDownPos">タッチ位置</param>
    protected void ShotTable(Vector3 TouchDownPos)
    {
        // 入力位置と中心位置の距離の2乗
        float distansPow = TableDistansePow(TouchDownPos, InSideTableRect.position);

        // 範囲外除外
        if (distansPow > TableRectRangePow(InSideRange))
            return;

        // 入力位置と中心位置の距離
        float distance = Mathf.Sqrt(distansPow);

        // 再度範囲外除外
        if (distance > HeightClampMax / 2)
        {
            Debug.Log("再度範囲外除外が実行された");
            return;
        }

        // 中心部分の入力を受け付けるか
        // 中心との距離によって中心の処理に切り替え
        if (SenterTapFlag && distansPow < TableRectRangePow(SenterRange))
        {
            ThrowUnderBomb();// 足元に爆弾を設置
            return;
        }

        // 距離に応じて強さを一定範囲内になるように計算
        float powerRange = MapingClamp(distance, SenterTapFlag ? SenterRange : 0, HeightClampMax / 2, MapingClampMin, MapingClampMax);

        // UI空間上での方向（画面上でのベクトル）
        Vector2 uiDirection = (TouchDownPos - InSideTableRect.position).normalized;

        // この2D方向を3D空間のローカル方向として解釈
        Vector3 localDirection = new Vector3(uiDirection.x, 0f, uiDirection.y);

        // プレイヤーのY軸方向を考慮した回転（ローカル → ワールド）
        Vector3 worldDirection = PlayerBodyTransform.rotation * localDirection;

        // powerRange が小さいほど Y軸方向の影響を減らす
        // Y軸方向とは、プレイヤーの上下方向の影響を抑えるために、worldDirection を水平に近づける
        worldDirection.y *= powerRange;

        // 正規化して力の方向を保持
        worldDirection = worldDirection.normalized;

        // 投擲
        ThrowBomb(powerRange, worldDirection);
    }

    /// <summary>
    /// テーブル角度の更新
    /// 操作終了時に現在の角度を基準値として保存
    /// </summary>
    protected void UpdateAngleTableRect()
    {
        // 現在の角度を保存を行う
        MouseUpangle = OutSideTableRect.eulerAngles.z;
    }
}
