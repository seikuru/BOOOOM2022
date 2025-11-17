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
}
