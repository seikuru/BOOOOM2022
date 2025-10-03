using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
class AttackPatternClass
{
    public EnemyActBase AttackPattern;
    public float AttackInterval;

}

public class EnemyAttackPattern : MonoBehaviour
{

    [SerializeField] AttackPatternClass[] enemyAttackPattern;

    [HideInInspector] public bool willDestroy = false;
    int AttackNumber = 0;
    int AttackIntervalCount = 0;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        foreach (AttackPatternClass act in enemyAttackPattern)
        {
            act.AttackPattern.Act_Start();
        }

        AttackIntervalCount = (int)(enemyAttackPattern[0].AttackInterval / Time.fixedDeltaTime);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!willDestroy)
        {
            AttackIntervalCount--;
        }

        if (AttackIntervalCount < 0)
        {
            enemyAttackPattern[AttackNumber].AttackPattern.Act_FixedUpdate();

            if (AttackNumber == enemyAttackPattern.Length - 1)
            { 
                AttackNumber = 0;
            }
            else
            {
                AttackNumber++;
            }

            AttackIntervalCount = (int)(enemyAttackPattern[AttackNumber].AttackInterval / Time.fixedDeltaTime);

        }

    }

}
