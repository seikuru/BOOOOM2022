using UnityEngine;

public class randomenemy : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    [SerializeField] int enemycount = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < enemycount; i++)
        {
            float x = Random.Range(-490, 490);
            //float y = Random.Range(10, 990);
            float z = Random.Range(-490, 490);

            Vector3 spawnPoint = new Vector3(x, 1, z);

            Instantiate(enemy,spawnPoint,Quaternion.identity);
        }
    }
}
