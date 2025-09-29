using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttackPattern : MonoBehaviour
{

    [SerializeField] EnemyActBace[] minethrow;
    [SerializeField] float AttackInterval = 7.0f;

    int AttackNumber = 0;
    int AttackIntervalCount = 0;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        foreach (EnemyActBace act in minethrow)
        { 
           act.Act_Start();
        }

        AttackIntervalCount = (int)(AttackInterval / Time.fixedDeltaTime);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AttackIntervalCount--;

        if (AttackIntervalCount < 0)
        {
            minethrow[AttackNumber].Act_FixedUpdate();

            if (AttackNumber == minethrow.Length - 1)
            { 
                AttackNumber = 0;
            }
            else
            {
                AttackNumber++;
            }

            
            AttackIntervalCount = (int)(AttackInterval / Time.fixedDeltaTime);

        }

    }

}
