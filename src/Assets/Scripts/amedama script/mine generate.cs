using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minegenerate : MonoBehaviour
{

    [SerializeField]GameObject mineObject;
    [SerializeField] float DestroyTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Terrain")
        {
            Vector3 minePosition = new Vector3(this.transform.position.x, +1.0f, this.transform.position.z);

            Destroy(this.gameObject);
            Destroy(Instantiate(mineObject,minePosition,Quaternion.identity),DestroyTime);
        }
    }
}
