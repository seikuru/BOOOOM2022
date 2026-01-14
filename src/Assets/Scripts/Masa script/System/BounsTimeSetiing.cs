using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BounsTimeSetiing : MonoBehaviour
{
    [SerializeField] GameObject BounsCoinPrehab;
    
    [SerializeField] Transform[] SpawnPoint;

    [SerializeField] float SpawnTimer = 5f;

    [SerializeField] Vector2 ForceRangeFlat = new(7f, 7f);
    [SerializeField] float Force_Y = 60f;

    [SerializeField] bool BounsSpawm_;
    public bool BounsSpawm() => BounsSpawm_ = true;

    float TimeCount;

    private void Start()
    {
        BounsSpawm_ = false;
        TimeCount = 0;
    }

    public void CoinSpawn()
    {
        foreach (var tf in SpawnPoint)
        {
            Vector3 spawnPos = tf.transform.position;

            GameObject instantiate = Instantiate(BounsCoinPrehab, spawnPos, Quaternion.identity);

            if (instantiate.TryGetComponent<Rigidbody>(out var rb))
            {
                Vector3 force = new()
                {
                    x = Random.Range(-ForceRangeFlat.x, ForceRangeFlat.x),
                    y = Force_Y,
                    z = Random.Range(-ForceRangeFlat.y, ForceRangeFlat.y)
                };

                rb.AddForce(force, ForceMode.Impulse);
            }            
        }
    }

    private void FixedUpdate()
    {
        if (!BounsSpawm_)
            return;

        TimeCount += Time.fixedDeltaTime;

        if (TimeCount >= SpawnTimer)
        {
            TimeCount = 0f;
            CoinSpawn();
        }
    }
}
