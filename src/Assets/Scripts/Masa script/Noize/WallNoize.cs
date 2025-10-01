using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WallNoize : MonoBehaviour
{
    [SerializeField] GameObject WallPrehab;

    [SerializeField] float SpawnDistance = 3.0f;

    [SerializeField] int InstantiateSegment = 10;

    [SerializeField] float ShotWaitTime = 0.04f;

    [SerializeField] float RayDistance = 150f;

    [SerializeField] LayerMask groundLayer;

    static readonly float ExceptionPointY = -9999f;

    float GetGroundPosition_Y(Vector3 origin)
    {
        RaycastHit hit;

        if (Physics.Raycast(origin, Vector3.down, out hit, RayDistance, groundLayer))
        {
            return hit.point.y;
        }

        return ExceptionPointY;
    }

    private void Start()
    {
        StartCoroutine(WallSpwanRoutine());
    }

    private IEnumerator WallSpwanRoutine()
    {
        for (int i = 0; i <= InstantiateSegment; i++)
        {
            // 自分の向いている正面から見て右方向へ SpawnDistance 離れた場所
            Vector3 rightOffsetPos = transform.position + (transform.right * SpawnDistance * i);

            float GroundPosY = GetGroundPosition_Y(rightOffsetPos);

            if (GroundPosY != ExceptionPointY)
            {
                rightOffsetPos.y = GroundPosY + WallPrehab.transform.localScale.y/2;
                // オブジェクト生成
                PrehabSpawn(rightOffsetPos);
            }


            if (i != 0)
            {
                // 自分の向いている正面から見て左方向へ SpawnDistance 離れた場所
                Vector3 leftOffsetPos = transform.position + (-transform.right * SpawnDistance * i);

                GroundPosY = GetGroundPosition_Y(leftOffsetPos);

                if (GroundPosY != ExceptionPointY)
                {
                    leftOffsetPos.y = GroundPosY + WallPrehab.transform.localScale.y / 2;
                    // オブジェクト生成
                    PrehabSpawn(leftOffsetPos);
                }
            }

            // FixedUpdate が終わるまで待つ
            yield return new WaitForSeconds(ShotWaitTime);
        }
    }

    void PrehabSpawn(Vector3 pos)
    {
        GameObject prehab = Instantiate(WallPrehab, pos, transform.rotation);

        prehab.transform.parent = transform;

    }
}
