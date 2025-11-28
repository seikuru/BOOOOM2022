using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;

public class CinemaChineCameraAngle : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Rigidbody PlayerRB;
    [SerializeField] CinemachineVirtualCamera VirtualCamera;
    [Space]
    [SerializeField, Header("カメラまでの基本距離")] float DefaltDistance = 5f;

    [SerializeField, Header("カメラの最大角度")] float MaxAngle = 80f;
    [SerializeField, Header("カメラの最小角度")] float MinAngle = 10f;
    [SerializeField, Header("アングル変更速度")] float AngleChengeSpeed = 800;

    [SerializeField, Header("落下速度計算上限")] float FallSpeedLimit = 50f;

    [SerializeField, Header("上昇量計算上限")] float UpPosLimit = 40f;
    [SerializeField, Header("上昇によるカメラのLeapの倍率")
        , Range(0f, 1f)]
    float UpCameraDiameter = 0.65f;

    [SerializeField, Header("平面移動量計算上限")] float FlatVerocityLimit = 60f;
    [SerializeField, Header("平面移動によるカメラのLeapの倍率")
        , Range(0f, 1f)]
    float FlatCameraDiameter = 0.35f;

    [Header("爆発によるカメラの引き")]

    [SerializeField, Header("補完する時の最大倍率")] float CompletionMaxDiameter = 1.1f;

    [SerializeField, Header("補完時間_引き")] float CompletionTimeBack = 0.1f;
    [SerializeField, Header("補完時間_最大値維持")] float CompletionTimeMaxValue = 0.15f;
    [SerializeField, Header("補完時間_縮み")] float CompletionTimeForward = 0.7f;

    CinemachineTransposer transposer;
    Coroutine Coroutine_TargetAngle;

    float TargetAngle;
    float AddAngleforward;

    bool BeforeGround;
    float ExitGroundPos = 0;
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

    float NormalizeClamp(float value, float min, float max)
    {
        return Mathf.Clamp01((value - min) / (max - min));
    }

    Vector2 DirFromAngle(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    void GroundCheck()
    {
        if (BeforeGround && !player.OnGround)
        {
            ExitGroundPos = transform.position.y;
        }
        if (player.OnGround)
        {
            UpClampValue = 0;
        }

        BeforeGround = player.OnGround;
    }

    void Start()
    {
        transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        TargetAngle = MinAngle;
        AddAngleforward = 0f;
    }

    void FixedUpdate()
    {
        if (transposer == null)
            return;

        GroundCheck();

        float clamp;
        float LerpAngle;

        float VerocityMagnitudeFlat = new Vector2(PlayerRB.velocity.x, PlayerRB.velocity.z).magnitude;

        float clampFlat = NormalizeClamp(VerocityMagnitudeFlat, 0, FlatVerocityLimit) * FlatCameraDiameter;

        if (!player.OnGround && PlayerRB.velocity.y >= 0)
        {
            float distance_y = Mathf.Max(Player.GetTransformPlayer.position.y - ExitGroundPos, 0);

            clamp = NormalizeClamp(distance_y, 0, UpPosLimit) * UpCameraDiameter;

            UpClampValue = Mathf.Max(clamp, UpClampValue);

            LerpAngle = MinAngle + (MaxAngle - MinAngle) * Mathf.Max(clamp - clampFlat, 0);
        }
        else
        {
            clamp = NormalizeClamp(PlayerRB.velocity.y, -FallSpeedLimit, 0);

            clamp = Mathf.Max(1 - clamp, UpClampValue);

            clamp = Mathf.Max(clamp - clampFlat, 0);

            LerpAngle = MinAngle + (MaxAngle - MinAngle) * Mathf.Max(clamp - clampFlat, 0);
        }

        

        float FixAngle = TargetAngle;     

        if (TargetAngle < LerpAngle)
        {
            FixAngle = Mathf.Min(TargetAngle + AngleChengeSpeed * Time.fixedDeltaTime, LerpAngle);
        }
        else if (TargetAngle > LerpAngle)
        {
            FixAngle = Mathf.Max(TargetAngle - AngleChengeSpeed * Time.fixedDeltaTime, LerpAngle);
        }

        TargetAngle = FixAngle;
    }

    private void Update()
    {
        Vector2 TargetOffSet2D = DirFromAngle(TargetAngle) * DefaltDistance;

        Vector3 TargetOffSet = new(0,TargetOffSet2D.y, -TargetOffSet2D.x);

        transposer.m_FollowOffset = TargetOffSet + TargetOffSet * (1 + (AddAngleforward - 1));
    }

    private void LateUpdate()
    {
        if (!player.PlayerBombHit)
        {
            return;
        }

        // フラグ解除
        player.PlayerBombHit = false;

        float AngleFactor = GetRightAngleFactor(PlayerRB.velocity, Player.GetTransformPlayer.position - transform.position);

        float AddFlatAngle = AngleFactor * CompletionMaxDiameter;

        if (Coroutine_TargetAngle != null)
        {
            StopCoroutine(Coroutine_TargetAngle);
        }
        Coroutine_TargetAngle = StartCoroutine(SlerpTarget_Z(AddFlatAngle));
    }

    IEnumerator SlerpTarget_Z(float TargetAngle)
    {
        //Vector3 Target = TargetOffSet;
        //float count01 = 


        float count = Mathf.Clamp01(AddAngleforward / TargetAngle) * CompletionTimeBack;

        while (count < CompletionTimeBack)
        {
            count += Time.fixedDeltaTime;

            float t = Mathf.Clamp01(count / CompletionTimeBack);

            float lerp = TargetAngle * t;

            AddAngleforward = lerp;

            yield return null;
        }

        count = CompletionTimeForward;

        yield return new WaitForSeconds(CompletionTimeMaxValue);

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
