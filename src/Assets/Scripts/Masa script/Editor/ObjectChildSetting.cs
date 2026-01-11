using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MusicSymbolObject))]
public class ObjectChildSetting : Editor
{
    public override void OnInspectorGUI()
    {
        // 通常のInspectorを描画
        DrawDefaultInspector();

        MusicSymbolObject container = (MusicSymbolObject)target;

        EditorGUILayout.Space();

        // ボタン
        if (GUILayout.Button("子オブジェクトをEnemylistに登録（直下のみ）"))
        {
            RegisterChildren(container);
        }
    }

    private void RegisterChildren(MusicSymbolObject container)
    {
        // Undo対応（エディター拡張では重要）
        Undo.RecordObject(container, "Register Enemy Children");

        // リストが null の場合は初期化
        List<GameObject> enemylist = new List<GameObject>();
        
        Transform parent = container.transform;

        // 直下の子のみ取得（孫は含まれない）
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            enemylist.Add(child.gameObject);
        }

        container.SetList(enemylist);

        // 変更を保存
        EditorUtility.SetDirty(container);

        Debug.Log($"Enemylist に {enemylist.Count} 個の子オブジェクトを登録しました");
    }
}
