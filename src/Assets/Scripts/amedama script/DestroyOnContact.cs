using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
