using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    /// 敵の爆発エフェクトを生成・管理するクラス
    /// 静的メソッドで任意の場所から爆発エフェクトを呼び出し可能
    
    [SerializeField] GameObject explodePrefab;

    [SerializeField] float createDistance = 4f;
    [SerializeField] float destroyTime = 5f;

    // 静的参照用の変数(他のクラスから呼び出せるようにするため)
    private static GameObject ExplodePrefab;
    private static float CreateDistance;
    private static float DestroyTime;

    /// <summary>
    /// 初期化処理
    /// インスペクタで設定した値を静的変数にコピー
    /// </summary>
    private void Awake()
    {
        ExplodePrefab = explodePrefab;
        CreateDistance = createDistance;
        DestroyTime = destroyTime;
    }

    /// <summary>
    /// 爆発エフェクトを生成
    /// ターゲットの少し手前に爆発方向を向いたエフェクトを配置
    /// </summary>
    /// <param name="BombTF">爆発の起点となるTransform</param>
    /// <param name="TargetTF">ターゲットのTransform</param>
    static public void CreateExplode(Transform BombTF, Transform TargetTF)
    {
        // 爆発方向を計算
        Vector3 dir = (TargetTF.position - BombTF.position).normalized;

        // ターゲット位置から少し離れた位置に生成
        Vector3 createPos = TargetTF.position + dir * CreateDistance;

        // 爆発方向を向いた回転を設定
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

        // エフェクトを生成
        GameObject create = Instantiate(ExplodePrefab, createPos, rot);

        // 指定時間後に自動削除
        Destroy(create, DestroyTime);
    }
}
