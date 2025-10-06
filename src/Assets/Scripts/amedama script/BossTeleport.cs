using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{

    [SerializeField] GameObject[] TeleportPoint;

    int BossState = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = TeleportPoint[BossState].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            BossStateChange();
        }
    }

    public void BossStateChange()
    {
        
        if (BossState < TeleportPoint.Length - 1)
        {
            BossState++;
            this.transform.position = TeleportPoint[BossState].transform.position;
        }
    }
}
