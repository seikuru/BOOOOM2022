using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class CoinDeleter : MonoBehaviour
{
    [SerializeField] private VisualEffect coinEffect;
    [SerializeField] private float delayDeleteTime = 3f;
    [SerializeField] private Collider coinCollider;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform coinTransform;
    [SerializeField] private float ScaleMinusTime = 1f;
    bool isTake = false;

    public void TakeCoin()
    {
        if(isTake)  
            return; 
        isTake = true;

        coinCollider.enabled = false;
        rigidBody.isKinematic = true;

        //ScoreManager.instance?.AddScoreBonusCoin(CountDownTimer.BonusTimeValue);
        ScoreManager.instance.AddScoreCoin();

        coinEffect.SendEvent("OnHit");

        StartCoroutine(ScaleMinus());

        Destroy(gameObject, delayDeleteTime);
    }
    
    IEnumerator ScaleMinus()
    {
        Vector3 local = coinTransform.localScale;
        float count = 0f;
        while (count < ScaleMinusTime)
        {
            count += Time.deltaTime;

            float clamp = Mathf.Clamp01(1 - count / ScaleMinusTime);

            coinTransform.localScale = local * clamp;

            yield return null;
        }

        coinTransform.localScale = Vector3.zero;
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
