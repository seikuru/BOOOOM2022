using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectObjectRePop : MonoBehaviour
{
    [SerializeField] float repopCount = 5f;

    [SerializeField] float ScaleUpTime = 1f;

    [SerializeField] GameObject RepopPrehab;

    [SerializeField] private GameObject[] CollectObjects;

    ObstacleExplosion[] obstacleExplosions;
    Vector3[] RepopPos;
    Vector3[] RepopScale;
    int ObjectsLength;

    bool BreakObject = false;
    bool once = true;
    float count = 0;

    // Start is called before the first frame update
    void Start()
    {
        ObjectsLength = CollectObjects.Length;
        obstacleExplosions = new ObstacleExplosion[ObjectsLength];
        RepopPos = new Vector3[ObjectsLength];
        RepopScale = new Vector3[ObjectsLength];

        for (int i = 0; i < ObjectsLength; i++)
        {
            RepopPos[i] = CollectObjects[i].transform.position;
            RepopScale[i] = CollectObjects[i].transform.localScale;

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

        for (int i = 0; i < ObjectsLength; i++)
        {
            if (CollectObjects[i] != null && !obstacleExplosions[i].IsExplosed)
            {
                continue;
            }
            else
            {
                BrokeNumber++;
                if (BrokeNumber >= ObjectsLength)
                {
                    BreakObject = true;
                }
            }       
        }

        if (BreakObject && once)
        {
            once = false;
            DestroyObstcleCount.DestroyAddCount();
            Destroy(this.gameObject);
        }

        if(BrokeNumber > 0)
        {
            count += Time.deltaTime;

            if (count > repopCount)
            {
                count = 0;

                for (int i = 0; i < ObjectsLength; i++)
                {
                    if (CollectObjects[i] == null)
                    {
                        CollectObjects[i] = Instantiate(RepopPrehab, RepopPos[i], Quaternion.identity, this.transform);
                        if (CollectObjects[i].TryGetComponent<ObstacleExplosion>(out ObstacleExplosion OE))
                        {
                            obstacleExplosions[i] = OE;
                        }

                        StartCoroutine(RepopUpScale(i));
                    }
                }
            }
        }
        else
        {
            count = 0;
        }
    }

    private IEnumerator RepopUpScale(int index)
    {
        float elapsed = 0f;

        while (elapsed <= ScaleUpTime)
        {
            elapsed += Time.unscaledDeltaTime;

            if (CollectObjects[index] != null)
            {
                float t = Mathf.Clamp01(elapsed / ScaleUpTime);

                CollectObjects[index].transform.localScale = RepopScale[index] * t;
            }

            yield return null;
        }

        if (CollectObjects[index] != null)
            CollectObjects[index].transform.localScale = RepopScale[index];
    }
}
