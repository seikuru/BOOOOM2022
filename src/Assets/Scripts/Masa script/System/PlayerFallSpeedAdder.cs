using UnityEngine;

public class PlayerFallSpeedAdder : MonoBehaviour
{
    /// プレイヤーの落下速度を段階的に加速させるクラス
    /// 落下中に時間経過と共に重力を強化し、よりスピードのある体験を演出

    [SerializeField] Rigidbody PlayerRB; // プレイヤーのRigidbody
    [SerializeField] bool FallFlag = true; // 落下速度加算機能の有効/無効フラグ
    [SerializeField] float MaxAddFallSpeed = 13f; // 追加できる最大落下速度
    [SerializeField] float BaseGrabityAcceleration = 9.8f; // 基本重力加速度
    [SerializeField] float AddFallValue = 2f; // 落下時間カウンターの増加倍率
    [SerializeField] bool PrintDebug = false; // デバッグログ出力フラグ

    
    float TimeCounter; // 落下継続時間のカウンター
    Vector3 BeforeVerocity; // 前フレームの速度（落下判定用）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 時間カウンターを初期化
        TimeCounter = 0;

        // Rigidbodyコンポーネントがない場合の再取得
        if (TryGetComponent<Rigidbody>(out var component))
        {
            PlayerRB = component;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 機能が無効な場合は処理を終了
        if (!FallFlag)
            return; 

        // 落下判定（現在の垂直速度が前フレームより小さい場合）
        if (PlayerRB.linearVelocity.y < BeforeVerocity.y)
        {
            // 落下時間カウンターを増加（時間×倍率）
            TimeCounter += Time.deltaTime * AddFallValue;

            // デバッグ出力
            if (PrintDebug) 
                Debug.Log("Fall"); 
        }
        else // 上昇または速度維持の場合
        {
            TimeCounter = 0; // 時間カウンターをリセット
        }

        // 追加重力の計算（最大値で制限）
        float AddAcceleration = Mathf.Min(MaxAddFallSpeed, TimeCounter);

        // 基本重力 + 追加重力を下向きに適用
        PlayerRB.AddForce(Vector3.down * (BaseGrabityAcceleration + AddAcceleration), ForceMode.Acceleration);

        // 次フレーム用に現在の速度を保存
        BeforeVerocity = PlayerRB.linearVelocity;
    }
}
