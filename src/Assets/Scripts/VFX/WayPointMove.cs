using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointMove : MonoBehaviour
{
    [SerializeField] GameObject wayP;
    [SerializeField] List<GameObject> wayPoints;
    [SerializeField] float moveSpeed;

    private int currentPoint = 0;
    private int goalPoint = 0;
    private Vector3 goalPosition;
    private Collider[] cols;

    void Start()
    {
        cols = this.gameObject.GetComponents<Collider>();
        this.transform.position = wayPoints[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentPoint != goalPoint)
        {
            var direction = goalPosition - this.transform.position;
            var nextPos = this.transform.position + direction * Time.deltaTime * moveSpeed;
            var newDirection = goalPosition - nextPos;
            if(Vector3.Dot(direction, newDirection) > 0.9f)
            {
                this.transform.position = nextPos;
            }
            else
            {
                this.transform.position = goalPosition;
                EnableCollder();
                currentPoint = goalPoint;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Player") return;
        Debug.Log("collision player");
        if(goalPoint < wayPoints.Count - 2) // 当たったのがプレイヤーで動く先があるなら
        {
            goalPoint++;
            goalPosition = wayPoints[goalPoint].transform.position;
            DisableCollider();
        }
        else
        {
            wayP.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger something");
        if (other.transform.tag != "Player") return;
        Debug.Log("trigger player");
        if (goalPoint < wayPoints.Count - 1) // 当たったのがプレイヤーで動く先があるなら
        {
            goalPoint++;
            goalPosition = wayPoints[goalPoint].transform.position;
            DisableCollider();
        }
        else
        {
            wayP.SetActive(false);
        }
    }

    // コライダーの無効化
    void DisableCollider()
    {
        foreach(Collider col in cols)
        {
            col.enabled = false;
        }
    }

    // コライダーの有効化
    void EnableCollder()
    {
        foreach(Collider col in cols)
        {
            col.enabled = true;
        }
    }

    // ギズモの表示
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        // 設定されたポイントに球を表示
        foreach(var p in wayPoints)
        {
            if(p == null) continue;
            Gizmos.DrawSphere(p.transform.position, 1);
        }

        // 移動順にラインを表示
        for(int i = 0; i < wayPoints.Count - 1; i++)
        {
            if (wayPoints[i] == null || wayPoints[i+1] == null) continue;
            Gizmos.DrawLine(wayPoints[i].transform.position, wayPoints[i+1].transform.position);
        }
    }
}
