using UnityEngine;

public class PlayerFallSpeedAdder : MonoBehaviour
{
    /// プレイヤーの落下速度を段階的に加速させるクラス
    /// 落下中に時間経過と共に重力を強化し、よりスピードのある体験を演出

    [SerializeField] Rigidbody PlayerRB; // プレイヤーのRigidbody
    [SerializeField] bool FallFlag = true; // 落下速度加算機能の有効/無効フラグ
    [SerializeField] float MaxAddFallSpeed = 13f; // 追加できる最大落下速度
    [SerializeField] float BaseGravityAcceleration = 9.8f; // 基本重力加速度
    [SerializeField] float AddFallValue = 2f; // 落下時間カウンターの増加倍率
    [SerializeField] bool PrintDebug = false; // デバッグログ出力フラグ



    /// ある特定の条件を満たしたら下方向に急加速する処理を追加実装

    [SerializeField] float FallSpeed = 1.0f;
    [SerializeField] float FallJudgeTime = 3.0f;
    float FallJudgeValue = 1.0f;//落下していると判定する下向きのベクトルの強さの値


    /// 爆弾の爆発を受けてから特定秒数後に急降下する処理を追加

    [SerializeField] float SecondsUntilFall = 3.5f;//爆発から何秒後に加速するか
    float BombHitCounter; //爆発を受けてからのカウント
    bool BombHitSwitch = false;//爆弾がヒットしたことによる処理を行うための値
    bool BombHit = false;//爆弾がヒットしたかどうか（すぐにオフに戻る）
    public bool _BombHit { set { BombHit = value; } }


    
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

        //爆弾がヒットしたら処理継続用のboolをONにしてヒット処理の方をOffに
        if (BombHit)
        {
            BombHitSwitch = true;
            BombHit = false;
            BombHitCounter = 0;
            TimeCounter = 0;
        }

        if (BombHitSwitch)
        {
            BombHitCounter += Time.deltaTime;

            if (BombHitCounter > SecondsUntilFall)
            {

                // 落下時間カウンターを増加（時間×倍率）
                TimeCounter += Time.deltaTime;

                //現在の垂直速度が前フレームよりも大きい場合（接地判定）
                if (PlayerRB.velocity.y > BeforeVerocity.y)
                {
                    BombHitSwitch = false;
                    BombHitCounter = 0;
                    TimeCounter = 0;
                    //PlayerRB.velocity = new Vector3(PlayerRB.velocity.x * 0.1f, PlayerRB.velocity.y, PlayerRB.velocity.z * 0.1f);
                }
                else 
                {

                // 追加重力の計算
                float AddAcceleration = TimeCounter ;

                //下向きのベクトルを値で入力（最小値で制限）
                PlayerRB.velocity = new Vector3(PlayerRB.velocity.x * 0.99f, Mathf.Max(-FallSpeed, PlayerRB.velocity.y - AddAcceleration), PlayerRB.velocity.z * 0.99f);
                  //PlayerRB.velocity = new Vector3(PlayerRB.velocity.x, Mathf.Max(-FallSpeed, PlayerRB.velocity.y - AddAcceleration), PlayerRB.velocity.z);

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

        //HighSpeedFall();
        //FallAccelerate();
    }

    void FallAccelerate()
    {
        // 落下判定（現在の垂直速度が前フレームより小さい場合）
        if (PlayerRB.velocity.y < BeforeVerocity.y)
        {
            // 落下時間カウンターを増加（時間×倍率）
            TimeCounter += Time.deltaTime * AddFallValue;

            // デバッグ出力
            if (PrintDebug)
                ;

        }
        else // 上昇または速度維持の場合
        {
            TimeCounter = 0; // 時間カウンターをリセット
        }

        // 追加重力の計算（最大値で制限）
        float AddAcceleration = Mathf.Min(MaxAddFallSpeed, TimeCounter);

        // 基本重力 + 追加重力を下向きに適用
        PlayerRB.AddForce(Vector3.down * (BaseGravityAcceleration + AddAcceleration * AddAcceleration * AddAcceleration), ForceMode.Acceleration);

        // 次フレーム用に現在の速度を保存
        BeforeVerocity = PlayerRB.velocity;
    }

    void HighSpeedFall()
    {
        // 落下判定(落下速度が規定値を下回った時)
        if (PlayerRB.velocity.y < -FallJudgeValue)
        {

            // 追加重力の計算（最大値で制限）
            float AddAcceleration = Mathf.Min(FallSpeed, TimeCounter + FallJudgeValue);

            //下向きのベクトルを値で入力
            PlayerRB.velocity = new Vector3(PlayerRB.velocity.x, -AddAcceleration, PlayerRB.velocity.z);

            // デバッグ出力
            if (PrintDebug)
                ;

            // 落下時間カウンターを増加（時間×倍率）
            TimeCounter += (TimeCounter * FallSpeed * 0.0007f) + FallJudgeValue;

        }
        else // 上昇または速度維持の場合
        {
            TimeCounter = 0; // 時間カウンターをリセット
        }
    }
}
