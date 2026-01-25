using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemaChineCameraAngle : MonoBehaviour
{
    /// プレイヤーの状態に応じてCinemachineカメラのアングルと距離を動的に調整するクラス
    /// 落下時・上昇時・爆発時にカメラの角度、距離、FOVを変化させる

    [SerializeField] Player player;
    [SerializeField] Rigidbody PlayerRB;
    [SerializeField] CinemachineVirtualCamera VirtualCamera;
    [Space]
    [SerializeField, Header("カメラまでの基本距離")] float DefaltDistance = 5f;

    [SerializeField, Header("カメラの最大角度")] float MaxAngle = 80f;
    [SerializeField, Header("カメラの最小角度")] float MinAngle = 10f;
    [SerializeField, Header("落下時のアングル変更速度")] float AngleChengeSpeedFall = 60f;
    [SerializeField, Header("上昇時のアングル変更速度")] float AngleChengeSpeedUp = 100f;

    [SerializeField, Header("落下速度計算上限")] float FallSpeedLimit = 50f;

    [SerializeField, Header("上昇量計算上限")] float UpPosLimit = 40f;
    [SerializeField, Header("上昇によるカメラのLeapの倍率")
        , Range(0f, 1f)]
    float UpCameraDiameter = 0.8f;

    [SerializeField, Header("平面移動量計算上限")] float FlatVerocityLimit = 60f;
    [SerializeField, Header("平面移動によるカメラのLeapの倍率")
        , Range(0f, 1f)]
    float FlatCameraDiameter = 0.35f;

    [Header("爆発によるカメラの引き")]

    [SerializeField, Header("補完する時の最大倍率")] float CompletionMaxDiameter = 1.1f;

    [SerializeField, Header("補完時間_引き")] float CompletionTimeBack = 0.1f;
    [SerializeField, Header("補完時間_最大値維持")] float CompletionTimeMaxValue = 0.15f;
    [SerializeField, Header("補完時間_縮み")] float CompletionTimeForward = 0.7f;

    [Header("落下によるカメラの引き")]
    [SerializeField, Header("落下時の引きの最大倍率")] float FallMaxDiameter = 1.4f;

    [SerializeField, Header("落下してからの待機時間")] float FallWaitTime = 0.02f;
    [SerializeField, Header("落下時の引き時間倍率")] float FallBackDiameter = 0.5f;
    [SerializeField, Header("落下してない時の縮み時間倍率")] float FallForwardDiameter = 2f;

    [Header("落下によるカメラのFov")]

    [SerializeField, Header("Fovの最大値")] float　FallMaxFov = 90f;
    [SerializeField, Header("Fovの最小値")] float FallMinFov = 60f;

    // Cinemachineのトランスポーザーコンポーネント
    CinemachineTransposer transposer;
    // アングル補間用コルーチンの参照
    Coroutine Coroutine_TargetAngle;

    // 目標カメラアングル
    float TargetAngle;
    // 爆発による追加アングル補正値
    float AddAngleforward;
    // 落下による追加距離補正値
    float AddFallforward;

    // 落下状態の継続時間
    float FallStateTime;

    // 前フレームの接地状態
    bool BeforeGround;
    // 地面から離れた時のY座標
    float ExitGroundPos = 0;
    // 上昇時のClamp値の最大保持用
    float UpClampValue = 0;

    /// <summary>
    /// 二つのベクトルをXZ平面上で角度を評価
    /// </summary>
    /// <param name="a">基準ベクトル</param>
    /// <param name="b">対象ベクトル</param>
    /// <returns>直角ほど 0、平行ほど 1</returns>
    float GetRightAngleFactor(Vector3 a, Vector3 b)
    {
        // XZ 平面に投影
        Vector2 a2 = new Vector2(a.x, a.z).normalized;
        Vector2 b2 = new Vector2(b.x, b.z).normalized;

        // cosθ（＝dot）を求める
        float dot = Vector2.Dot(a2, b2);

        // 0°→1, 90°→0, 180°→1 に変換
        return Mathf.Abs(dot);
    }

    /// <summary>
    /// 値を指定範囲で正規化し0-1にクランプ
    /// </summary>
    float NormalizeClamp(float value, float min, float max)
    {
        return Mathf.Clamp01((value - min) / (max - min));
    }

    /// <summary>
    /// 角度(度)から2D方向ベクトルを取得
    /// </summary>
    Vector2 DirFromAngle(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    /// <summary>
    /// 接地状態の変化を検知し、必要な値を更新
    /// </summary>
    void GroundCheck()
    {
        // 接地→空中への遷移時、離陸位置を記録
        if (BeforeGround && !player.OnGround)
        {
            ExitGroundPos = transform.position.y;
        }
        // 着地時は上昇値をリセット
        if (player.OnGround)
        {
            UpClampValue = 0;
        }

        BeforeGround = player.OnGround;
    }

    void Start()
    {
        transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // 初期値設定
        TargetAngle = MinAngle;
        AddAngleforward = 0f;
        AddFallforward = 0f;

        FallStateTime = 0f;
    }

    void FixedUpdate()
    {
        // カメラアングルの計算と更新
        // プレイヤーの速度・高度に応じて目標アングルを決定
        if (transposer == null)
            return;

        GroundCheck();

        float clamp;
        float LerpAngle;

        // XZ平面での速度を計算
        float VerocityMagnitudeFlat = new Vector2(PlayerRB.velocity.x, PlayerRB.velocity.z).magnitude;

        // 平面移動速度による補正値
        float clampFlat = NormalizeClamp(VerocityMagnitudeFlat, 0, FlatVerocityLimit) * FlatCameraDiameter;

        // 空中かつ上昇中の処理
        if (!player.OnGround && PlayerRB.velocity.y >= 0)
        {
            // 離陸位置からの上昇距離を計算
            float distance_y = Mathf.Max(Player.GetTransformPlayer.position.y - ExitGroundPos, 0);

            clamp = NormalizeClamp(distance_y, 0, UpPosLimit) * UpCameraDiameter;

            // 上昇時の最大値を保持(下降時も維持するため)
            UpClampValue = Mathf.Max(clamp, UpClampValue);

            LerpAngle = MinAngle + (MaxAngle - MinAngle) * Mathf.Max(clamp - clampFlat, 0);

            FallStateTime = 0;
        }
        // 落下中または接地時の処理
        else
        {
            // 落下速度による補正値
            clamp = NormalizeClamp(PlayerRB.velocity.y, -FallSpeedLimit, 0);

            // 上昇時の値と比較して大きい方を使用
            clamp = Mathf.Max(1 - clamp, UpClampValue);

            LerpAngle = MinAngle + (MaxAngle - MinAngle) * Mathf.Max(clamp - clampFlat, 0);

            // 落下時間を計測
            if (PlayerRB.velocity.y < 0)
                FallStateTime += Time.fixedDeltaTime;
            else
                FallStateTime = 0;
        }

        // 目標角度への補間(上昇・落下で速度が異なる)
        float FixAngle = TargetAngle;     

        if (TargetAngle < LerpAngle)
        {
            // 角度を上げる(落下時)
            FixAngle = Mathf.Min(TargetAngle + AngleChengeSpeedFall * Time.fixedDeltaTime, LerpAngle);
        }
        else if (TargetAngle > LerpAngle)
        {
            // 角度を下げる(上昇時)
            FixAngle = Mathf.Max(TargetAngle - AngleChengeSpeedUp * Time.fixedDeltaTime, LerpAngle);
        }

        TargetAngle = FixAngle;

        // 落下によるカメラ引きの補正値を更新
        if (FallStateTime >= FallWaitTime)
            AddFallforward = Mathf.Min(1, AddFallforward + Time.fixedDeltaTime * FallBackDiameter);
        else
            AddFallforward = Mathf.Max(0, AddFallforward - Time.fixedDeltaTime * FallForwardDiameter);
    }

    void Update()
    {
        // カメラのオフセット位置とFOVを適用

        // 角度から2Dオフセットを計算
        Vector2 TargetOffSet2D = DirFromAngle(TargetAngle) * DefaltDistance;

        // 3D空間でのオフセットに変換(Y方向が高さ、Z方向が距離)
        Vector3 TargetOffSet = new(0,TargetOffSet2D.y, -TargetOffSet2D.x);

        // 爆発と落下による追加オフセット倍率を計算
        float AddOffSetValue = (1 + (AddAngleforward - 1) + (AddFallforward * FallMaxDiameter));

        // カメラオフセットを設定
        transposer.m_FollowOffset = TargetOffSet + TargetOffSet * AddOffSetValue;

        // 落下時のFOV変化を適用
        VirtualCamera.m_Lens.FieldOfView = FallMinFov + AddFallforward *(FallMaxFov - FallMinFov);
    }

    void LateUpdate()
    {
        // 爆発ヒット時のカメラ引きエフェクトを開始

        if (!player.PlayerBombHit)
        {
            return;
        }

        // フラグ解除
        player.PlayerBombHit = false;

        // プレイヤーの移動方向とカメラ方向の平行度を計算
        float AngleFactor = GetRightAngleFactor(PlayerRB.velocity, Player.GetTransformPlayer.position - transform.position);

        // 平行なほど大きな引きエフェクトを適用
        float AddFlatAngle = AngleFactor * CompletionMaxDiameter;

        // 既存のコルーチンがあれば停止
        if (Coroutine_TargetAngle != null)
        {
            StopCoroutine(Coroutine_TargetAngle);
        }
        Coroutine_TargetAngle = StartCoroutine(SlerpTarget_Z(AddFlatAngle));
    }

    /// <summary>
    /// カメラ引きエフェクトの処理
    /// 引き→維持→戻しの3段階で補間
    /// </summary>
    IEnumerator SlerpTarget_Z(float TargetAngle)
    {
        // 現在の値から開始するための調整
        float count = Mathf.Clamp01(AddAngleforward / TargetAngle) * CompletionTimeBack;

        // カメラを引く
        while (count < CompletionTimeBack)
        {
            count += Time.fixedDeltaTime;

            float t = Mathf.Clamp01(count / CompletionTimeBack);

            float lerp = TargetAngle * t;

            AddAngleforward = lerp;

            yield return null;
        }

        count = CompletionTimeForward;

        // 最大値を維持
        yield return new WaitForSeconds(CompletionTimeMaxValue);

        // カメラを戻す
        while (count > 0)
        {
            count -= Time.fixedDeltaTime;

            float t = Mathf.Clamp01(count / CompletionTimeForward);

            float lerp = TargetAngle * t;

            AddAngleforward = lerp;

            yield return null;
        }
    }
}
