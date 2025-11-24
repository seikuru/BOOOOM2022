using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    static Transform PlayerTransform;

    private void Awake()
    {
        PlayerTransform = transform;
        PlayerBombHit = false;
    }

    public static Transform GetTransformPlayer => PlayerTransform;

    public int PlayerHP = 5;

    public bool PlayerBombHit = false;

    public bool OnGround = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Terrain") || collision.transform.CompareTag("Floor"))
        {
            OnGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Terrain") || collision.transform.CompareTag("Floor"))
        {
            OnGround = false;
        }
    }
}
