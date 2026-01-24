using UnityEngine;
using System.Collections;

public class SpeakerStraightNoizeShot : MonoBehaviour
{
    [SerializeField] GameObject NoizePrehab;

    [SerializeField] float SpawnDistance = 3.0f;

    [SerializeField] float ActTimeCount = 10f;

    [SerializeField] int InstantiateValue = 6;

    [SerializeField] float ShotWaitTime = 0.2f;

    [SerializeField] float DestroyTime = 10f;

    float ActCount;
    bool IsEndShooting;

    void Start()
    {
        ActCount = 0;
        IsEndShooting = true;
    }

    void FixedUpdate()
    {
        if (IsEndShooting)
            ActCount += Time.fixedDeltaTime;

        if (ActCount > ActTimeCount)
        {
            ActCount = 0;
            IsEndShooting = false;

            StartCoroutine(StraightShotRoutine());
        }
    }

    private IEnumerator StraightShotRoutine()
    {
        for (int i = 0; i < InstantiateValue; i++)
        {
            // 基準方向
            Vector3 dirTarget = transform.forward.normalized;

            Vector3 SpawnPos = transform.position + dirTarget * SpawnDistance;

            // オブジェクト生成
            GameObject prehab = Instantiate(NoizePrehab, SpawnPos, transform.rotation);

            Destroy(prehab, DestroyTime);

            // FixedUpdate が終わるまで待つ
            yield return new WaitForSeconds(ShotWaitTime);
        }

        IsEndShooting = true;
    }
}
