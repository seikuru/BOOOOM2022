using System.Linq;
using UnityEngine;

public class ReverseCollider : MonoBehaviour
{
    /// メッシュの法線を反転させて内側にコリジョンを持つMeshColliderを作成するクラス
    /// 通常は外側で当たり判定が発生するが、反転することで内側で当たり判定を取れるようになる
    /// 
    [SerializeField] bool StartReverseCollider = false;
    public bool removeExistingColliders = true;

    /// <summary>
    /// 初期化処理
    /// StartReverseColliderがtrueの場合、開始時に反転コリジョンを作成
    /// </summary>
    private void Start()
    {
        if(StartReverseCollider)
            CreateInvertedMeshCollider();
    }

    /// <summary>
    /// 反転したMeshColliderを作成
    /// 既存のコリジョン削除→メッシュ反転→新規MeshCollider追加の順で処理
    /// </summary>
    public void CreateInvertedMeshCollider()
    {
        if (removeExistingColliders)
            RemoveExistingColliders();

        InvertMesh();

        gameObject.AddComponent<MeshCollider>();
    }

    /// <summary>
    /// 既存のコライダーを全て削除
    /// </summary>
    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
    }

    /// <summary>
    /// メッシュの三角形頂点の順序を反転
    /// 頂点順序を逆にすることで法線が反転し、内側が表面になる
    /// </summary>
    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // 三角形の頂点順序を逆順にして法線を反転
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}
