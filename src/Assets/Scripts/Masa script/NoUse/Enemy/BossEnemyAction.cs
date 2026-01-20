using UnityEngine;

public class BossEnemyAction : MonoBehaviour
{
    [SerializeField] EnemyActBase[] enemyActs;

    [SerializeField] float ActInterval = 3f;

    float timeCounter;

    void Start()
    {
        foreach (var Act in enemyActs)
        {
            Act.Act_Start();
        }

        timeCounter = 0;
    }


    void FixedUpdate()
    {
        timeCounter += Time.fixedDeltaTime;

        if(timeCounter > ActInterval)
        {
            timeCounter = 0;

            int ActIndex = Random.Range(0, enemyActs.Length);

            enemyActs[ActIndex].Act_FixedUpdate();
        }
    }
}
