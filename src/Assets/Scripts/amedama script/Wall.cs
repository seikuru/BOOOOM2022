using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //敵が壁に当たった際に消えるスクリプトです。
    //爆弾で飛ばした破片を消すかどうかによってアタッチするかどうかを決定してください。

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {           
            Destroy(collision.gameObject);  
        }
    }
}
