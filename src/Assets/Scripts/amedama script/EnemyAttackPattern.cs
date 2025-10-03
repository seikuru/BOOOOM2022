using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
class AttackPatternClass
{
    public EnemyActBase AttackPattern;
    public float AttackInterval;

}

public class EnemyAttackPattern : MonoBehaviour
{
    [Header("行動パターン")]
    [SerializeField] AttackPatternClass[] ShortRangeAttackPattern;
    [SerializeField] AttackPatternClass[] LongRangeAttackPattern;
    [SerializeField] AttackPatternClass[] enemyAttackPattern;

    [Header("行動パターン切り替え距離")]

    [SerializeField] float ChengeRangeDistance = 100f;

    [HideInInspector] public bool willDestroy = false;

    int AttackNumber = 0;
    int AttackIntervalCount = 0;
    AttackPatternClass NextAttackClass;
    /// <summary>
    /// AttackPatternClassからAttackIntervalを取得する
    /// </summary>
    /// <param name="AttackPattern">攻撃パターン</param>
    /// <returns>インターバル</returns>
    int GetInterval(AttackPatternClass Pattern)
    {
        return (int)(Pattern.AttackInterval / Time.fixedDeltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        //それぞれのスタート処理
        foreach (AttackPatternClass act in ShortRangeAttackPattern)
        {
            act.AttackPattern.Act_Start();
        }
        foreach (AttackPatternClass act in LongRangeAttackPattern)
        {
            act.AttackPattern.Act_Start();
        }

        //初回攻撃は遠距離攻撃にしておく
        NextAttackClass = LongRangeAttackPattern[0];

        //遠距離攻撃の最初のインターバルを設定
        AttackIntervalCount = GetInterval(NextAttackClass);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!willDestroy)
        {
            AttackIntervalCount--;
        }

        if (AttackIntervalCount < 0)
        {
            // 行動実行
            NextAttackClass.AttackPattern.Act_FixedUpdate();

            // プレイヤーとの距離取得
            float PlayerDistance = Vector3.Distance(Player.GetTransformPlayer.position, transform.position);

            // 扱う行動パターンを取得
            var CurrentPattern = ChengeRangeDistance > PlayerDistance 
                ? ShortRangeAttackPattern : LongRangeAttackPattern;

            //パターン進行
            if (AttackNumber >= CurrentPattern.Length - 1)
            { 
                AttackNumber = 0;
            }
            else
            {
                AttackNumber++;
            }

            //次回攻撃パターンの設定
            NextAttackClass = CurrentPattern[AttackNumber];

            //攻撃のインターバルを設定
            AttackIntervalCount = GetInterval(NextAttackClass);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // ギズモの色を赤に設定
        Gizmos.DrawWireSphere(transform.position, ChengeRangeDistance); // 球体を描画
    }
}
