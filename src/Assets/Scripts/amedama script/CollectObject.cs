using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CollectObject : MonoBehaviour
{
    [HideInInspector] static public int CollectPoint;
    [SerializeField] private GameObject[] CollectObjects;
    ObstacleExplosion[] obstacleExplosions;
    bool BreakObject = false;
    bool once = true;
    int BrokeNumber;
    // Start is called before the first frame update
    void Start()
    {
        obstacleExplosions = new ObstacleExplosion[CollectObjects.Length];
        int i = 0;
        foreach (var obj in CollectObjects)
        {
            if (obj.TryGetComponent<ObstacleExplosion>(out ObstacleExplosion OE))
            {
                
                obstacleExplosions[i] = OE;
            }
            
                i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (var obj in CollectObjects)
        {
            Debug.Log(obstacleExplosions[i].IsExplosed);
                if (obj != null && !obstacleExplosions[i].IsExplosed)
                {
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
            i++;
            if (BreakObject && once)
            {
                CollectPoint++;
                once = false;
                DestroyObstcleCount.DestroyAddCount();
                Destroy(this.gameObject);
            }
        }


        BrokeNumber = 0;

    }

}
