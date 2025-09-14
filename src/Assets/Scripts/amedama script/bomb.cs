using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    [SerializeField] GameObject Bomb;
    [SerializeField] GameObject ThrowBombSpawnPosition;//前に投げる際に参照する位置
    [SerializeField] GameObject JumpBombSpawnPosition;//下に投げる際に参照する位置
    [SerializeField] GameObject BrinkBombSpawnPosition;//後ろに投げる際に参照する位置
    [SerializeField] float Bombthrow;//爆弾を投げる強さ
    [SerializeField] float Underthrow = 3f;
    [SerializeField] float spawnDistance = 2f;
    [SerializeField] bool InputFlag = false;//パソコン操作時に下に投げるかどうかの判定に用いているflag

    Queue<Bombeffects> BombsQueue;
    Rigidbody PlayerRigidbody;
    
    public void InstantiateUnder()
    {
        // float spawnDistance = 2f;
        Vector3 spawnPos = this.transform.position + (Vector3.down * spawnDistance);

        // 爆弾を生成
        GameObject Spawned_Bomb = Instantiate(Bomb, spawnPos, Quaternion.identity);

        // Rigidbodyを取得
        Rigidbody Bomb_rb = Spawned_Bomb.GetComponent<Rigidbody>();

        Vector3 _force = Vector3.down * Underthrow;

        // 投げる力にプレイヤーの移動速度（慣性）を加算する
        // Vector3 _force = direction * (Bombthrow + PlayerRigidbody.linearVelocity.magnitude);
        // Vector3 _force = direction * ThrowPower + this.gameObject.GetComponent<Rigidbody>().linearVelocity * 0.4f;

        Vector3 _Inertia = PlayerRigidbody.linearVelocity;

        // 力を加える（投擲力＋慣性）
        Bomb_rb.AddForce(_force + _Inertia, ForceMode.VelocityChange);

        // これを使うと瞬時に速度を与えるタイプの力（Impulse）
        // Bomb_rb.AddForce(_force, ForceMode.Impulse);

        // 直接速度に慣性を加算したい場合
        // Bomb_rb.linearVelocity += _Inertia;

        BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
    }

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
        Rigidbody Bomb_rb = Spawned_Bomb.GetComponent<Rigidbody>();

        // 投げる力（プレイヤーの移動速度を加味する）
        // Bombthrow + プレイヤーの速度の大きさ × percentage
        Vector3 _force = direction * (Bombthrow + PlayerRigidbody.linearVelocity.magnitude) * percentage;
        // Vector3 _force = direction * ThrowPower + this.gameObject.GetComponent<Rigidbody>().linearVelocity * 0.4f;

        Vector3 _Inertia = PlayerRigidbody.linearVelocity * (1f - percentage);

        // 投擲力を加える（Impulseは質量を考慮した一時的な力）
        Bomb_rb.AddForce(_force, ForceMode.Impulse);

        // 慣性（プレイヤーの移動速度）を追加で加算
        Bomb_rb.linearVelocity += _Inertia;

        BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
    }

    public void DestroyBombs()
    {
        float waitTime = 0f;

        foreach (Bombeffects bombs in BombsQueue)
        {
            if(bombs == null) 
                continue;

            StartCoroutine(DestroyBombsRoutine(waitTime,bombs));

            waitTime += Time.fixedDeltaTime;
        }

        BombsQueue.Clear();
    }

    private IEnumerator DestroyBombsRoutine(float WaitTime,Bombeffects bombs)
    {
        // FixedUpdate のタイミングまで待機
        yield return new WaitForSeconds(WaitTime);

        if (bombs != null)
        {
            bombs.Bakuhatu();
            Debug.Log("Bakuhatu");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        BombsQueue = new Queue<Bombeffects>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!InputFlag)
            return;

#if UNITY_EDITOR

        if ((Input.GetMouseButton(0) && Input.GetMouseButtonUp(1)) || 
            (Input.GetMouseButtonUp(0) && Input.GetMouseButton(1)))//左右のマウスボタンが両方押されているとき
        {
            GameObject Spawned_Bomb;
            Spawned_Bomb = Instantiate(Bomb, JumpBombSpawnPosition.transform.position, Quaternion.identity);
            Spawned_Bomb.GetComponent<Rigidbody>().AddForce
                (-this.transform.up + this.gameObject.GetComponent<Rigidbody>().linearVelocity
                , ForceMode.Impulse);

            BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
        }

        else if (Input.GetMouseButtonUp(0))
        {
            GameObject Spawned_Bomb;
            Spawned_Bomb = Instantiate(Bomb, ThrowBombSpawnPosition.transform.position, Quaternion.identity);
            Spawned_Bomb.GetComponent<Rigidbody>().AddForce(this.transform.forward * (Bombthrow + this.gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude /** 0.8f*/ ), ForceMode.Impulse);

            BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
        }

        else if (Input.GetMouseButtonUp(1))
        {
            GameObject Spawned_Bomb;
            Spawned_Bomb = Instantiate(Bomb, BrinkBombSpawnPosition.transform.position, Quaternion.identity);
            Spawned_Bomb.GetComponent<Rigidbody>().AddForce(-this.transform.forward * 5.0f + this.gameObject.GetComponent<Rigidbody>().linearVelocity, ForceMode.Impulse);

            BombsQueue.Enqueue(Spawned_Bomb.GetComponent<Bombeffects>());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            DestroyBombs();
        } 
        
#endif

    }
}


