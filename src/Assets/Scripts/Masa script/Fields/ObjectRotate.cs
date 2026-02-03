using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    // オブジェクトを一定速度で回転させる制御クラス
    [SerializeField] Vector3 rotateSpeed = new(0, 0, 10);

    void Update()
    {
        // 指定された速度でオブジェクトを回転
        transform.Rotate(rotateSpeed * Time.deltaTime);
    }
}
