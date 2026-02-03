using System.Linq;
using UnityEngine;

public class EnemySpawnOverCover : MonoBehaviour
{
    /// 敵の自動スポーンシステム管理クラス
    /// プレイヤーから最も近いスポーン地点を除いた場所に定期的に敵を生成する

    [SerializeField] Transform PlayerTransform; // プレイヤーTransform（距離計算用）

    [SerializeField] GameObject EnemyPrehab; // 生成する敵のプレハブ

    [SerializeField] Transform[] SpawnTransform; // スポーン地点の配列

    [SerializeField] EnemyInstanceCounter enemyInstanceCounter; // 敵カウンター管理クラス

    [SerializeField] float SpawnIntarval = 2f; // スポーン間隔（秒）

    float TimeCount = 0;// スポーン用の時間カウンター

    /// <summary>
    /// 2点間の距離の二乗を計算
    /// 平方根計算を省略して処理速度を向上させる
    /// </summary>
    /// <param name="a">地点A</param>
    /// <param name="b">地点B</param>
    /// <returns>距離の二乗値</returns>
    float DistancePow(Vector3 a, Vector3 b)
    {
        float x = a.x - b.x;
        float y = a.y - b.y;
        float z = a.z - b.z;
        return x * x + y * y + z * z;// 距離の二乗を返す
    }

    /// <summary>
    /// プレイヤーから最も近いスポーン地点のインデックスを取得
    /// この地点は敵生成から除外される（プレイヤーの近くに敵が出現するのを防ぐ）
    /// </summary>
    /// <returns>最も近いスポーン地点のインデックス（-1は無効値）</returns>
    int GetNotSpawnPointIndex()
    {
        int index = -1; // 戻り値用のインデックス

        // スポーン地点が設定されていない場合は無効値を返す
        if (SpawnTransform.Length == 0)
            return index;

        float minDistance = float.MaxValue; // 最小距離の初期値

        // 全スポーン地点をチェックして最も近い地点を探す
        for (int i = 0; i < SpawnTransform.Count(); i++)
        {
            float distancePow = DistancePow(PlayerTransform.transform.position, SpawnTransform[i].position);
            if (distancePow < minDistance)
            {
                minDistance = distancePow; // 最小距離を更新
                index = i; // 最も近い地点のインデックスを記録
            }
        }

        return index;
    }

    void Start()
    {
        // 時間カウンターを初期化
        TimeCount = 0;

        // プレイヤーオブジェクトが未設定の場合はタグで検索
        if (PlayerTransform == null)
        {
            PlayerTransform = GameObject.FindWithTag("Player").transform;
        }
    }

    void FixedUpdate()
    {
        TimeCount += Time.deltaTime; // 時間カウンターを更新

        // スポーン間隔に達した場合の処理
        if (SpawnIntarval <= TimeCount)
        {
            TimeCount = 0; // 時間カウンターをリセット

            // プレイヤーに最も近いスポーン地点を取得（除外用）
            int notUseIndex = GetNotSpawnPointIndex();

            // 全スポーン地点をチェックして敵を生成
            for (int i = 0; i < SpawnTransform.Count(); i++)
            {
                // プレイヤーに最も近い地点はスキップ
                if (notUseIndex == i)
                {
                    continue;
                }

                // 敵オブジェクトを生成
                GameObject enemy = Instantiate(EnemyPrehab, SpawnTransform[i].transform.position, Quaternion.Euler(0f, 0f, 0f));

                // 敵カウンターに敵を登録
                enemyInstanceCounter.AddEnemyObject(enemy);
            }
        }
    }
}
