using UnityEngine;

public class PlayerFallSpeedAdder : MonoBehaviour
{
    /// プレイヤーの落下速度を段階的に加速させるクラス
    /// 落下中に時間経過と共に重力を強化し、よりスピードのある体験を演出

    [SerializeField] Player player;
    [SerializeField] Rigidbody PlayerRB; // プレイヤーのRigidbody
    [SerializeField,Header("落下速度加算機能の有効/無効フラグ")] bool FallFlag = true;
    [SerializeField,Header("デバッグログ出力フラグ")] bool PrintDebug = false;
    [SerializeField,Header("落下速度上限")] float FallSpeed = 30.0f;
    [SerializeField,Header("爆発から何秒後に加速するか")] float SecondsUntilFall = 3.5f;

    /// <summary>
    /// 外部から爆弾ヒットフラグを設定
    /// </summary>
    public bool _BombHit { set { BombHit = value; } }

    /// <summary>
    /// 急降下中かどうかを判定
    /// </summary>
    public bool IsFall => BombHitSwitch && BombHitCounter > SecondsUntilFall;

    float BombHitCounter; //爆発を受けてからのカウント
    bool BombHitSwitch = false;//爆弾がヒットしたことによる処理を行うための値
    bool BombHit = false;//爆弾がヒットしたかどうか（すぐにオフに戻る）

    float TimeCounter; // 落下継続時間のカウンター
    Vector3 BeforeVerocity; // 前フレームの速度（落下判定用）
    
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

    /// <summary>
    /// 爆弾ヒット後の経過時間管理と落下加速処理
    /// 一定時間後に急降下モードに移行し、徐々に加速
    /// </summary>
    void FixedUpdate()
    {
        // 機能が無効な場合は処理を終了
        if (!FallFlag)
            return;

        //爆弾がヒットしたら処理継続用のboolをONにしてヒット処理の方をOffに
        if (BombHit)
        {
            BombHitSwitch = true;
            BombHit = false;
            BombHitCounter = 0;
            TimeCounter = 0;
        }

        // 爆弾ヒット後の処理
        if (BombHitSwitch)
        {
            // ヒットからの経過時間を計測
            BombHitCounter += Time.fixedDeltaTime;

            // 指定時間経過後、急降下モードに移行
            if (BombHitCounter > SecondsUntilFall)
            {
                // 落下時間カウンターを増加（時間×倍率）
                TimeCounter += Time.fixedDeltaTime;

                // 数後に急降下する処理

                //現在地形に接している場合（接地判定）
                if (player.OnGround)
                {
                    // 着地したら全てリセット
                    BombHitSwitch = false;
                    BombHitCounter = 0;
                    TimeCounter = 0;
                }
                // 下方向に急加速する処理
                else
                {
                    // 追加重力の計算(時間経過で加速)
                    float AddAcceleration = TimeCounter;

                    // 下向きのベクトルを値で入力(最小値で制限)
                    // X・Z軸は減衰、Y軸は加速して下降
                    PlayerRB.velocity = new Vector3(PlayerRB.velocity.x * 0.99f, Mathf.Max(-FallSpeed, PlayerRB.velocity.y - AddAcceleration), PlayerRB.velocity.z * 0.99f);
             
                    // 落下時間カウンターを増加（時間×倍率）
                    TimeCounter += (TimeCounter * FallSpeed * 0.0007f);
                }
            }
        }

        // 次フレーム用に現在の速度を保存
        BeforeVerocity = PlayerRB.velocity;

        // デバッグ出力
        if (PrintDebug)
            Debug.Log(PlayerRB.velocity.y);
    }
}
