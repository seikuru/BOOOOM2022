using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerSpwanAround : EnemyActBace
{
    [SerializeField] GameObject SpeakerPrehab;

    [SerializeField] float SpawnDistance = 10.0f;

    [SerializeField] int SpawnValue = 8;

    [SerializeField] float DestroyTime = 41f;

    Transform target => Player.GetTransformPlayer;

    public override void Act_Start()
    {

    }

    public override void Act_FixedUpdate()
    {
        // Playerが向いているベクトル（XZ平面に投影）
        Vector3 forward = target.forward;
        forward.y = 0f;
        forward.Normalize();


        for (float Angle = -0; Angle < 360; Angle += (360f / SpawnValue))
        {
            // Y軸まわりに回転
            Vector3 rotated = Quaternion.Euler(0f, Angle, 0f) * forward;

            Vector3 SpawnPos = target.position + rotated * SpawnDistance;

            // オブジェクト生成
            GameObject prehab = Instantiate(SpeakerPrehab, SpawnPos, Quaternion.Euler(0f, Angle + 180f, 0f));

            Destroy(prehab, DestroyTime);
        }
    }
}
