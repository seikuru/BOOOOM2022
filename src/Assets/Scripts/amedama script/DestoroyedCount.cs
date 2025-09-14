using UnityEngine;

public class DestoroyedCount : MonoBehaviour
{

    EnemyCount enemyCount;

    private void OnDestroy()
    {
        if (GameObject.Find("EnemyCount") != null)
        {
            enemyCount = GameObject.Find("EnemyCount").GetComponent<EnemyCount>();
            enemyCount.int_EnemyCount--;
            enemyCount.ChangeTextInt(enemyCount.int_EnemyCount);
        } 
    }
}
