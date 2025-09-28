using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttackPattern : MonoBehaviour
{
    [SerializeField] UnityEvent aaa;
    [SerializeField] UnityEvent<GameObject> bbb;
    [SerializeField] Event ccc;
    [SerializeField] Func<IEnumerator> MineThrowFunc;
    [SerializeField] Func<GameObject,Rigidbody> MineThrowFunc2;
    [SerializeField] minethrow[] minethrow;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        MineThrowFunc = minethrow[0].ThrowMine;
        MineThrowFunc2 = minethrow[0].ThrowMine2;

        MineThrowFunc2(player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Mine()
    {

    }

}
