using UnityEngine;

public class DiscRotate : MonoBehaviour
{
    /// コントローラー入力に基づいてオブジェクトを回転させるクラス
    /// UIコントローラーからの角度入力を受け取り、指定軸で回転を適用

    [SerializeField] ControllerUIInput controllerUIInput;

    [SerializeField] float rotateSpeed = 5f;

    [SerializeField] Vector3 RotateOffset = Vector3.forward;

    // float before = 0;

    /// <summary>
    /// ディスクの回転処理
    /// コントローラーの角度入力を取得し、RotateOffsetで指定された軸に回転を適用
    /// </summary>
    void DiscRotateUpdate()
    {
        // 角度を取得
        float angle = controllerUIInput.GetAngle() * -2;

        // 回転量を計算
        float resultRotate = (angle + rotateSpeed) * Time.deltaTime;

        // 負の角度を正の角度に変換
        //if (resultRotate < 0)
        //    resultRotate += 360f;

        // RotateOffsetに基づいて各軸の回転を計算
        // OffsetがVector3.forwardの場合、Z軸のみ回転
        Vector3 ResultAngle = new() { 
            x = (transform.eulerAngles.x + resultRotate) * RotateOffset.x,
            y = (transform.eulerAngles.y + resultRotate) * RotateOffset.y,
            z = (transform.eulerAngles.z + resultRotate) * RotateOffset.z
        };

        // ディスクからの回転を設定（回転表現）
        transform.eulerAngles = ResultAngle;

        // before = angle;
    }

    void Update()
    {
        DiscRotateUpdate();
    }
}
