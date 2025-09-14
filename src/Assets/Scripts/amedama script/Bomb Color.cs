using Unity.VisualScripting;
using UnityEngine;

public class BombColor : MonoBehaviour
{
    //このスクリプトはボム本体ではなく、その子供に当たり判定を追加し、接触した際に色を変更するスクリプトです。
    //そのため使用する際に本体にアタッチしないように気を付けてください。

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //Bombeffects bombeffects = GetComponentInParent<Bombeffects>();
        //this.transform.localScale = Vector3.one * bombeffects.BombRadius * 2.0f ;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Renderer[] renderers = gameObject.GetComponentsInParent<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = Color.red;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Renderer[] renderers = gameObject.GetComponentsInParent<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = Color.blue;
            }
        }
    }
}
