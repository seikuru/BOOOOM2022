using Unity.VisualScripting;
using UnityEngine;

public class mineexplode : MonoBehaviour
{

    [SerializeField] float BombStrange = 10.0f;//爆発した時の強さ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{ 

    //}

    private void OnTriggerEnter(Collider other)//プレイヤーが触れたときに起爆
    {

        if (other.tag == "Player")
        {
            Rigidbody PlayerRigidbody;
            PlayerRigidbody = other.GetComponent<Rigidbody>();
            PlayerRigidbody.velocity = Vector3.up * BombStrange;
            Destroy(gameObject);
        }
    }
}
