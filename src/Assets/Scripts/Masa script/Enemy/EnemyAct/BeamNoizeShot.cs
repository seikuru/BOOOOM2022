using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamNoizeShot : EnemyActBase
{
    [SerializeField] GameObject SpeakerPrehab;

    [SerializeField] float SpawnDistance = 3.0f;

    [SerializeField] int InstantiateValue = 6;

    [SerializeField] float ShotWaitTime = 0.2f;

    [SerializeField] float ActTimeCount = 10f;

    [SerializeField] float NoizeSpeed = 3f;

    [SerializeField] float DestroyTime = 10f;

    Transform target => Player.GetTransformPlayer;

    float ActCount;

    bool IsEndShooting;

    public override void Act_Start()
    {
        ActCount = 0;
        IsEndShooting = true;
    }

    public override void Act_FixedUpdate()
    {
        if(IsEndShooting)
            ActCount += Time.fixedDeltaTime;

        if (ActCount > ActTimeCount)
        {
            ActCount = 0;
            IsEndShooting = false;

            StartCoroutine(BeamShotRoutine());     
        }
    }

    private IEnumerator BeamShotRoutine()
    {
        for(int i = 0; i < InstantiateValue;i++)
        {
            // XZ平面の基準方向
            Vector3 dirTarget = (target.position - transform.position).normalized;

            Vector3 SpawnPos = transform.position + dirTarget * SpawnDistance;

            // オブジェクト生成
            GameObject prehab = Instantiate(SpeakerPrehab, SpawnPos, Quaternion.LookRotation(dirTarget, Vector3.up));

            Destroy(prehab, DestroyTime);

            // FixedUpdate が終わるまで待つ
            yield return new WaitForSeconds(ShotWaitTime);
        }

        IsEndShooting = true;
    }
}
