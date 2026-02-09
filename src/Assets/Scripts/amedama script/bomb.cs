using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    [SerializeField] GameObject Bomb;
    [SerializeField] GameObject Bomb2;
    [SerializeField] GameObject ThrowBombSpawnPosition;//前に投げる際に参照する位置
    [SerializeField] GameObject JumpBombSpawnPosition;//下に投げる際に参照する位置
    [SerializeField] GameObject BrinkBombSpawnPosition;//後ろに投げる際に参照する位置
    [SerializeField] float bombThrowPower = 20f;//爆弾を投げる強さ
    [SerializeField] float UnderthrowForce = 3f;
    [SerializeField] float spawnDistance = 2f;
    [SerializeField] float spawnDistanceUnder = 1f;
    [SerializeField] bool InputFlag = false;//パソコン操作時に下に投げるかどうかの判定に用いているflag
    [SerializeField] bool FullautoEnable = false;
    [SerializeField] int BombShotInterval = 25;//爆弾を投げる間隔
    [SerializeField] float AudioCoolTime = 0.1f;
    [SerializeField] AudioSource ThrowAudioSource;
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] RotationByVector rotationByVector;

    Queue<Bombeffects> BombsQueue;
    Animator PlayerAnimator;
    Rigidbody PlayerRigidbody;
    int ShotInterval_Count = 0;
    private bool PlayerHit = false;
    float AudioCount, DestroyWait;
    bool AudioPlayFlag;

    public void InstantiateUnder()
    {
        // float spawnDistance = 2f;
        Vector3 spawnPos = this.transform.position + (Vector3.down * spawnDistanceUnder);

        // 爆弾を生成
        GameObject Spawned_Bomb = Instantiate(Bomb2, spawnPos, Quaternion.identity);

        // Rigidbodyを取得
        Rigidbody Bomb_rb = Spawned_Bomb.GetComponent<Bombeffects>().GetRB;

        Vector3 _force = Vector3.down * UnderthrowForce;

        // 投げる力にプレイヤーの移動速度（慣性）を加算する
        // Vector3 _force = direction * (Bombthrow + PlayerRigidbody.linearVelocity.magnitude);
        // Vector3 _force = direction * ThrowPower + this.gameObject.GetComponent<Rigidbody>().linearVelocity * 0.4f;

        Vector3 _Inertia = PlayerRigidbody.velocity;

        // 力を加える（投擲力＋慣性）
        Bomb_rb.AddForce(_force + _Inertia, ForceMode.VelocityChange);

        // これを使うと瞬時に速度を与えるタイプの力（Impulse）
        // Bomb_rb.AddForce(_force, ForceMode.Impulse);

        // 直接速度に慣性を加算したい場合
        // Bomb_rb.linearVelocity += _Inertia;

        BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
        
        // Animator にトリガーを送信
        playerAnimation.ThrowUnder();

        

        AudioPlayFlag = true;
    }

    /// <summary>
    /// 爆弾を横方向に投げる
    /// </summary>
    /// <param name="percentage">0～1の範囲で投げる力を乗算で調整</param>
    /// <param name="direction">爆弾を投げる方向</param>
    /// <param name="rotation">爆弾の向き</param>
    public void InstantiateBomb(float percentage, Vector3 direction, Quaternion rotation)
    {
        // プレイヤーの前方位置にオフセットして爆弾を生成
        // float spawnDistance = 1.0f; // 必要に応じて設定
        Vector3 spawnPos = this.transform.position + direction.normalized * spawnDistance;

        // 向いている方向に回転を合わせて投擲（direction方向に向ける）
        //Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        //PlayerModelRotate(rotation);

        // 爆弾を生成
        GameObject Spawned_Bomb = Instantiate(Bomb, spawnPos, rotation);

        // Rigidbodyを取得
        Rigidbody Bomb_rb = Spawned_Bomb.GetComponent<Bombeffects>().GetRB;

        // 投げる力（プレイヤーの移動速度を加味する）
        // Bombthrow + プレイヤーの速度の大きさ × percentage
        // Vector3 _force = (Bombthrow + PlayerRigidbody.velocity.magnitude) * percentage * direction;
        Vector3 _force = (bombThrowPower * percentage) * direction.normalized;
        // Vector3 _force = direction * ThrowPower + this.gameObject.GetComponent<Rigidbody>().linearVelocity * 0.4f;

        // Vector3 _Inertia = PlayerRigidbody.velocity * (1f - percentage);
        Vector3 _Inertia = PlayerRigidbody.velocity;

        // 投擲力を加える（Impulseは質量を考慮した一時的な力）
        Bomb_rb.AddForce(_force, ForceMode.Impulse);

        // 慣性（プレイヤーの移動速度）を追加で加算
        Bomb_rb.velocity += _Inertia;

        BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());

        // Animator にトリガーを送信
        playerAnimation.onThrow();

        //爆弾を投げたことをプレイヤーの回転処理に送信
        rotationByVector.Throwing = true;

        AudioPlayFlag = true;
}

    public void DestroyBombs()
    {
        if (Time.timeScale == 0f || DestroyWait< 10)
            return;

        // コンボリセット
        ScoreManager.ComboReset();

        float waitTime = 0f;
        PlayerHit = false;
        int BombNumber = 0;
        int SameCount = 0;

        foreach (Bombeffects bombs in BombsQueue)
        {
            if (bombs == null)
                continue;

            BombNumber++;

            StartCoroutine(DestroyBombsRoutine(waitTime, bombs,BombNumber));
            SameCount++;
            if(SameCount <= 3)
            {
                SameCount = 0;
                waitTime += Time.fixedDeltaTime * 2;
            }

            //Debug.Log("getHit()" + bombs.getHit());
            if (bombs.getHit())
            {
                Debug.Log("bomb hit");
                PlayerHit = true;
                bombs.setHit(false);
            }
        }

        BombsQueue.Clear();
        if(PlayerHit)
        {
            playerAnimation.BombHit();
        }
    }

    private IEnumerator DestroyBombsRoutine(float WaitTime, Bombeffects bombs,int BombNumber)
    {
        //ComboCounter.ResetCombo();

        // FixedUpdate のタイミングまで待機
        yield return new WaitForSeconds(WaitTime);
        

        if (bombs != null)
        {
            bombs.Bakuhatu(BombNumber);
            
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DestroyWait = 0;
        PlayerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        PlayerAnimator = this.gameObject.GetComponent<Animator>();
        BombsQueue = new Queue<Bombeffects>();
        AudioCoolTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DestroyWait = Mathf.Min (DestroyWait + 1, 31);
        if (!InputFlag)
            return;

#if UNITY_EDITOR

        if (FullautoEnable)//フルオートの時
        {

            if ((Input.GetMouseButton(0) && Input.GetMouseButton(1)) && ShotInterval_Count == 0)//左右のマウスボタンが両方押されているとき
            {
                GameObject Spawned_Bomb;
                Spawned_Bomb = Instantiate(Bomb, JumpBombSpawnPosition.transform.position, Quaternion.identity);
                Spawned_Bomb.GetComponent<Bombeffects>().GetRB.AddForce
                    (-this.transform.up + this.gameObject.GetComponent<Rigidbody>().velocity
                    , ForceMode.Impulse);

                BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
                playerAnimation.ThrowUnder();
            }
            else if (Input.GetMouseButton(0) && ShotInterval_Count == 0)
            {
                GameObject Spawned_Bomb;
                Spawned_Bomb = Instantiate(Bomb, ThrowBombSpawnPosition.transform.position, Quaternion.identity);
                Spawned_Bomb.GetComponent<Bombeffects>().GetRB.AddForce(this.transform.forward * (bombThrowPower + this.gameObject.GetComponent<Rigidbody>().velocity.magnitude /** 0.8f*/ ), ForceMode.Impulse);

                BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
                playerAnimation.onThrow();
            }
            else if (Input.GetMouseButton(1) && ShotInterval_Count == 0)
            {
                GameObject Spawned_Bomb;
                Spawned_Bomb = Instantiate(Bomb, BrinkBombSpawnPosition.transform.position, Quaternion.identity);
                Spawned_Bomb.GetComponent<Bombeffects>().GetRB.AddForce(-this.transform.forward * 5.0f + this.gameObject.GetComponent<Rigidbody>().velocity, ForceMode.Impulse);

                BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
                playerAnimation.onThrow();
            }

            ShotInterval_Count++;

            if (!Input.GetMouseButton(1) && !Input.GetMouseButton(0))
            {
                ShotInterval_Count = 0;
            }

            if (ShotInterval_Count >= BombShotInterval)
            {
                ShotInterval_Count = 0;
            }
        }
    }

    void Update()
    {
        if(Time.timeScale == 0f)
            DestroyWait = 0;

        AudioCount += Time.deltaTime;
        if (AudioCount >= AudioCoolTime && AudioPlayFlag)
        {
            AudioCount = 0f;
            AudioPlayFlag = false;
            Debug.Log("Play");
            if(ThrowAudioSource != null)
                ThrowAudioSource.PlayOneShot(ThrowAudioSource.clip);
        }

        if (!InputFlag)
             return;

        if (!FullautoEnable)//フルオートで無い時
        {
            if ((Input.GetMouseButton(0) && Input.GetMouseButtonUp(1)) ||
            (Input.GetMouseButtonUp(0) && Input.GetMouseButton(1)))//左右のマウスボタンが両方押されているとき
            {
                GameObject Spawned_Bomb;
                Spawned_Bomb = Instantiate(Bomb, JumpBombSpawnPosition.transform.position, Quaternion.identity);
                Spawned_Bomb.GetComponent<Bombeffects>().GetRB.AddForce
                    (-this.transform.up + this.gameObject.GetComponent<Rigidbody>().velocity
                    , ForceMode.Impulse);

                BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
                playerAnimation.ThrowUnder();
            }

            else if (Input.GetMouseButtonUp(0))
            {
                GameObject Spawned_Bomb;
                Spawned_Bomb = Instantiate(Bomb, ThrowBombSpawnPosition.transform.position, Quaternion.identity);
                Spawned_Bomb.GetComponent<Bombeffects>().GetRB.AddForce(this.transform.forward * (bombThrowPower + this.gameObject.GetComponent<Rigidbody>().velocity.magnitude /** 0.8f*/ ), ForceMode.Impulse);

                BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
                playerAnimation.onThrow();
            }

            else if (Input.GetMouseButtonUp(1))
            {
                GameObject Spawned_Bomb;
                Spawned_Bomb = Instantiate(Bomb, BrinkBombSpawnPosition.transform.position, Quaternion.identity);
                Spawned_Bomb.GetComponent<Bombeffects>().GetRB.AddForce(-this.transform.forward * 5.0f + this.gameObject.GetComponent<Rigidbody>().velocity, ForceMode.Impulse);

                BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
                playerAnimation.onThrow();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            DestroyBombs();
        }
#endif
    }
}


