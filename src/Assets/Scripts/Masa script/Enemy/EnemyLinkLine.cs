using UnityEngine;
using UnityEngine.VFX;

public class EnemyLinkLine : MonoBehaviour
{
    /// Visual Effect Graphで敵同士を繋ぐラインエフェクトを制御するクラス
    /// ターゲットへの方向をローカル座標系で計算しVFXに渡す
     
    [SerializeField] VisualEffect EnemyLink;
    [SerializeField] Transform EffectTransform;
    [SerializeField] string orbit = "Position2_position"; // VFXのパラメータを指定
    [SerializeField] Transform target;

    void FixedUpdate()
    {
        // ワールド方向ベクトル
        Vector3 worldDir = target.position - EffectTransform.position;

        // ワールド → ローカル変換(エフェクトの向きに合わせた相対座標に変換)
        Vector3 localDir = EffectTransform.InverseTransformDirection(worldDir);

        // VFXのパラメータに設定
        EnemyLink.SetVector3(orbit, localDir);
    }
}
