using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDeleter : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Debug.Log("CollisionDeleter Collided with Player");
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // Debug.Log("CollisionDeleter Triggered with Player");
            this.gameObject.SetActive(false);
        }
    }
}
