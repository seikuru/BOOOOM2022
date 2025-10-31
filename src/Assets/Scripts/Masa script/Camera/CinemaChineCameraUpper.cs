using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaChineCameraUpper : MonoBehaviour
{
    [SerializeField] float CheckDistanceValue = 3f;

    [SerializeField] float UpperAimComposerScreenY = 0.85f;
    [SerializeField] float DefaltAimComposerScreenY = 0.5f;

    [SerializeField] float MaxLimitAngle = 30f;

    [SerializeField] float nearDistance = 3f, farDistance = 5f;
    
    [SerializeField] float maxBonusOffset_Z = -12f;
    [SerializeField] float DefaltFollowOffset_Z = -8f;

    [SerializeField] CinemachineVirtualCamera VirtualCamera;

    CinemachineComposer composer;
    CinemachineTransposer transposer;

    Vector3 BeforePos;

    void Start()
    {
        composer = VirtualCamera.GetCinemachineComponent<CinemachineComposer>();
        transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        BeforePos = Player.GetTransformPlayer.position;
    }

    float GetAngle(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 AB = b - a;
        Vector3 AC = c - a;

        // 角度を求める（単位は度）
        return Vector3.Angle(AB, AC);
    }

    // Update is called once per frame
    void Update()
    {
        if (composer != null)
        {
            float distance = Vector3.Distance(Player.GetTransformPlayer.position, VirtualCamera.transform.position);
            //Debug.Log(distance);
            if (distance > CheckDistanceValue)
            {
                //composer.m_ScreenY = DefaltAimComposerScreenY;
            }

            //else
                //composer.m_ScreenY = UpperAimComposerScreenY;
        }
        if(transposer != null)
        {
            float distance = Vector3.Distance(Player.GetTransformPlayer.position, BeforePos);

            // 距離に応じたボーナス計算
            float bonus = DefaltFollowOffset_Z;
            if (MaxLimitAngle > GetAngle(Player.GetTransformPlayer.position,BeforePos, transform.position))
            {
                if (distance <= nearDistance)
                {
                    bonus = maxBonusOffset_Z; // 3以下なら最大
                }
                else if (distance < farDistance)
                {
                    // 3～5の範囲で線形に減少（5で0）
                    float t = Mathf.InverseLerp(farDistance, nearDistance, distance);
                    bonus = Mathf.Lerp(DefaltFollowOffset_Z, maxBonusOffset_Z - DefaltFollowOffset_Z, t);
                }
            }
          
            transposer.m_FollowOffset = new()
            {
                x = transposer.m_FollowOffset.x,
                y = transposer.m_FollowOffset.y,
                z = bonus
            };
        }

        BeforePos = Player.GetTransformPlayer.position;
    }
}
