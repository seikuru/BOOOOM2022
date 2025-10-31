using System.Collections;
using TMPro;
using UnityEngine;

public class playerhit : MonoBehaviour
{
    TextMeshProUGUI EnemyCountText;
    [SerializeField] Player player;
    [SerializeField] float InvincibleTime = 1.0f;//無敵時間

    bool Invincible = false;//現在無敵かどうか
    int TimeCount = 0;
    WaitForSeconds waitforSec;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
        if (GameObject.Find("HPCount") != null)
        {
            EnemyCountText = GameObject.Find("HPCount").GetComponent<TextMeshProUGUI>();
            EnemyCountText.text = player.PlayerHP.ToString();
        }
        
        waitforSec = new WaitForSeconds(InvincibleTime);//waitforSecondsのserialize
        
    }

    IEnumerator WaitLoop()
    {

        yield return waitforSec;//無敵時間の秒数だけ待つ

        if (Invincible)
        {
            Invincible = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == "enemy" || other.tag == "Attack") && Invincible == false)
        {
            player.PlayerHP--;
            EnemyCountText.text = player.PlayerHP.ToString();
            Invincible = true;
            StartCoroutine(WaitLoop());
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if ((collision.gameObject.tag == "enemy" || collision.gameObject.tag == "Attack") && Invincible == false)
        {

            player.PlayerHP--;
            //EnemyCountText.text = player.PlayerHP.ToString();
            Invincible = true;
            StartCoroutine(WaitLoop());

        }

    }
}
