using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class Bombeffects : MonoBehaviour
{
    Rigidbody[] PlayerRigidbodies;
    [SerializeField] LayerMask InfluencedMask;//爆発の影響を受けるレイヤーを指定
    [SerializeField] float DestroyEnemyTimer = 3f;//敵が爆発の影響を受けてから何秒で消えるか
    [SerializeField] float BombStrange = 5.0f;//爆弾が与える力の大きさ
    [SerializeField] float BombRadius = 10.0f;//爆発の影響の範囲
    [SerializeField] GameObject particle;//爆発した際のパーティクル
    [SerializeField] bool GetKillCount = false;

    EnemyCount EnemyCountText;

    public float _bombradius { get { return BombRadius; } set { BombRadius = value; } }

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
        if(InfluencedMask == LayerMask.GetMask()) 
            InfluencedMask = LayerMask.GetMask("Player", "enemy" , "enemyCore");
        if (GameObject.Find("EnemyCount") != null)
        {
            EnemyCountText = GameObject.Find("EnemyCount").GetComponent<EnemyCount>();
        }
    }

   
    // Update is called once per frame
    void Update()
    {
    
    }

    public void Bakuhatu()
    {
        float BombStrangeValue = BombStrange;

        if (GetKillCount)
            BombStrangeValue += GetBombAddStrange();

        Collider[] hits = Physics.OverlapSphere(this.transform.position, BombRadius, InfluencedMask);
        //爆弾が爆発した際、爆弾を中心に、爆弾の影響範囲下にある、影響を受けるレイヤーを探す。

        GameObject[] P = { };

        foreach (Collider hit in hits)
        {

            P = hits.Select(hit => hit.gameObject).ToArray();//ColliderをgameObjectの形で再格納
          
        }

        PlayerRigidbodies = new Rigidbody[P.Length];//格納した数だけRigidbodyを宣言

        for (int i = 0; i < P.Length; i++)
        {
            PlayerRigidbodies[i] = P[i].GetComponent<Rigidbody>();
        }


        for (int i = 0; i < P.Length; i++)
        {
            Debug.Log("Obstacle" + P[i].tag);
            if (P[i].tag == "Obstacle")
            {
                if (P[i].TryGetComponent<ObstacleExplosion>(out ObstacleExplosion obstacle))
                {
                    obstacle.Explosion(transform.position, BombStrangeValue);
                    continue;
                }
            }

            PlayerRigidbodies[i].linearVelocity = PlayerRigidbodies[i].linearVelocity * 0.7f + (P[i].transform.position - this.transform.position).normalized * BombStrangeValue;
            //最後に受けた爆発の影響が出やすくなるように今のVectorに0,7を掛ける

            if (P[i].tag == "enemy")
            {

                PlayerRigidbodies[i].isKinematic = false;
                if (P[i].TryGetComponent<EnemiesAttack>(out EnemiesAttack EA))
                {
                    EA.willDestoroy = true;
                    
                }
                if(P[i].TryGetComponent<enemyMove>(out enemyMove EM))
                {
                    EM.willDestroy = true;

                }


                Destroy(P[i], DestroyEnemyTimer);//DestoryEnemyTimer秒後に消滅
            }
            else if( P[i].tag == "Attack2")//敵の弾を爆弾で防ぐ際はこれを使用
            {
                PlayerRigidbodies[i].linearVelocity = PlayerRigidbodies[i].linearVelocity * 0.1f;
            }

            PlayerRigidbodies[i].linearVelocity = PlayerRigidbodies[i].linearVelocity * 0.7f + (P[i].transform.position - this.transform.position).normalized * BombStrange;
            //最後に受けた爆発の影響が出やすくなるように今のVectorに0,7を掛ける
        }

        Destroy(Instantiate(particle, this.transform.position, Quaternion.identity), 2.0f);    
    }
}
