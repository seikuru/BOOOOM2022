using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minegenerate : MonoBehaviour
{

    [SerializeField] GameObject mineObject;
    [SerializeField] float DestroyTime = 10.0f;

    Rigidbody Rigidbody;

    private float _GravityMultiply = 0;

    public void setParamator(float gravity)
    {
        _GravityMultiply = gravity;
    }

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody.AddForce((_GravityMultiply - 1) * Physics.gravity, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain")
        {

            Vector3 minePosition = this.transform.position + Vector3.up;

            Destroy(this.gameObject);
            Destroy(Instantiate(mineObject, minePosition, Quaternion.identity), DestroyTime);
        }
    }
}