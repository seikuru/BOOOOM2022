using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounsTimeSetiing : MonoBehaviour
{
    [SerializeField]
    GameObject BounsOnjectPrehab;

    [SerializeField,Header("Trueにすると、全て破壊した後常にボーナスを生成する")]
    bool BounsAutoSpawm = false;

    Vector3[] BounsSpawnPos;
    Vector3 LastDestoryPos = Vector3.zero;

    private void Start()
    {
        LastDestoryPos = Vector3.zero;

        BounsSpawnPos = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            BounsSpawnPos[i] = transform.GetChild(i).position;
        }
    }

    public void StartBounsTime()
    {
        foreach (Vector3 Pos in BounsSpawnPos)
        {
            if (LastDestoryPos != Pos)
            {
                GameObject instantiate = Instantiate(BounsOnjectPrehab, Pos, Quaternion.identity);
                instantiate.transform.parent = this.transform; 
            }
               
        }
    }

    private void Update()
    {
        if(transform.childCount == 1)
        {
            LastDestoryPos = transform.GetChild(0).position;
        }

        if(BounsAutoSpawm && transform.childCount == 0)
        {
            StartBounsTime();
        }
    }
}
