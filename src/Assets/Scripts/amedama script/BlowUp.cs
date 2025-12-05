using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUp : MonoBehaviour
{
    //private GameObject Player;
    private Rigidbody PlayerRB;
    [SerializeField] private float DetectDistance = 10.0f;
    bool Detected = false;
    float DetectTimer = 0;
    [SerializeField] float BlowUpTime = 7.0f;
    [SerializeField] float BlowUpStrange = 10.0f;
    Vector3 BlowUpVector = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
        PlayerRB = Player.GetTransformPlayer.GetComponent<Rigidbody>();
        BlowUpVector = Vector3.up * BlowUpStrange;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.GetTransformPlayer.position, this.transform.position) <= DetectDistance)
        {
            Detected = true;
        }
        else
        {
            Detected = false;
            DetectTimer = 0;
        }

        if (Detected)
        {
            DetectTimer += Time.deltaTime;
        }

        if (DetectTimer >= BlowUpTime)
        {
            PlayerRB.AddForce(BlowUpVector, ForceMode.Impulse);
            Detected = false;
            DetectTimer = 0;
        }
    }
}
