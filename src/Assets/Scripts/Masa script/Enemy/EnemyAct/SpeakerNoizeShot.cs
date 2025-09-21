using UnityEngine;

public class SpeakerNoizeShot : EnemyActBase
{
    [SerializeField] Transform target;

    [SerializeField] GameObject SpeakerPrehab;

    [SerializeField] float SpawnDistance = 3.0f;

    [SerializeField] float ActTimeCount = 10f;

    [SerializeField] float NoizeSpeed = 3f;

    [SerializeField] float DestroyTime = 10f;

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

            // XZ平面の基準方向
            Vector3 dirTarget = (target.position - transform.position);
            dirTarget.y = 0f;
            dirTarget.Normalize();

            Vector3 SpawnPos = transform.position + dirTarget * SpawnDistance;

            // オブジェクト生成
            GameObject prehab = Instantiate(SpeakerPrehab, SpawnPos, Quaternion.LookRotation(dirTarget.normalized, Vector3.up));

            Destroy(prehab, DestroyTime);
        }
    }
}
