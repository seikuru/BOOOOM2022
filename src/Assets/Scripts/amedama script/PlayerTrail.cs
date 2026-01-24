using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    [SerializeField] GameObject TrailObject;
    [SerializeField] float TrailOnSpeed = 10.0f;
    [SerializeField] float TrailOffSpeed = 1.0f;
    [SerializeField] float enableTime = 1.0f;
    Rigidbody rb;
    float Timer = 0.0f;
    bool TrailTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (rb.velocity.magnitude >= TrailOnSpeed)
        {
            TrailObject.SetActive(true);
            TrailTimer = false;
        }
        else if(!TrailTimer && rb.velocity.magnitude <= TrailOffSpeed)
        {
            Timer = 0.0f;
            TrailTimer = true;
        }

        if (TrailTimer)
        {
            Timer += Time.deltaTime;

            if (Timer > enableTime)
            {
                TrailObject.SetActive(false);
            }
            
        }
    }
}
