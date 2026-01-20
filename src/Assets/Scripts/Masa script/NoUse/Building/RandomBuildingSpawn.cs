using UnityEngine;

public class RandomBuildingSpawn : MonoBehaviour
{
    /// ランダムに建物のプレハブを生成するクラス

    [SerializeField] GameObject BuildingPrehab; // 建物のプレハブ

    [SerializeField] int InstantiateValue; // 建物を生成する数

    [SerializeField] Vector3 RandomMin; // ランダムな位置の最小値

    [SerializeField] Vector3 RandomMax; // ランダムな位置の最大値

    void Awake()
    {
        for (int i = 0; i < InstantiateValue; i++)
        {
            // 生成する位置をランダムで決定
            Vector3 InstantiatePos = new Vector3()
            {
                x = Random.Range(RandomMin.x, RandomMax.x),
                y = Random.Range(RandomMin.y, RandomMax.y),
                z = Random.Range(RandomMin.z, RandomMax.z)
            };

            // 建物を生成
            Instantiate(BuildingPrehab, InstantiatePos, Quaternion.identity);
        }
    }
}
