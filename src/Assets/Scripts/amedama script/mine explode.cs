using Unity.VisualScripting;
using UnityEngine;

public class mineexplode : MonoBehaviour
{

    [SerializeField] float BombStrange = 10.0f;//爆発した時の強さ

    GameObject Player;
    Rigidbody PlayerRigidbody;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        if (Player != null)
        {
            PlayerRigidbody = Player.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerEnter(Collider other)//プレイヤーが触れたときに起爆
    {
        if (other.tag == "Player")
        {
            PlayerRigidbody.velocity = Vector3.up * BombStrange;
            Destroy(gameObject);
        }
    }
}
