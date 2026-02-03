using UnityEngine;

public class EnemyActionSingle : MonoBehaviour
{
    [SerializeField] EnemyActBace enemyActBace;

    void Start()
    {
        enemyActBace.Act_Start();
    }

    
    void FixedUpdate()
    {
        enemyActBace.Act_FixedUpdate();
    }
}
