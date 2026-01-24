#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MeshEditor : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Mesh/Combine Selected Meshes")]
    static void CombineSelectedMeshes()
    {
        GameObject[] objs = Selection.gameObjects;
        if (objs.Length == 0) return;

        CombineInstance[] combine = new CombineInstance[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            MeshFilter mf = objs[i].GetComponent<MeshFilter>();
            combine[i].mesh = mf.sharedMesh;
            combine[i].transform = mf.transform.localToWorldMatrix;
        }

        Mesh combined = new Mesh();
        combined.CombineMeshes(combine);

        GameObject newObj = new GameObject("CombinedMesh");
        newObj.AddComponent<MeshFilter>().sharedMesh = combined;
        newObj.AddComponent<MeshRenderer>().sharedMaterial = objs[0].GetComponent<MeshRenderer>().sharedMaterial;

        Debug.Log("選択したメッシュを結合しました！");
    }
#endif
}
