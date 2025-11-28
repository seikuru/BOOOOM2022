using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReverseCollider : MonoBehaviour
{
    [SerializeField] bool StartReverseCollider = false;
    public bool removeExistingColliders = true;

    private void Start()
    {
        if(StartReverseCollider)
            CreateInvertedMeshCollider();
    }

    public void CreateInvertedMeshCollider()
    {
        if (removeExistingColliders)
            RemoveExistingColliders();

        InvertMesh();

        gameObject.AddComponent<MeshCollider>();
    }

    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
    }

    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}
