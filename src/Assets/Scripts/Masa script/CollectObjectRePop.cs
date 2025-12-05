using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectObjectRePop : MonoBehaviour
{
    [SerializeField] float repopCount = 5f;
    [SerializeField] GameObject RepopPrehab;

    [SerializeField] private GameObject[] CollectObjects;
    ObstacleExplosion[] obstacleExplosions;
    Vector3[] RepopPos; 
    int ObjectValue;

    bool BreakObject = false;
    bool once = true;
    float count = 0;

    // Start is called before the first frame update
    void Start()
    {
        ObjectValue = CollectObjects.Length;
        obstacleExplosions = new ObstacleExplosion[ObjectValue];
        RepopPos = new Vector3[ObjectValue];

        for (int i = 0; i < ObjectValue; i++)
        {
            RepopPos[i] = CollectObjects[i].transform.position;

            if (CollectObjects[i].TryGetComponent<ObstacleExplosion>
                (out ObstacleExplosion OE))
            {
                obstacleExplosions[i] = OE;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        int BrokeNumber = 0;

        for (int i = 0; i < ObjectValue; i++)
        {
            if (CollectObjects[i] != null && !obstacleExplosions[i].IsExplosed)
            {
                continue;
            }
            else
            {
                BrokeNumber++;
                if (BrokeNumber >= ObjectValue)
                {
                    BreakObject = true;
                }
            }       
        }

        if (BreakObject && once)
        {
            once = false;
            Destroy(this.gameObject);
        }

        if(BrokeNumber > 0)
        {
            count += Time.deltaTime;

            if (count > repopCount)
            {
                count = 0;

                for (int i = 0; i < ObjectValue; i++)
                {
                    if (CollectObjects[i] == null)
                    {
                        CollectObjects[i] = Instantiate(RepopPrehab, RepopPos[i], Quaternion.identity, this.transform);
                        if (CollectObjects[i].TryGetComponent<ObstacleExplosion>(out ObstacleExplosion OE))
                        {
                            obstacleExplosions[i] = OE;
                        }
                    }
                }
            }
        }
        else
        {
            count = 0;
        }
    }
}
