using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscRotate : MonoBehaviour
{
    [SerializeField]
    ControllerUIInput controllerUIInput;

    [SerializeField] float rotateSpeed = 5f;
        　

    void DiscRotateUpdate()
    {
        // 角度を取得
        float Angle = controllerUIInput.GetAngle() + transform.eulerAngles.z;

        // 負の角度を正の角度に変換
        if (Angle < 0)
            Angle += 360f;

        Angle += rotateSpeed * Time.deltaTime;

        // ディスクのZ軸回転を設定（画面上での回転表現）
        transform.eulerAngles = new(0f, 0f, Angle);
    }

    // Update is called once per frame
    void Update()
    {
        DiscRotateUpdate();
    }
}
