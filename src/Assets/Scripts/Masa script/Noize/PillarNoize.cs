using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarNoize : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] float DownPowerSpeed = 0.1f;
    [SerializeField] float DownPowerMax = 10f;
    [Space]
    [SerializeField] float AddPowerFlat = 5f;
    [SerializeField] float AddPowerY = 12f;
    [Space]
    [SerializeField] float PillarDestroyTime = 15f;
    [SerializeField] GameObject PillarPrehab;

    Vector3 StartPos;

    private void Start()
    {
        StartPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, StartPos);
        float downPower = Mathf.Min(DownPowerMax, distance * DownPowerSpeed);
        rb.AddForce(Vector3.down * downPower);
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
        if (Object.CompareTag("Terrain") || Object.CompareTag("Floor"))
        {
            Vector3 SpwanPos = transform.position + Vector3.up * (PillarPrehab.transform.localScale.y / 2);

            // オブジェクト生成
            GameObject prehab = Instantiate(PillarPrehab, SpwanPos, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));

            Destroy(prehab , PillarDestroyTime);
            Destroy(gameObject);
        }
    }
}
