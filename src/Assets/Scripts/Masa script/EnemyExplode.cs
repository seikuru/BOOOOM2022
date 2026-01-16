using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    [SerializeField] GameObject explodePrefab;

    [SerializeField] float createDistance = 4f;
    [SerializeField] float destroyTime = 5f;

    static GameObject ExplodePrefab;
    static float CreateDistance;
    static float DestroyTime;

    void Awake()
    {
        ExplodePrefab = explodePrefab;
        CreateDistance = createDistance;
        DestroyTime = destroyTime;
    }

    static public void CreateExplode(Transform BombTF, Transform TargetTF)
    {
        Vector3 dir = (TargetTF.position - BombTF.position).normalized;
        Vector3 createPos = TargetTF.position + dir * CreateDistance;

        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

        GameObject create = Instantiate(ExplodePrefab, createPos, rot);
        
        Destroy(create, DestroyTime);


        //Quaternion quaternion = Quaternion.LookRotation
    }
}
