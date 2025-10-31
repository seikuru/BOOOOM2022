using UnityEngine;

public class CameraDistanceModification : MonoBehaviour
{
    /// プレイヤーの上下移動に応じてカメラの距離を動的に調整するクラス
    /// プレイヤーが上昇するとカメラが後退し、下降すると元の位置に戻る

    [SerializeField] Rigidbody Player_rb; // プレイヤーのRigidbody（速度検知用）

    [SerializeField] Transform CameraTransform; // 調整対象のカメラTransform

    [SerializeField] float DistanceMax = 5f; // カメラ距離調整の最大値

    [SerializeField] float AddValue = 0.1f; // 毎フレームの距離調整増減値

    [SerializeField] float AddSpeedMagnification = 0.1f; // カメラ距離への影響倍率

    Vector3 StartCameraLocalPos; // カメラの初期ローカル座標
    float UpMoveCount; // 上昇移動の累積カウンター

    void Start()
    {
        StartCameraLocalPos = CameraTransform.localPosition; // 初期カメラ位置を保存
        UpMoveCount = 0; // 上昇カウンターを初期化
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // プレイヤーの垂直方向の速度を取得
        float velocity = Player_rb.velocity.y;

        // 速度に応じて上昇カウンターを増減
        if (velocity > 0)
        {
            UpMoveCount += AddValue; // 上昇時はカウンターを増加
        }
        else
        {
            UpMoveCount -= AddValue; // 下降・停止時はカウンターを減少
        }

        // 上昇カウンターを0～最大値の範囲に制限
        // カウンター値をクランプ
        float velocityClamp = Mathf.Clamp(UpMoveCount, 0, DistanceMax);

        // カメラ位置を調整（初期位置に倍率を適用）
        CameraTransform.localPosition = StartCameraLocalPos * (1 + velocityClamp * AddSpeedMagnification);
    }
}
