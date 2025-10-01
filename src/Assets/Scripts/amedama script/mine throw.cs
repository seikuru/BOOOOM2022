using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class minethrow : EnemyActBase
{


    [SerializeField] GameObject mineObject;
    [SerializeField,Range(0.0f,90.0f)] float ThrowAngle = 45.0f;
    [SerializeField] float ThrowInterval = 1.0f;
    [SerializeField] float DistanceTarget = 50.0f;
    [SerializeField] float GravityMultiply = 1.0f;//投射物の重力をどれくらい掛けるか
    [SerializeField] int BurstThrow = 5;//何回バーストするか
    [SerializeField] int BurstDelay = 10;//何フレームで1バースト行うか
    

    GameObject player;
    WaitForFixedUpdate waitFixedUpdate;
    Coroutine coroutine;
    Vector3 ProjectionVector = Vector3.zero;
    int Countdown = 0;
    int BurstCount = 0;
    int BurstDelayCount = 0;
    bool CanThrow = true;

    // Start is called before the first frame update
    public override void Act_Start()
    {

        player = GameObject.FindWithTag("Player");
        Countdown = (int)(ThrowInterval / Time.fixedDeltaTime);
        waitFixedUpdate = new WaitForFixedUpdate();
        BurstCount = BurstThrow;
        BurstDelayCount = BurstDelay;

    }

    // Update is called once per frame
    public override void Act_FixedUpdate()
    {  
        StartCoroutine(ThrowMine());
    }

    IEnumerator ThrowMine()
    {

        for (int i = 0; i < BurstCount; i++)
        {

            Vector3 EnemyPoint = (new Vector3(this.transform.position.x, 0, this.transform.position.z));
            Vector3 PlayerPoint = (new Vector3(player.transform.position.x, 0, player.transform.position.z) + EnemyPoint) / 2;

            GameObject mine;
            minegenerate _minegenerate;

            mine = Instantiate(mineObject, this.transform.position, Quaternion.identity);
            _minegenerate = mine.GetComponent<minegenerate>();
            _minegenerate.setParamator(GravityMultiply);

            ProjectionVector = CalculateVelocity(this.transform.position, PlayerPoint);
            mine.GetComponent<Rigidbody>().AddForce(ProjectionVector, ForceMode.Impulse);

            for (int j = 0; j < BurstDelay; j++)
            {
                yield return waitFixedUpdate;
            }
        }

    }


    private Vector3 CalculateVelocity(Vector3 SpawnPoint, Vector3 PlayerPoint)
    {
        float rad = ThrowAngle * Mathf.Deg2Rad;
        float x = Vector2.Distance(new Vector2(SpawnPoint.x, SpawnPoint.z), new Vector2(PlayerPoint.x, PlayerPoint.z));
        float y = PlayerPoint.y - SpawnPoint.y;
        float speed = Mathf.Sqrt(-Physics.gravity.y * GravityMultiply * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) - y)));
        
        if (float.IsNaN(speed))
        {
            return Vector3.zero;
        }
        else
        {
            return new Vector3(PlayerPoint.x - SpawnPoint.x, x * Mathf.Tan(rad), PlayerPoint.z - SpawnPoint.z).normalized * speed;
        }
    }
}
