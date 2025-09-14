using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class bomb : MonoBehaviour
{
    [SerializeField] GameObject Bomb;
    [SerializeField] GameObject ThrowBombSpawnPosition;//前に投げる際に参照する位置
    [SerializeField] GameObject JumpBombSpawnPosition;//下に投げる際に参照する位置
    [SerializeField] GameObject BrinkBombSpawnPosition;//後ろに投げる際に参照する位置
    [SerializeField] GameObject PlayerModelObject;
    [SerializeField] float Bombthrow;//爆弾を投げる強さ
    [SerializeField] float Underthrow = 3f;
    [SerializeField] float JumpCoolDownTime = 5;
    [SerializeField] float spawnDistance = 2f;
    [SerializeField] bool InputFlag = false;//パソコン操作時に下に投げるかどうかの判定に用いているflag
    [SerializeField] int SlapeFlame = 6;
    bool JumpCoolDown = false;
    float JumpCoolDownTimer = 0;

     Queue<GameObject> BombsQueue;
    Rigidbody PlayerRigidbody;

    public struct QuaternionSlape 
    {
        public Quaternion player;
        public Quaternion Look;

        public float clamp;
    }
    Queue<QuaternionSlape> slapesQueue;

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

        BombsQueue.Enqueue(Spawned_Bomb);
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

        BombsQueue.Enqueue(Spawned_Bomb);
    }

    void PlayerModelRotate(Quaternion rotation)
    {
        slapesQueue = new Queue<QuaternionSlape>();

        for (int i = 0; i <= SlapeFlame; i++)
        {
            QuaternionSlape quaternionSlape = new();

            quaternionSlape.player = PlayerModelObject.transform.rotation;
            quaternionSlape.Look = rotation;
            quaternionSlape.clamp = (float)i / SlapeFlame;
           
            slapesQueue.Enqueue(quaternionSlape);
        }
    }

    public void DestroyBombs()
    {
        //GameObject[] Bombs = GameObject.FindGameObjectsWithTag("Bomb");

        float a = 0.0f;

        foreach (GameObject bombs in BombsQueue)
        {
            Destroy(bombs, a);
            a += 0.02f;
        }

        BombsQueue.Clear();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        BombsQueue = new Queue<GameObject>();
        slapesQueue = new Queue<QuaternionSlape>();
    }

    // Update is called once per frame
    void Update()
    {
        if(slapesQueue.Count > 0)
        {
            var slape = slapesQueue.Dequeue();
            var quaternion = Quaternion.Slerp(slape.player, slape.Look, slape.clamp);
            
            PlayerModelObject.transform.rotation = quaternion;        
        }

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

        }
        else if (Input.GetMouseButtonUp(0))
        {


            GameObject Spawned_Bomb;
            Spawned_Bomb = Instantiate(Bomb, ThrowBombSpawnPosition.transform.position, Quaternion.identity);
            Spawned_Bomb.GetComponent<Rigidbody>().AddForce(this.transform.forward * (Bombthrow + this.gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude /** 0.8f*/ ), ForceMode.Impulse);

            

        }
        else if (Input.GetMouseButtonUp(1))
        {

            GameObject Spawned_Bomb;
            Spawned_Bomb = Instantiate(Bomb, BrinkBombSpawnPosition.transform.position, Quaternion.identity);
            Spawned_Bomb.GetComponent<Rigidbody>().AddForce(-this.transform.forward * 5.0f + this.gameObject.GetComponent<Rigidbody>().linearVelocity, ForceMode.Impulse);
            

        }

        if (Input.GetKeyUp(KeyCode.Space) )
        {
            GameObject[] Bombs = GameObject.FindGameObjectsWithTag("Bomb");

            float a = 0.0f;

            foreach (GameObject bombs in Bombs)
            {
                Destroy(bombs, a);
                a += 0.02f;
            }
        }      
#endif
    }

    private void FixedUpdate()
    {
        if (JumpCoolDown)
        {
            JumpCoolDownTimer += 0.02f;


            if (JumpCoolDownTimer >= JumpCoolDownTime)
            {
                JumpCoolDown = false;
                JumpCoolDownTimer = 0;
            }

        }

    }
}


