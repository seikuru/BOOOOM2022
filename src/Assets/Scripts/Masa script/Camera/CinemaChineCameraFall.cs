using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] float YMoveLimit = 5f;

    [SerializeField] float FallOffset_Y = 3f;
    [SerializeField] float DefaltOffset_Y = 2f;

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

        // Šp“x‚ð‹‚ß‚éi’PˆÊ‚Í“xj
        return Vector3.Angle(AB, AC);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if (orbital != null)
        {
            

            Debug.Log(distance_y);

            if (distance_y > CheckDistanceY)
                orbital.m_YDamping = FallYDumping;
            else
                orbital.m_YDamping = DefaltYDumping;
        }
        */
        if (transposer != null)
        {
            float distance = Vector3.Distance(Player.GetTransformPlayer.position, BeforePos);
            float distance_y = Mathf.Abs(BeforePosFixed.y - Player.GetTransformPlayer.position.y);
            float distanceXZ = Vector2.Distance(
                new(Player.GetTransformPlayer.position.x, Player.GetTransformPlayer.position.z),
                new(BeforePosFixed.x, BeforePosFixed.z));

            float bonus = transposer.m_FollowOffset.y;

            // ‹——£‚É‰ž‚¶‚½‰ÁŽZ•”•ªŒvŽZ
            if (YMoveLimit < distance_y || BeforePosFixed.y - Player.GetTransformPlayer.position.y > 0)
            {
                //Debug.Log(distance_y +" "+ distanceXZ);
                if (distance_y <= distanceXZ)
                {
                    bonus += Time.deltaTime * -0.5f;
                }
                else
                {
                    bonus += Time.fixedDeltaTime * 2f;       
                }
            }
            else
                bonus += Time.fixedDeltaTime * -2f;

            transposer.m_FollowOffset = new()
            {
                x = transposer.m_FollowOffset.x,
                y = Mathf.Clamp(bonus, DefaltOffset_Y, FallOffset_Y),
                z = transposer.m_FollowOffset.z,
            };
        }

        BeforePosFixed = Player.GetTransformPlayer.position;
    }
    private void Update()
    {
        /*
        if (transposer != null)
        {
            float distance = Vector3.Distance(Player.GetTransformPlayer.position, BeforePos);

            float bonus = transposer.m_FollowOffset.z;

            // ‹——£‚É‰ž‚¶‚½‰ÁŽZ•”•ªŒvŽZ
            if (MaxLimitAngle < GetAngle(Player.GetTransformPlayer.position, BeforePos, transform.position))
            {
                if (distance <= nearDistance)
                {
                    bonus += Time.deltaTime * -1f;
                }
                if (distance < farDistance)
                {

                    bonus += Time.deltaTime * -1f;
                    
                    // 3`5‚Ì”ÍˆÍ‚ÅüŒ`‚ÉŒ¸­i5‚Å0j
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

}
