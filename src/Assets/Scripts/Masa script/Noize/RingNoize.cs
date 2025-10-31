using UnityEngine;

public class RingNoize : MonoBehaviour
{
    [SerializeField] float ScaleUpSpeed = 0.1f;

    [SerializeField] float ScaleUPMax = 10f;

    [SerializeField] float AddPowerFlat = 5f;

    [SerializeField] float AddPowerY = 12f;

    Vector3 StartPos;

    private void Start()
    {
        StartPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Min(ScaleUPMax , scale.x + Time.fixedDeltaTime * ScaleUpSpeed);
        transform.localScale = scale;
    }

    private void OnTriggerEnter(Collider other)
    {
        var Object = other.gameObject;

        if (Object.CompareTag("Player"))
        {
            Rigidbody rb = Object.GetComponent<Rigidbody>();

            Vector3 flat = new()
            {
                x = Object.transform.position.x - StartPos.x,
                y = 0,
                z = Object.transform.position.z - StartPos.z
            };

            flat = flat.normalized;
            
            rb.velocity = (flat * AddPowerFlat) + (Vector3.up * AddPowerY);
        }
    }
}
