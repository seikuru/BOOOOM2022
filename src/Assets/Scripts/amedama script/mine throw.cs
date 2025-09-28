using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class minethrow : MonoBehaviour
{


    [SerializeField] GameObject mineObject;
    [SerializeField,Range(0.0f,90.0f)] float ThrowAngle = 45.0f;
    [SerializeField] float ThrowInterval = 1.0f;
    [SerializeField] float DistanceTarget = 50.0f;
    [SerializeField] float GravityMultiply = 1.0f;
    [SerializeField] int BurstThrow = 5;

    GameObject player;
    Rigidbody mineRB;
    WaitForSeconds waitForSeconds;
    Coroutine coroutine;
    Vector3 ProjectionVector = Vector3.zero;
    int Countdown = 0;
    
    bool CanThrow = true;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag("Player");
        waitForSeconds = new WaitForSeconds(ThrowInterval);
        Countdown = (int)(ThrowInterval / Time.fixedDeltaTime);
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Countdown--;

        if (Vector3.Distance(player.transform.position, this.transform.position) <= DistanceTarget && CanThrow == true)
        {
            //coroutine = StartCoroutine(ThrowMine());
            //mineRB = ThrowMine2();
            CanThrow = false;
        }
        else
        {

        }

        if (Countdown == 0)
        {
            
            mineRB = ThrowMine2(player);
            Countdown = (int)(ThrowInterval / Time.fixedDeltaTime);
        }

        if (mineRB != null)
        {

            mineRB.AddForce((GravityMultiply - 1) * Physics.gravity, ForceMode.Force);

        }

    }

    public IEnumerator ThrowMine()
    {
        yield return waitForSeconds;

        Vector3 EnemyPoint = (new Vector3(this.transform.position.x, 0, this.transform.position.z));
        Vector3 PlayerPoint = (new Vector3(player.transform.position.x,0,player.transform.position.z) + EnemyPoint) / 2;
        
        GameObject mine;
        //Destroy(mine = Instantiate(mineObject, transform.position + ((player.transform.position - transform.position).normalized) * 5, Quaternion.identity), DestroyTime);
        mine = Instantiate(mineObject,this.transform.position,Quaternion.identity);
        ProjectionVector = CalculateVelocity(this.transform.position,PlayerPoint );
        mine.GetComponent<Rigidbody>().AddForce(ProjectionVector,ForceMode.Impulse);

        CanThrow = true;
        
      
    }

    public Rigidbody ThrowMine2(GameObject player)
    {


        Vector3 EnemyPoint = (new Vector3(this.transform.position.x, 0, this.transform.position.z));
        Vector3 PlayerPoint = (new Vector3(player.transform.position.x, 0, player.transform.position.z) + EnemyPoint) / 2;
        GameObject mine;
        Rigidbody mineRB;
        //Destroy(mine = Instantiate(mineObject, transform.position + ((player.transform.position - transform.position).normalized) * 5, Quaternion.identity), DestroyTime);
        mine = Instantiate(mineObject, this.transform.position, Quaternion.identity);
        ProjectionVector = CalculateVelocity(this.transform.position, PlayerPoint);
        mineRB = mine.GetComponent<Rigidbody>();
        mineRB.AddForce(ProjectionVector, ForceMode.Impulse);
        Debug.Log(ProjectionVector);
        

        CanThrow = true;
        return mineRB;

    }

    IEnumerator Wait()
    {
        yield return waitForSeconds;
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
