using UnityEngine;


public class PlayerCamera : MonoBehaviour
{

    [SerializeField] float rotateSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 localAngle = transform.localEulerAngles;

        localAngle.x -= Input.GetAxis("Mouse Y") * rotateSpeed;

        transform.localEulerAngles = localAngle;

        Vector3 angle = transform.eulerAngles;
        angle.y += Input.GetAxis("Mouse X") * rotateSpeed;
        transform.eulerAngles = angle;
    }
}
