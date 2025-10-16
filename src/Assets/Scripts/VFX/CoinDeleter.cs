using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CoinDeleter : MonoBehaviour
{
    [SerializeField] private VisualEffect coinEffect;
    [SerializeField] private float delayDeleteTime = 2;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            coinEffect.SendEvent("OnHit");
            Destroy(this, delayDeleteTime);
        }
    }
}
