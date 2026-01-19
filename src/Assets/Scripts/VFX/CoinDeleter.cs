using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CoinDeleter : MonoBehaviour
{
    [SerializeField] private VisualEffect coinEffect;
    [SerializeField] private float delayDeleteTime = 2;
    [SerializeField] private Collider coinCollider;
    [SerializeField] private Rigidbody rigidBody;
    bool isTake = false;

    public void TakeCoin()
    {
        if(isTake)  
            return; 
        isTake = true;

        coinCollider.enabled = false;
        rigidBody.isKinematic = true;

        ScoreManager.instance.AddScoreBonusCoin(CountDownTimer.BonusTimeValue);

        coinEffect.SendEvent("OnHit");

        Destroy(gameObject, delayDeleteTime);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Terrain")
        {
            coinEffect.SendEvent("OnHit");

            CoinCounter.AddCount();

            Destroy(gameObject, delayDeleteTime);
        }
    }
    */
}
