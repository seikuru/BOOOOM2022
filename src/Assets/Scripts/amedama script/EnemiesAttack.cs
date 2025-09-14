using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.Image;

public class EnemiesAttack : MonoBehaviour
{

    enum AttackType
    {
        beam,
        shotgun,
        Charge
    };

    [SerializeField] float AttackCooldown = 5.0f;//攻撃の間隔
    [SerializeField] float EnemySearchRadius = 50.0f;//敵が攻撃してくるようになる距離
    [SerializeField] float yokokuDelay = 0.5f;//敵がターゲットしてから攻撃するまでの猶予
    //-----Charge攻撃の際に使用する
    [SerializeField] float ChargePower = 1.3f;//敵が一秒間に自分との距離の何倍移動するか
    [SerializeField] float ChargeTime = 2f;//移動を行う時間
    //-----弾を飛ばす攻撃全般で使用する
    [SerializeField] float ShotLifeTime = 5.0f;//敵の弾がどれくらい長く残るか
    [SerializeField] float ShotStrange = 50.0f;//敵の弾がどれくらいの速度で飛ぶか
    //-----Shotgun攻撃の際に使用する
    [SerializeField] float ShotRange = 0.1f;//ショットガンがどれくらい拡散するか
    [SerializeField] int ShotgunonlyPellet = 1;//ショットガンの拡散パターンの変更。値を増やすと弾がより多く出るようになる。
    [SerializeField] bool yokokuChange = true;//攻撃に予告線をつけるかどうか
    [SerializeField] GameObject ShotObj;//発射する弾
    [SerializeField] AttackType attacktype = AttackType.beam;//攻撃の種類

    [HideInInspector] public bool willDestoroy = false;

    GameObject player;
    Rigidbody rb;
    LineRenderer lineRenderer;
    Color LineColor = Color.green;

    float CountTime = 0.0f;
    bool yokokuOn = false;
    bool ChargeNow = false;//Charge(突進)攻撃中かどうか
    bool StopRutineFlag = false;
    LayerMask PlayerMask;

    Coroutine AttackCorutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerMask = LayerMask.GetMask("Player");
        player = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = GetComponent<LineRenderer>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if (EnemySearchRadius >= Vector3.Distance(transform.position, player.transform.position))//Playerが攻撃範囲に入った時
        {
            if (CountTime > AttackCooldown)
            {
                if (yokokuChange)
                {
                    yokokuOn = true;
                    LineColor.r = 0.0f;
                    LineColor.g = 1.0f;
                    lineRenderer.enabled = true;
                }

                AttackCorutine = StartCoroutine(EnemyAttack());

                CountTime = 0.0f;
            }

        }
        else
        {
            yokokuOn = false;
            lineRenderer.enabled = false;
        }

        if (willDestoroy)
        {

            lineRenderer.enabled = false;
            yokokuOn = false;
            CountTime = 0.0f;

            StopCoroutine(AttackCorutine);
        }
        else
        {
            CountTime += 0.02f;
        }

        if (yokokuOn)
        {

            //徐々に赤色に変化していく
            LineColor.r += 0.02f / AttackCooldown;
            LineColor.g -= 0.02f / AttackCooldown;
            
            Vector3[] yokoku = new Vector3[2] { transform.position, player.transform.position };
            lineRenderer.SetPositions(yokoku);
            lineRenderer.startColor = LineColor;
            lineRenderer.endColor = LineColor;

        }

    }

    IEnumerator EnemyAttack()
    {

        GameObject EnemyShot;

        switch (attacktype)
        {
            case AttackType.beam://一つの弾がまっすぐ飛んでいく攻撃

                EnemyShot = Instantiate(ShotObj, transform.position + ((player.transform.position - transform.position).normalized) * 5, Quaternion.identity);
                EnemyShot.transform.LookAt(player.transform.position);
                EnemyShot.GetComponent<Rigidbody>().AddForce(((player.transform.position - transform.position).normalized) * ShotStrange, ForceMode.Impulse);

                lineRenderer.enabled = false;
                yokokuOn = false;
                Destroy(EnemyShot, ShotLifeTime);

                break;

            case AttackType.shotgun://複数の弾が飛んでいく攻撃

                Vector3 addvector;
                Vector3 dir = player.transform.position - transform.position;
                Vector3 right = Vector3.Cross(dir, Vector3.up).normalized;
                Vector3 up = Vector3.Cross(right, dir).normalized;

                // center
                EnemyShot = Instantiate(ShotObj, transform.position + ((player.transform.position - transform.position).normalized) * 5, Quaternion.identity);
                addvector = dir.normalized;
                EnemyShot.GetComponent<Rigidbody>().AddForce(addvector * ShotStrange, ForceMode.Impulse);
                Destroy(EnemyShot, ShotLifeTime);

                // horizontal
                for (int i = -ShotgunonlyPellet; i <= ShotgunonlyPellet; i++)
                {
                    // 真ん中には撃たない
                    if (i == 0) continue;

                    EnemyShot = Instantiate(ShotObj, transform.position + ((player.transform.position - transform.position).normalized) * 5, Quaternion.identity);
                    addvector = (dir.normalized + right * (float)(i * ShotRange)).normalized;
                    EnemyShot.GetComponent<Rigidbody>().AddForce(addvector * ShotStrange, ForceMode.Impulse);
                    Destroy(EnemyShot, ShotLifeTime);
                }

                // vertical
                for (int i = -ShotgunonlyPellet; i <= ShotgunonlyPellet; i++)
                {
                    // 真ん中には撃たない
                    if (i == 0) continue;

                    EnemyShot = Instantiate(ShotObj, transform.position + ((player.transform.position - transform.position).normalized) * 5, Quaternion.identity);
                    addvector = (dir.normalized + up * (float)(i * ShotRange)).normalized;
                    EnemyShot.GetComponent<Rigidbody>().AddForce(addvector * ShotStrange, ForceMode.Impulse);
                    Destroy(EnemyShot, ShotLifeTime);

                }

                //斜め
                for (int i = -ShotgunonlyPellet + 1; i <= ShotgunonlyPellet - 1; i++)
                {
                    // 真ん中には撃たない
                    if (i == 0) continue;

                    EnemyShot = Instantiate(ShotObj, transform.position + ((player.transform.position - transform.position).normalized) * 5, Quaternion.identity);
                    addvector = (dir.normalized + (up + right) * (float)(i * ShotRange)).normalized;
                    EnemyShot.GetComponent<Rigidbody>().AddForce(addvector * ShotStrange, ForceMode.Impulse);
                    Destroy(EnemyShot, ShotLifeTime);

                }

                for (int i = -ShotgunonlyPellet + 1; i <= ShotgunonlyPellet - 1; i++)
                {
                    // 真ん中には撃たない
                    if (i == 0) continue;

                    EnemyShot = Instantiate(ShotObj, transform.position + ((player.transform.position - transform.position).normalized) * 5, Quaternion.identity);
                    addvector = (dir.normalized + (-up + right) * (float)(i * ShotRange)).normalized;
                    EnemyShot.GetComponent<Rigidbody>().AddForce(addvector * ShotStrange, ForceMode.Impulse);
                    Destroy(EnemyShot, ShotLifeTime);

                }

                lineRenderer.enabled = false;
                yokokuOn = false;

                break;


            case AttackType.Charge://敵がこちらに向かって突進してくる攻撃

                transform.LookAt(player.transform.position);
                rb.AddForce(transform.forward * (ChargePower / ChargeTime) * Vector3.Distance(transform.position, player.transform.position), ForceMode.Impulse);

                yield return new WaitForSeconds(ChargeTime);

                rb.velocity = Vector3.zero;

                yield return null;

                break;
        }
    }

    private void OnDestroy()
    {

        if (AttackCorutine != null)
        {
            StopCoroutine(AttackCorutine);
        }
    }

}
