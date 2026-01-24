using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ReverseCollider))]
public class ReverseColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ReverseCollider script = (ReverseCollider)target;
        if (GUILayout.Button("Create Reverse Mesh Collider"))
            script.CreateInvertedMeshCollider();
    }
}
