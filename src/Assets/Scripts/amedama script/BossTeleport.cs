using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class TeleportClass
{
    public GameObject TeleportPoint;
    public int BombHitRequire = 1;
    public int TeleportDelay = 3;
}

public class BossTeleport : MonoBehaviour
{

    [SerializeField] TeleportClass[] Teleport;
    int BombHitCount = 0;
    [HideInInspector]public int BossState = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Teleport[BossState].TeleportPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator BossStateChange()
    {

        BombHitCount++;

        if (BossState >= Teleport.Length - 1 || BombHitCount >= Teleport[BossState].BombHitRequire)
        {
            if (BossState < Teleport.Length - 1)
            {
                

                if (this.TryGetComponent<EnemyAttackPattern>(out EnemyAttackPattern EAP))
                {
                    EAP.willDestroy = true;
                }

                yield return new WaitForSeconds(1);

                if (BombHitCount >= Teleport[BossState].BombHitRequire)
                {
                    BombHitCount = 0;

                if (EAP != null)
                {
                    EAP.willDestroy = false;
                }
                if (this.TryGetComponent<Rigidbody>(out Rigidbody RB))
                {
                    RB.velocity = Vector3.zero;
                }

                
                    BossState++;
                    this.transform.position = Teleport[BossState].TeleportPoint.transform.position;
                }
                Debug.Log(BossState);
            }
            else
            {
                if (this.TryGetComponent<EnemyAttackPattern>(out EnemyAttackPattern EAP))
                {
                    EAP.willDestroy = true;
                }
            }

        }

        yield return null;
    }
}
