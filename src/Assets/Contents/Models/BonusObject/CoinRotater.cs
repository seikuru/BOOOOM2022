using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotater : MonoBehaviour
{
    [SerializeField] private GameObject Coin;
    [SerializeField] private float RotationSpeed = 1f;

    void Start()
    {
        if(Coin == null)
        {
            Debug.LogError("Coinオブジェクトがアタッチされていません。");
        }
    }

    void Update()
    {
        Coin.transform.Rotate(0, RotationSpeed, 0);
    }
}
