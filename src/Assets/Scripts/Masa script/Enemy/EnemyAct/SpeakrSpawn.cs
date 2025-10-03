using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakrSpawn : EnemyActBase
{
    [SerializeField] GameObject SpeakerPrehab;

    [SerializeField] float SpawnDistance = 3.0f;

    [SerializeField] float ActTimeCount = 10f;

    [SerializeField] float RandomAngleMax = 60f;

    [SerializeField] float DestroyTime = 41f;

    Transform target => Player.GetTransformPlayer;

    float ActCount;

    public override void Act_Start()
    {
        ActCount = 0;
    }

    public override void Act_FixedUpdate()
    {
        ActCount += Time.fixedDeltaTime;

        if (ActCount > ActTimeCount)
        {
            ActCount = 0;

            // Playerが向いているベクトル（XZ平面に投影）
            Vector3 forward = target.forward;
            forward.y = 0f;
            forward.Normalize();

            // -90 ~ 90 度のランダム角度を決定
            float randomAngle = Random.Range(-RandomAngleMax, RandomAngleMax);

            // Y軸まわりに回転
            Vector3 rotated = Quaternion.Euler(0f, randomAngle, 0f) * forward;

            Vector3 SpawnPos = target.position + rotated * SpawnDistance;

            // オブジェクト生成
            GameObject prehab = Instantiate(SpeakerPrehab, SpawnPos, Quaternion.Euler(0f, randomAngle + 180f, 0f));

            Destroy(prehab, DestroyTime);

        }
    }
}
