using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarNoizeShot : EnemyActBase
{
    [SerializeField] GameObject PillarPrehabSeed;

    [SerializeField] float ShotAngle = -45f;

    [SerializeField] float StartShotPower = 10f;

    [SerializeField] float StopShotPower = 40f;

    [SerializeField] float AddShotPower = 2f;

    [SerializeField] float ActTimeCount = 0f;

    [SerializeField] float DestroyTime = 6f;

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

            Vector3 SpwanPos = transform.position + Vector3.up;

            // XZ平面の基準方向
            Vector3 dirTarget = (target.position - SpwanPos);
            dirTarget.y = 0f;
            dirTarget.Normalize();

            // 上方向に ShotAngle 度傾けたベクトルを作成
            // dirTarget を水平ベクトルとし、そこに上方向を混ぜる
            Quaternion tilt = Quaternion.AngleAxis(ShotAngle, Vector3.Cross(Vector3.up, dirTarget));
            Vector3 shotDir = tilt * dirTarget;

            for (float i = StartShotPower; i <= StopShotPower; i+= AddShotPower)
            {
                // オブジェクト生成
                GameObject prehab = Instantiate(PillarPrehabSeed, SpwanPos, Quaternion.identity);

                // Rigidbody取得 (3D用)
                Rigidbody RB = prehab.GetComponent<Rigidbody>();

                // 発射方向を代入
                RB.velocity = shotDir.normalized * i;

                // 弾を発射方向に回転させる
                if (shotDir != Vector3.zero)
                {
                    prehab.transform.rotation = Quaternion.LookRotation(shotDir, Vector3.up);
                }

                Destroy(prehab, DestroyTime);
            }   
        }
    }
}
