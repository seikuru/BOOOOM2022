using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCompass : MonoBehaviour
{

    [SerializeField] GameObject Boss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Boss.transform.position);
    }
}
