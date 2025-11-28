using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CollectObject : MonoBehaviour
{
    [HideInInspector] static public int CollectPoint;
    [SerializeField] private GameObject[] CollectObjects;
    bool BreakObject = false;
    bool once = true;
    int BrokeNumber;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var obj in CollectObjects)
        {
            if (obj != null)
            {
                // Debug.Log("able");

                break;
            }
            else
            {
                BrokeNumber++;

                if (BrokeNumber >= CollectObjects.Length)
                {
                    BreakObject = true;
                }
            }

            if (BreakObject && once)
            {
                CollectPoint++;
                Debug.Log(CollectPoint);
                once = false;
                Destroy(this.gameObject);
                
            }
        }


        BrokeNumber = 0;
       
    }

}
