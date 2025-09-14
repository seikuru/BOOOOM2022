using System.Collections;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    [SerializeField] GameObject On;
    [SerializeField] GameObject Off;
    [SerializeField] float Interval = 2f;//オンとオフを何秒で切り替えるのか

    bool flag = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(onoff());
    }

    // Update is called once per frame

    IEnumerator onoff()
    {

        while (true)
        {
            if (flag)
            {

                On.SetActive(true);
                Off.SetActive(false);

            }
            else
            {
                On.SetActive(false);
                Off.SetActive(true);
            }

            flag = !flag;

            yield return new WaitForSeconds(Interval);

        }
    }
}
