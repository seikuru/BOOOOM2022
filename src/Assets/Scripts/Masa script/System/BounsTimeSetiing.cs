using UnityEngine;

public class BounsTimeSetiing : MonoBehaviour
{
    /// ボーナスコインの定期スポーン管理クラス
    /// 指定した複数のスポーン地点からランダムな方向にコインを射出

    [SerializeField] GameObject BounsCoinPrehab;
    
    [SerializeField] Transform[] SpawnPoint;

    [SerializeField] float SpawnTimer = 5f;

    [SerializeField] Vector2 ForceRangeFlat = new(7f, 7f);
    [SerializeField] float Force_Y = 60f;

    // ボーナススポーンの有効フラグ
    [SerializeField] bool BounsSpawm_;

    // 最初のスポーン
    private bool firstSpawn = false;
    [SerializeField] private int firstSpawnCount = 10;

    /// <summary>
    /// ボーナススポーンを開始
    /// </summary>
    public bool BounsSpawm() => BounsSpawm_ = true;

    // スポーン間隔のカウンター
    float TimeCount;

    private void Start()
    {
        BounsSpawm_ = false;
        TimeCount = 0;
    }

    /// <summary>
    /// 全てのスポーン地点からコインを生成
    /// ランダムな方向と上向きの力を加えて射出
    /// </summary>
    public void CoinSpawn()
    {
        BounsSpawm_ = true;

        // 各スポーン地点でコイン生成
        foreach (var tf in SpawnPoint)
        {
            Vector3 spawnPos = tf.transform.position;

            GameObject instantiate = Instantiate(BounsCoinPrehab, spawnPos, Quaternion.identity);

            // Rigidbodyがあれば力を加える
            if (instantiate.TryGetComponent<Rigidbody>(out var rb))
            {
                // ランダムなXZ方向と固定のY方向の力を設定
                Vector3 force = new()
                {
                    x = Random.Range(-ForceRangeFlat.x, ForceRangeFlat.x),
                    y = Force_Y,
                    z = Random.Range(-ForceRangeFlat.y, ForceRangeFlat.y)
                };

                // 瞬間的な力を加えて射出
                rb.AddForce(force, ForceMode.Impulse);
            }            
        }
    }

    private void FirstSpawn()
    {
        foreach (var tf in SpawnPoint)
        {
            Vector3 spawnPos = tf.transform.position;

            for (int i = 0; i < firstSpawnCount; i++)
            {
                GameObject instantiate = Instantiate(BounsCoinPrehab, spawnPos, Quaternion.identity);

                // Rigidbodyがあれば力を加える
                if (instantiate.TryGetComponent<Rigidbody>(out var rb))
                {
                    // ランダムなXZ方向と固定のY方向の力を設定
                    Vector3 force = new()
                    {
                        x = Random.Range(-ForceRangeFlat.x, ForceRangeFlat.x),
                        y = Force_Y * 0.5f,
                        z = Random.Range(-ForceRangeFlat.y, ForceRangeFlat.y)
                    };

                    // 瞬間的な力を加えて射出
                    rb.AddForce(force, ForceMode.Impulse);
                }
            }
        }
    }

    /// <summary>
    /// 定期的なコインスポーン処理
    /// 指定時間経過ごとにCoinSpawnを呼び出し
    /// </summary>
    private void FixedUpdate()
    {
        // ボーナススポーンが無効なら処理しない
        if (!BounsSpawm_)
            return;

        if(firstSpawn == false)
        {
            FirstSpawn();
            firstSpawn = true;
        }

        // 経過時間を加算
        TimeCount += Time.fixedDeltaTime;

        // スポーン間隔に達したら実行
        if (TimeCount >= SpawnTimer)
        {
            TimeCount = 0f;
            CoinSpawn();
        }
    }
}
