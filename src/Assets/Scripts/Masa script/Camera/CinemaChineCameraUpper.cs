using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaChineCameraUpper : MonoBehaviour
{
    [SerializeField] float CheckDistanceY = 0.1f;

    [SerializeField] float FallYDumping = 1f;
    [SerializeField] float DefaltYDumping = 0.2f;

    [SerializeField] float MaxLimitAngle = 30f;

    [SerializeField] float nearDistance = 3f, farDistance = 5f;
    
    [SerializeField] float maxBonusOffset_Z = -12f;
    [SerializeField] float DefaltFollowOffset_Z = -8f;

    [SerializeField] CinemachineVirtualCamera VirtualCamera;

     CinemachineOrbitalTransposer orbital;
    CinemachineComposer composer;
    CinemachineTransposer transposer;

    Vector3 BeforePos;
    Vector3 BeforePosFixed;

    void Start()
    {
        orbital = VirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        BeforePos = Player.GetTransformPlayer.position;
        BeforePosFixed = Player.GetTransformPlayer.position;
    }

    float GetAngle(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 AB = b - a;
        Vector3 AC = c - a;

        // 角度を求める（単位は度）
        return Vector3.Angle(AB, AC);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (orbital != null)
        {
            float distance_y = BeforePosFixed.y - Player.GetTransformPlayer.position.y;

            Debug.Log(distance_y);

            if (distance_y > CheckDistanceY)
                orbital.m_YDamping = FallYDumping;
            else
                orbital.m_YDamping = DefaltYDumping;
        }

        BeforePosFixed = Player.GetTransformPlayer.position;
    }
    private void Update()
    {      
        if(transposer != null)
        {
            float distance = Vector3.Distance(Player.GetTransformPlayer.position, BeforePos);
            
            float bonus = transposer.m_FollowOffset.z;

            // 距離に応じた加算部分計算
            if (MaxLimitAngle < GetAngle(Player.GetTransformPlayer.position,BeforePos, transform.position))
            {
                if (distance <= nearDistance)
                {
                    bonus += Time.deltaTime * -1f;
                }
                if (distance < farDistance)
                {

                    bonus += Time.deltaTime * -1f;
                    /*
                    // 3～5の範囲で線形に減少（5で0）
                    float t = Mathf.InverseLerp(farDistance, nearDistance, distance);
                    bonus = Mathf.Lerp(DefaltFollowOffset_Z, maxBonusOffset_Z - DefaltFollowOffset_Z, t);
                    */
                }
            }
            else
                bonus += Time.deltaTime * 1f;

            transposer.m_FollowOffset = new()
            {
                x = transposer.m_FollowOffset.x,
                y = transposer.m_FollowOffset.y,
                z = Mathf.Clamp(bonus , maxBonusOffset_Z ,DefaltFollowOffset_Z)
            };
        }

        BeforePos = Player.GetTransformPlayer.position;
    }
}
