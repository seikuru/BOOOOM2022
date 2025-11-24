using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class CinemaChineCameraFall : MonoBehaviour
{
    /*
    [SerializeField] float CheckDistanceY = 0.1f;

    [SerializeField] float FallYDumping = 1f;
    [SerializeField] float DefaltYDumping = 0.2f;

    [SerializeField] float nearDistance = 3f, farDistance = 5f;

    [SerializeField] float maxBonusOffset_Z = -12f;
    [SerializeField] float DefaltFollowOffset_Z = -8f;
    */
    [SerializeField] Player player;
    [SerializeField] Rigidbody PlayerRB;
    [SerializeField] CinemachineVirtualCamera VirtualCamera;
    [Space]
    [SerializeField, Header("補完する時の最大倍率")] float CompletionMaxDiameter = 1.1f;

    [SerializeField, Header("補完時間_引き")] float CompletionTimeBack = 0.1f;
    [SerializeField, Header("補完時間_縮み")] float CompletionTimeForward = 0.7f;
    
    [SerializeField, Header("標準カメラ距離")] Vector3 DefaltOffSet = new Vector3(0, 2, -8);
    [SerializeField, Header("落下カメラ距離")] Vector3 FallOffSet = new Vector3(0, 10, -4);

    [SerializeField, Header("落下速度計算上限")] float FallSpeedLimit = 50f;
    [SerializeField, Header("上昇量計算上限")] float UpPosLimit = 40f;

    [SerializeField, Header("上昇によるカメラのLeapの倍率")
        ,Range(0f,1f)] float UpCameraDiameter = 0.65f; 

    [SerializeField, Header("オフセット変更速度")] Vector3 OffsetSpeed = Vector3.one;

    [SerializeField] float YMoveLimit = 5f;

    [SerializeField] float FallOffset_Y = 3f;
    [SerializeField] float DefaltOffset_Y = 2f;

    [SerializeField] float DefaltAngleSpeed = 2f;
    [SerializeField] float FlatAngleSpeed = 0.5f;
    [SerializeField] float FallAngleSpeed = 2f;

    

    CinemachineOrbitalTransposer orbital;
    CinemachineComposer composer;
    CinemachineTransposer transposer;

    Vector3 TargetOffSet;

    Coroutine Coroutine_Z;
    float AddAngleforward;

    Vector3 BeforePos;
    Vector3 BeforePosFixed;

    bool BeforeGround;
    float ExitGroundPos = 0;

    float UpClampValue = 0;

    Vector3 OffsetSpeedVector => OffsetSpeed * Time.fixedDeltaTime;

    void Start()
    {
        orbital = VirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        BeforePos = Player.GetTransformPlayer.position;
        BeforePosFixed = Player.GetTransformPlayer.position;

        TargetOffSet = DefaltOffSet;
        AddAngleforward = 0f;
    }

    float GetAngle(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 AB = b - a;
        Vector3 AC = c - a;

        // 角度を求める（単位は度）
        return Vector3.Angle(AB, AC);
    }

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

    void FixedUpdate()
    {
        if (transposer == null)
            return;

        GroundCheck();

        float clamp;
        Vector3 LerpOffset;

        if (!player.OnGround && PlayerRB.velocity.y >= 0)
        {
            UpClampValue = 0;

            float distance_y = Mathf.Max(Player.GetTransformPlayer.position.y - ExitGroundPos, 0);
            
            clamp = NormalizeClamp(distance_y, 0, UpPosLimit) * UpCameraDiameter;
            
            LerpOffset = Vector3.Lerp(DefaltOffSet, FallOffSet, clamp);

            UpClampValue = clamp;
        }
        else
        {
            clamp = NormalizeClamp(PlayerRB.velocity.y, -FallSpeedLimit, 0);

            clamp = Mathf.Max(1 - clamp,UpClampValue);

            LerpOffset = Vector3.Lerp(DefaltOffSet, FallOffSet, clamp);
        }

        //float distance = Vector3.Distance(Player.GetTransformPlayer.position, BeforePos);
        //float distance_y = Mathf.Abs(BeforePosFixed.y - Player.GetTransformPlayer.position.y);
        //float distanceXZ = Vector2.Distance(
        //    new(Player.GetTransformPlayer.position.x, Player.GetTransformPlayer.position.z),
        //    new(BeforePosFixed.x, BeforePosFixed.z));
        //float bonus = transposer.m_FollowOffset.y;

        Vector3 FixOffset = TargetOffSet;
        /*
        if(TargetOffSet.x < LerpOffset.x)
        {
            FixedOffset.x = Mathf.Min(TargetOffSet.x + FixedOffset.x, LerpOffset.x);
        }
        else if(TargetOffSet.x > LerpOffset.x)
        {
            FixedOffset.x = Mathf.Max(TargetOffSet.x - FixedOffset.x, LerpOffset.x);
        }
        */

        if (TargetOffSet.y < LerpOffset.y)
        {
            FixOffset.y = Mathf.Min(TargetOffSet.y + OffsetSpeedVector.y, LerpOffset.y);
        }
        else if (TargetOffSet.y > LerpOffset.y)
        {
            FixOffset.y = Mathf.Max(TargetOffSet.y - OffsetSpeedVector.y, LerpOffset.y);
        }

        if (TargetOffSet.z < LerpOffset.z)
        {
            FixOffset.z = Mathf.Min(TargetOffSet.z +OffsetSpeedVector.z, LerpOffset.z);
        }
        else if (TargetOffSet.z > LerpOffset.z)
        {
            FixOffset.z = Mathf.Max(TargetOffSet.z - OffsetSpeedVector.z, LerpOffset.z);
        }
        TargetOffSet = FixOffset;
        /*

        // 距離に応じた加算部分計算
        if (YMoveLimit < distance_y || BeforePosFixed.y - Player.GetTransformPlayer.position.y > 0)
        {
            //Debug.Log(distance_y +" "+ distanceXZ);
            if (distance_y <= distanceXZ)
            {
                bonus += Time.fixedDeltaTime * -FlatAngleSpeed;
            }
            else
            {
                bonus += Time.fixedDeltaTime * FallAngleSpeed;
            }
        }
        else
            bonus += Time.fixedDeltaTime * -DefaltAngleSpeed;

        transposer.m_FollowOffset = new()
        {
            x = transposer.m_FollowOffset.x,
            y = Mathf.Clamp(bonus, DefaltOffset_Y, FallOffset_Y),
            z = transposer.m_FollowOffset.z,
        };

        BeforePosFixed = Player.GetTransformPlayer.position;*/
    }

    void GroundCheck()
    {
        if (BeforeGround && !player.OnGround)
        {
            ExitGroundPos = transform.position.y;
        }
        if(player.OnGround)
        {
            UpClampValue = 0;
        }

        BeforeGround = player.OnGround;
    }













    private void Update()
    {
        //transposer.m_FollowOffset = TargetOffSet - Vector3.forward * (AddAngleforward);
        transposer.m_FollowOffset = TargetOffSet + TargetOffSet * (1 + (AddAngleforward -1));
        

        /*
        if (transposer != null)
        {
            float distance = Vector3.Distance(Player.GetTransformPlayer.position, BeforePos);

            float bonus = transposer.m_FollowOffset.z;

            // 距離に応じた加算部分計算
            if (MaxLimitAngle < GetAngle(Player.GetTransformPlayer.position, BeforePos, transform.position))
            {
                if (distance <= nearDistance)
                {
                    bonus += Time.deltaTime * -1f;
                }
                if (distance < farDistance)
                {

                    bonus += Time.deltaTime * -1f;
                    
                    // 3～5の範囲で線形に減少（5で0）
                    float t = Mathf.InverseLerp(farDistance, nearDistance, distance);
                    bonus = Mathf.Lerp(DefaltFollowOffset_Z, maxBonusOffset_Z - DefaltFollowOffset_Z, t);
                    
                }
            }
            else
                bonus += Time.deltaTime * 1f;

            transposer.m_FollowOffset = new()
            {
                x = transposer.m_FollowOffset.x,
                y = transposer.m_FollowOffset.y,
                z = Mathf.Clamp(bonus, maxBonusOffset_Z, DefaltFollowOffset_Z)
            };
        }

        BeforePos = Player.GetTransformPlayer.position;
        */
    }

    private void LateUpdate()
    {
        if(!player.PlayerBombHit)
        {
            return;
        }

        // フラグ解除
        player.PlayerBombHit = false;

        float AngleFactor = GetRightAngleFactor(PlayerRB.velocity, Player.GetTransformPlayer.position - transform.position);

        float AddFlatAngle = AngleFactor * (CompletionMaxDiameter);

        if (Coroutine_Z != null)
        {
            StopCoroutine(Coroutine_Z);
        }
        Coroutine_Z = StartCoroutine(SlerpTarget_Z(AddFlatAngle));
    }

    IEnumerator SlerpTarget_Z(float AddAngle)
    {
        //Vector3 Target = TargetOffSet;

        float count = NormalizeClamp(AddAngleforward, 0f, CompletionMaxDiameter);

        while (count < CompletionTimeBack)
        {
            count += Time.deltaTime;
            float t = Mathf.Clamp01(count / CompletionTimeBack);

            float lerp = AddAngle * t;

            AddAngleforward = lerp;
            
            yield return null;
        }

        count = CompletionTimeForward;

        while (count > 0)
        {
            count -= Time.deltaTime;
            float t = Mathf.Clamp01(count / CompletionTimeForward);

            float lerp = AddAngle * t;

            AddAngleforward = lerp;
            
            yield return null;
        }
    }
}
