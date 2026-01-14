using UnityEngine;
using UnityEngine.VFX;

public class EnemyLinkLine : MonoBehaviour
{
    [SerializeField] VisualEffect EnemyLink;
    [SerializeField] Transform EffectTransform;
    [SerializeField] string orbit = "Position2_position";
    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {
        // ワールド方向ベクトル
        Vector3 worldDir = target.position - EffectTransform.position;

        // ワールド → ローカル変換
        Vector3 localDir = EffectTransform.InverseTransformDirection(worldDir);

        EnemyLink.SetVector3(orbit, localDir);
    }
}
