using UnityEngine;

public class EnemyActionSingle : MonoBehaviour
{
    [SerializeField] EnemyActBase enemyActBase;

    void Start()
    {
        enemyActBase.Act_Start();
    }

    
    void FixedUpdate()
    {
        enemyActBase.Act_FixedUpdate();
    }
}
