using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationByVector : MonoBehaviour
{
    [SerializeField] Rigidbody PlayerRigidbody;
    [SerializeField] Transform PlayerModelTransform;
    [SerializeField] int rotationSpeed = 4;
    [SerializeField] float StopForThrowing = 1.0f;
    Vector3 Velocity;

    public bool Throwing = false;
    float StopCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Throwing)
        {
            StopCount += Time.deltaTime;
            if(StopCount >= StopForThrowing)
            {
                Throwing = false;
                StopCount = 0.0f;
            }
        }
        else if (PlayerRigidbody.velocity.x != 0 || PlayerRigidbody.velocity.z != 0)
        {
            Velocity = new Vector3(PlayerRigidbody.velocity.x, 0, PlayerRigidbody.velocity.z).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(Velocity);

            PlayerModelTransform.rotation = Quaternion.Slerp(
                PlayerModelTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
                );
        }
    }
}
