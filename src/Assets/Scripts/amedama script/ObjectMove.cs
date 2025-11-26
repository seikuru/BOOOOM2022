using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{

    enum MoveType
    {
        Satellite
    }

    [SerializeField] MoveType moveType = MoveType.Satellite;
    [SerializeField] GameObject Center;
    [SerializeField] float MoveSpeed;
    //[SerializeField] float MoveRadius;

    float DistancefromCenter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        DistancefromCenter = Vector3.Distance(this.transform.localPosition, Center.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform tr = transform;
        // 回転のクォータニオン作成
        Quaternion angleAxis = Quaternion.AngleAxis(360 / MoveSpeed * Time.fixedDeltaTime, Vector3.up);

        // 円運動の位置計算
        Vector3 pos = tr.position;

        pos -= Center.transform.position;
        pos = angleAxis * pos;
        pos += Center.transform.position;

        tr.position = pos;

        // 向き更新
        //if (_updateRotation)
        //{
        //    tr.rotation = tr.rotation * angleAxis;
        //}
    }
}
