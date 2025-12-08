using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static CollectObjectRePop;

public class CollectObjectRePop : MonoBehaviour
{
    [SerializeField] float repopCount = 5f;

    [SerializeField] float ScaleUpTime = 1f;

    [SerializeField] GameObject RepopPrehab;

    [SerializeField] private List<CollectObjectlist> CollectObjects;

    [System.Serializable]
    public class CollectObjectlist 
    { 
        public GameObject[] ObjectList;
    }

    public class RepopGroup
    {
        public ObstacleExplosion[] obstacleExplosions;
        public Vector3[] RepopPos;
        public Vector3[] RepopScale;

        public int ObjectLength;

        public bool BreakObject = false;
        public bool AllBreak = true;
        public float count = 0;
    }
    
    private RepopGroup[] RepopGroups;

    int GroupLength;
    bool once = true;

    // Start is called before the first frame update
    void Start()
    {
        GroupLength = CollectObjects.Count;
        RepopGroups = new RepopGroup[GroupLength];

        for (int i = 0; i < GroupLength; i++)
        {
            RepopGroups[i] = new RepopGroup();

            var Group = RepopGroups[i];

            Group.ObjectLength = CollectObjects[i].ObjectList.Length;

            Group.obstacleExplosions = new ObstacleExplosion[Group.ObjectLength];
            Group.RepopPos = new Vector3[Group.ObjectLength];
            Group.RepopScale = new Vector3[Group.ObjectLength];

            for (int j = 0; j < Group.ObjectLength; j++)
            {
                Group.RepopPos[j] = CollectObjects[i].ObjectList[j].transform.position;
                Group.RepopScale[j] = CollectObjects[i].ObjectList[j].transform.localScale;

                if (CollectObjects[i].ObjectList[j].TryGetComponent<ObstacleExplosion>
                    (out ObstacleExplosion OE))
                {
                    Group.obstacleExplosions[j] = OE;
                }

                Group.AllBreak = false;
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        BrokunCheck();

        if(IsAllBreak() && once)
        {
            once = false;
            DestroyObstcleCount.DestroyAddCount();
            Destroy(this.gameObject);
        }

        RepopCheck();
    }

    void BrokunCheck()
    {
        for (int i = 0; i < GroupLength; i++)
        {
            var Group = RepopGroups[i];

            if (Group.AllBreak)
                continue;

            int BrokunValue = 0;

            for (int j = 0; j < Group.ObjectLength; j++)
            {
                // オブジェクトがある状態、かつまだ爆発していなければスキップ
                if (CollectObjects[i].ObjectList[j] != null && !Group.obstacleExplosions[i].IsExplosed)
                {
                    continue;
                }
                else
                {
                    BrokunValue++;
                }
            }

            if (BrokunValue >= Group.ObjectLength)
            {
                Group.AllBreak = true;
            }
            else if (BrokunValue > 0)
            {
                Group.BreakObject = true;
            }
        }
    }

    bool IsAllBreak()
    {
        foreach (var group in RepopGroups)
        {
            if (!group.AllBreak)
                return false;
        }

        return true;
    }

    void RepopCheck()
    {
        for (int i = 0; i < GroupLength; i++)
        {
            var Group = RepopGroups[i];

            if (Group.AllBreak)
                continue;

            if (Group.BreakObject)
            {
                Group.count += Time.deltaTime;

                if (Group.count > repopCount)
                {
                    Group.count = 0;

                    for (int j = 0; j < Group.ObjectLength; j++)
                    {
                        if (CollectObjects[i].ObjectList[i] == null)
                        {
                            CollectObjects[i].ObjectList[j] = Instantiate(RepopPrehab, Group.RepopPos[j], Quaternion.identity, this.transform);
                            if (CollectObjects[i].ObjectList[j].TryGetComponent<ObstacleExplosion>(out ObstacleExplosion OE))
                            {
                                Group.obstacleExplosions[j] = OE;
                            }

                            StartCoroutine(RepopUpScale(i,j));
                        }
                    }
                }
            }
            else
            {
                Group.count = 0;
            }
        }
    }

    private IEnumerator RepopUpScale(int Groupindex, int Objectindex)
    {
        float elapsed = 0f;

        while (elapsed <= ScaleUpTime)
        {
            elapsed += Time.unscaledDeltaTime;

            if (CollectObjects[Groupindex].ObjectList[Objectindex] != null)
            {
                float t = Mathf.Clamp01(elapsed / ScaleUpTime);

                CollectObjects[Groupindex].ObjectList[Objectindex].transform.localScale =
                    RepopGroups[Groupindex].RepopScale[Objectindex] * t;
            }

            yield return null;
        }

        if (CollectObjects[Groupindex].ObjectList[Objectindex] != null)
            CollectObjects[Groupindex].ObjectList[Objectindex].transform.localScale =
                RepopGroups[Groupindex].RepopScale[Objectindex];
    }
}
