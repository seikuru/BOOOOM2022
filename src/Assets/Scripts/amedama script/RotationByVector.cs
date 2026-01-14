using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationByVector : MonoBehaviour
{
    [SerializeField] Rigidbody PlayerRigidbody;
    [SerializeField] Transform PlayerModelTransform;
    [SerializeField] int rotationSpeed = 4;
    Vector3 Velocity;
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerRigidbody.velocity.x != 0 || PlayerRigidbody.velocity.z != 0)
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
