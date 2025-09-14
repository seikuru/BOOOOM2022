using UnityEngine;

public class ObstacleExplosion : MonoBehaviour
{
    /// 障害物が破壊されたときの演出を行うクラス

    [SerializeField] GameObject DustPrehab; // 爆発した時の破片プレハブ

    [SerializeField] float DestroyTime = 7f; // プレハブをDestroyするための時間

    [SerializeField] float DustDivisionSize = 3f; // 破片を生成する間隔（分割サイズ）

    bool IsExplosed = false;// 爆発済みかどうかのフラグ

    /// <summary>
    /// オブジェクトのスケールの半分の値を取得
    /// 爆発範囲の計算に使用
    /// </summary>
    Vector3Int ObjectScale => new Vector3Int()
    {
        x = (int)Mathf.Abs(this.transform.localScale.x / 2),
        y = (int)Mathf.Abs(this.transform.localScale.y / 2),
        z = (int)Mathf.Abs(this.transform.localScale.z / 2),
    };

    /// <summary>
    /// 爆発処理のメイン関数
    /// 指定された位置と力で破片を生成・飛散させる
    /// </summary>
    /// <param name="pos">爆発の中心位置</param>
    /// <param name="power">爆発の力</param>
    public void Explosion(Vector3 pos, float power)
    {
        // 重複実行防止
        if (IsExplosed) return;
        IsExplosed = true;

        // 破片生成範囲の計算
        Vector3 StartPosVec3 = transform.position - ObjectScale; // 生成開始位置
        Vector3 EndPosVec3 = transform.position + ObjectScale; // 生成終了位置

        // 3重ループで破片を格子状に生成
        for (float x = StartPosVec3.x; x < EndPosVec3.x; x += DustDivisionSize)
            for (float y = StartPosVec3.y; y < EndPosVec3.y; y += DustDivisionSize)
                for (float z = StartPosVec3.z; z < EndPosVec3.z; z += DustDivisionSize)
                {
                    // 破片の生成位置を計算
                    Vector3 createPos = new Vector3(x, y, z);

                    // 破片オブジェクトを生成
                    GameObject dust = Instantiate(DustPrehab, createPos, Quaternion.identity);

                    // 破片サイズを変更
                    dust.transform.localScale = Vector3.one * DustDivisionSize; 

                    dust.SetActive(true);

                    // 物理演算で爆発力を適用
                    Rigidbody rb = dust.GetComponent<Rigidbody>();
                    // 爆発中心から破片への方向ベクトルに力を加える
                    rb.AddForce((dust.transform.position - pos).normalized * power, ForceMode.Impulse);

                    // 一定時間後に破片を削除
                    Destroy(dust, DestroyTime);
                }

        // 元のオブジェクトを削除
        Destroy(this.gameObject, DestroyTime);
        this.gameObject.SetActive(false);
    }
}
