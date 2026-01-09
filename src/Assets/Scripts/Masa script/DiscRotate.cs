using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscRotate : MonoBehaviour
{
    [SerializeField]
    ControllerUIInput controllerUIInput;

    [SerializeField] float rotateSpeed = 5f;

    float before = 0;

    void DiscRotateUpdate()
    {
        // 角度を取得
        float angle = controllerUIInput.GetAngle() * -2;
       
        float resultAngle = (angle + rotateSpeed) * Time.deltaTime + transform.eulerAngles.z;

        // 負の角度を正の角度に変換
        if (resultAngle < 0)
            resultAngle += 360f;

        // ディスクのZ軸回転を設定（画面上での回転表現）
        transform.eulerAngles = new(0f, 0f, resultAngle);

        before = angle;
    }

    // Update is called once per frame
    void Update()
    {
        DiscRotateUpdate();
    }
}
