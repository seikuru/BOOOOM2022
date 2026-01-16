using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscRotate : MonoBehaviour
{
    [SerializeField]
    ControllerUIInput controllerUIInput;

    [SerializeField] float rotateSpeed = 5f;

    [SerializeField] Vector3 RotateOffset = Vector3.forward;

    float before = 0;

    void DiscRotateUpdate()
    {
        // 角度を取得
        float angle = controllerUIInput.GetAngle() * -2;
       
        float resultRotate = (angle + rotateSpeed) * Time.deltaTime;

        // 負の角度を正の角度に変換
        //if (resultRotate < 0)
        //    resultRotate += 360f;


        Vector3 ResultAngle = new() { 
            x = (transform.eulerAngles.x + resultRotate) * RotateOffset.x,
            y = (transform.eulerAngles.y + resultRotate) * RotateOffset.y,
            z = (transform.eulerAngles.z + resultRotate) * RotateOffset.z
        };

        // ディスクからの回転を設定（回転表現）
        transform.eulerAngles = ResultAngle;

        before = angle;
    }

    // Update is called once per frame
    void Update()
    {
        DiscRotateUpdate();
    }
}
