using Cinemachine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;


public class Bombeffects : MonoBehaviour
{  
    [SerializeField] LayerMask InfluencedMask;//爆発の影響を受けるレイヤーを指定
    [SerializeField] float DestroyEnemyTimer = 3f;//敵が爆発の影響を受けてから何秒で消えるか
    [SerializeField] float BombStrange = 5.0f;//爆弾が与える力の大きさ
    [SerializeField] float BombRadius = 10.0f;//爆発の影響の範囲
    [SerializeField] float OffScreenAddRadius;//爆発の画面外補正
    [SerializeField] Collider bombCollider;
    [SerializeField] VisualEffect VEffect;//爆発した際のエフェクト
    [SerializeField] GameObject BombOuter;//爆弾の外枠のオブジェクト
    [SerializeField] Rigidbody BombRB;//爆弾のRigidBody
    [SerializeField] Transform BombSenterPos;//爆発の中心位置
    [SerializeField] bool GetKillCount = false;
    [SerializeField] Renderer BombRenderer;
    AudioSource PlayerAudioSource;
    [SerializeField] AudioSource BombAudioSource;
    [SerializeField] AudioScriptable AudioScriptable;
    [SerializeField] CinemachineImpulseSource impulseSource;
    // Animator PlayerAnimation;
    EnemyCount EnemyCountText;
    bool GetPlayerAnimationFlag = false;
    bool isHitPlayer = false;

    public Rigidbody GetRB => BombRB;

    // public float _bombradius { get { return BombRadius; } set { BombRadius = value; } }

    /// <summary>
    /// 爆弾が与える力の大きさを追加するパラメータの数値を取得
    /// </summary>
    /// <returns>追加するパラメータの数値</returns>
    float GetBombAddStrange()
    {
        return BombExtraParameter.GetAddStrange();      
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (InfluencedMask == LayerMask.GetMask())
        {
            InfluencedMask = LayerMask.GetMask("Player", "enemy", "enemyCore");
        }

        if (GameObject.Find("EnemyCount") != null)
        {
            EnemyCountText = GameObject.Find("EnemyCount").GetComponent<EnemyCount>();
        }

        if (GameObject.FindWithTag("Player").TryGetComponent<AudioSource>(out AudioSource AS))
        {
            PlayerAudioSource = AS;
        }

        if (BombSenterPos == null)
            BombSenterPos = this.transform;
    }


    public async void Bakuhatu()
    {
        float BombStrangeValue = BombStrange;

        if (GetKillCount)
            BombStrangeValue += GetBombAddStrange();

        BombAudioSource.PlayOneShot(AudioScriptable._ExplodeSounds);

        Collider[] hits;
        Debug.Log(BombRenderer.isVisible);
        if (BombRenderer.isVisible)
        {
            hits = Physics.OverlapSphere(BombSenterPos.position, BombRadius, InfluencedMask);
            //爆弾が爆発した際、爆弾を中心に、爆弾の影響範囲下にある、影響を受けるレイヤーを探す。
        }
        else
        {
            hits = Physics.OverlapSphere(BombSenterPos.position, BombRadius + OffScreenAddRadius, InfluencedMask);
            //画面外にいる際に爆発を強化
        }


        GameObject[] P = { };

        foreach (Collider hit in hits)
        {

            P = hits.Select(hit => hit.gameObject).ToArray();//ColliderをgameObjectの形で再格納
          
        }

        Rigidbody[] PlayerRigidbodies = new Rigidbody[P.Length];//格納した数だけRigidbodyを宣言

        for (int i = 0; i < P.Length; i++)
        {
            PlayerRigidbodies[i] = P[i].GetComponent<Rigidbody>();
        }


        for (int i = 0; i < P.Length; i++)
        {
            //Debug.Log("Obstacle" + P[i].tag);
            if (P[i].tag == "Obstacle")
            {
                ComboCounter.AddCombo();

                if (P[i].TryGetComponent<ObstacleExplosion>(out ObstacleExplosion obstacle))
                {
                    obstacle.Explosion(BombSenterPos.position, BombStrangeValue);
                    BombAudioSource.PlayOneShot(AudioScriptable._DestroyObstacleSounds); 
                    //PlayerAudioSource.PlayOneShot(AudioScriptable._DestroyObstacleSounds);
                    continue;
                }
            }
            else if (P[i].tag == "Noize")
            {
                Destroy(P[i]);
               
                BombAudioSource.PlayOneShot(AudioScriptable._DestroyObstacleSounds);
                //PlayerAudioSource.PlayOneShot(AudioScriptable._DestroyObstacleSounds);
                continue;
            }
            else if (P[i].tag == "enemy")
            {

                ComboCounter.AddCombo();

                PlayerRigidbodies[i].isKinematic = false;
                if (P[i].TryGetComponent<EnemiesAttack>(out EnemiesAttack EA))
                {
                    EA.willDestoroy = true;

                }
                if (P[i].TryGetComponent<enemyMove>(out enemyMove EM))
                {
                    EM.willDestroy = true;

                }
                if (P[i].TryGetComponent<EnemyAttackPattern>(out EnemyAttackPattern EAP))
                {
                    //EAP.willDestroy = true;
                }
                if(P[i].TryGetComponent<BossTeleport>(out BossTeleport BT))
                {
                    StartCoroutine(BT.BossStateChange());
                }
                if (P[i].TryGetComponent<Animator>(out Animator enemyAnimator))
                {
                    enemyAnimator.SetTrigger("OnDamage");
                }


                //Destroy(P[i], DestroyEnemyTimer);//DestoryEnemyTimer秒後に消滅
            }
            else if (P[i].tag == "Attack2")//敵の弾を爆弾で防ぐ際はこれを使用
            {
                PlayerRigidbodies[i].velocity = PlayerRigidbodies[i].velocity * 0.1f;
            }
            else if (P[i].tag == "Player")
            {
                if (P[i].TryGetComponent<Player>(out Player p))
                {
                    p.PlayerBombHit = true;
                }
                if (P[i].TryGetComponent<PlayerRotateAction>(out PlayerRotateAction act))
                {
                    //Debug.Log("try get component PlayerRotateAction");
                    act.RotateToExplosion(P[i].transform.position - BombSenterPos.position);
                }

                if (P[i].TryGetComponent<Animator>(out Animator animator))
                {
                    animator.SetTrigger("BombHit");
                }
                if (P[i].TryGetComponent<PlayerFallSpeedAdder>(out PlayerFallSpeedAdder PFSA))
                {
                    PFSA._BombHit = true;
                }
                BombAudioSource.PlayOneShot(AudioScriptable._BombHitSounds);

                // Debug.Log("set bombs hit true");
                isHitPlayer = true;
            }

            PlayerRigidbodies[i].velocity = PlayerRigidbodies[i].velocity * 0.7f + (P[i].transform.position - BombSenterPos.position).normalized * BombStrangeValue;
            //最後に受けた爆発の影響が出やすくなるように今のVectorに0,7を掛ける

        }

        if(bombCollider != null)
            bombCollider.enabled = false;
        if (BombRB != null)
            BombRB.isKinematic = true;
        if (VEffect != null)
            VEffect.SendEvent("OnPlay");
        if (BombOuter != null)
            BombOuter.SetActive(false);
        if (impulseSource != null)
            impulseSource.GenerateImpulse();

        Destroy(gameObject, 3f);
    }

    public bool getHit()
    {
        // Debug.Log("bombs hit");
        return isHitPlayer;
    }

    public void setHit(bool hit)
    {
        isHitPlayer = hit;
    }
}
