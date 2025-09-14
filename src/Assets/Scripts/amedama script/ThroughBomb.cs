using System.Collections;
using UnityEngine;

public class ThroughBomb : MonoBehaviour
{

    [SerializeField] float throughTime = 0.2f;
    [SerializeField] Collider BombCollider;

    WaitForSeconds _throughTime;//コライダーが機能するまでの時間


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _throughTime = new WaitForSeconds(throughTime);

        if (BombCollider != null)
        {
            StartCoroutine("ColliderON");
        }
        else
        {
            Debug.Log("Colliderがアタッチされていません。");
            return;
        }
    }

    IEnumerator ColliderON()//throughTime秒後にコライダーを機能させる処理
    {

        yield return _throughTime;

        BombCollider.enabled = true;

    }
}
