using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerNoize : MonoBehaviour
{
    [SerializeField] Transform AnchorTransform;

    [SerializeField] float FlashClamp = 0.8f;
    [Space]
    [SerializeField] float AddPowerFlat = 5f;
    [SerializeField] float AddPowerY = 12f;

    Vector3 StartPos;
    Vector3 StartScale;

    Vector3 FlatImpactVerocity;

    float timeCount;

    private void Start()
    {
        StartPos = transform.localPosition;
        StartScale = transform.localScale;

        FlatImpactVerocity = new Vector3()
        {
            x = transform.position.x - AnchorTransform.position.x,
            y = 0f,
            z = transform.position.z - AnchorTransform.position.z
        };

        FlatImpactVerocity.Normalize();
        timeCount = 0f;
    }

    float MapingClamp(float value, float min1, float max1, float min2, float max2)
    {
        return (value - min1) * (max2 - min2) / (max1 - min1) + min2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeCount += Time.fixedDeltaTime;

        float clamp01 = MapingClamp(timeCount % FlashClamp,0f, FlashClamp,0f,1f);

        transform.localPosition = StartPos * clamp01;

        transform.localScale = StartScale * clamp01;
    }

    private void OnTriggerEnter(Collider other)
    {
        var Object = other.gameObject;

        if (Object.CompareTag("Player"))
        {
            Rigidbody rb = Object.GetComponent<Rigidbody>();

            Vector3 flat = new()
            {
                x = AnchorTransform.position.x - StartPos.x,
                y = 0,
                z = AnchorTransform.position.z - StartPos.z
            };

            flat = flat.normalized;

            rb.AddForce((FlatImpactVerocity * AddPowerFlat) + (Vector3.up * AddPowerY),ForceMode.Impulse);
        }
    }
}
