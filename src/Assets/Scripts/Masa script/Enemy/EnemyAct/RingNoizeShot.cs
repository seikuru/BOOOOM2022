using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class RingNoizeShot : EnemyActBace
{
    [SerializeField] Transform target;

    [SerializeField] GameObject WavePrehab;

    [SerializeField] float ActTimeCount = 10f;

    [SerializeField] float NoizeSpeed = 3f;

    [SerializeField] float DestroyTime = 10f;

    static readonly float AllForwardAngle = 360f;

    static readonly float AngleSplit = 10f;
    float ActCount;

    public override void Act_Start()
    {
        ActCount = 0;
    }


    public override void Act_FixedUpdate()
    {
        ActCount += Time.fixedDeltaTime;

        if(ActCount > ActTimeCount)
        {
            ActCount = 0;

            // 10度ずつずらす
            for (int i = 0; i < AllForwardAngle; i++)
            {
                // 基準となる方向 (XZ平面でターゲットまでのベクトル)
                Vector3 dirTarget = (target.position - transform.position);
                dirTarget.y = 0f; // 水平方向だけ考慮

                // オブジェクト生成
                GameObject gameObject = Instantiate(WavePrehab, transform.position, Quaternion.identity);

                // Rigidbody取得 (3D用)
                Rigidbody RB = gameObject.GetComponent<Rigidbody>();

                // 発射方向の回転角度 (Y軸回転)
                float angleRadians = (AngleSplit * i) * Mathf.Deg2Rad;

                // 回転行列を使ってXZ平面上で回転させる
                Quaternion rotation = Quaternion.Euler(0f, angleRadians * Mathf.Rad2Deg, 0f);
                Vector3 rotatedDir = rotation * dirTarget.normalized;

                // 発射方向を代入
                RB.velocity = rotatedDir * NoizeSpeed;

                // 弾を発射方向に回転させる
                if (rotatedDir != Vector3.zero)
                {
                    gameObject.transform.rotation = Quaternion.LookRotation(rotatedDir, Vector3.up);
                }

                Destroy(gameObject, DestroyTime);
            }

        }
    }
}
