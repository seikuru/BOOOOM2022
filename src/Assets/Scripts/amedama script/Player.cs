using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    static Transform PlayerTransform;

    private void Awake()
    {
        PlayerTransform = transform; 
    }

    public static Transform GetTransformPlayer => PlayerTransform;

    public int PlayerHP = 5;
}
