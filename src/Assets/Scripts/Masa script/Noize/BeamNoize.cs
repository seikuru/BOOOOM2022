using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamNoize : MonoBehaviour
{
    [SerializeField] Rigidbody Beam_rb;
    [SerializeField] float BeamSpeed = 5f;
    [SerializeField] float ScaleMaxTime = 3f;
    [SerializeField] Vector3 MaxScale = new(5, 5, 1);
    [Space]
    [SerializeField] float AddPowerFlat = 12f;
    [SerializeField] float AddPowerY = 12f;

    float timeCount;
    Vector3 StartScale;

    private void Start()
    {
        Beam_rb.velocity = transform.forward * BeamSpeed;
        timeCount = 0;
        StartScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        timeCount += Time.fixedDeltaTime;

        float clamp01 = Mathf.Clamp01(timeCount / ScaleMaxTime);

        transform.localScale = StartScale + (MaxScale - StartScale) * clamp01;
    }

    private void OnTriggerEnter(Collider other)
    {
        var Object = other.gameObject;

        if (Object.CompareTag("Player"))
        {
            Rigidbody rb = Object.GetComponent<Rigidbody>();

            Vector3 flat = new()
            {
                x = Beam_rb.velocity.x,
                y = 0,
                z = Beam_rb.velocity.z
            };

            flat = flat.normalized;

            rb.velocity = (flat * AddPowerFlat) + (Vector3.up * AddPowerY);
        }

        Destroy(gameObject);
    }
}
