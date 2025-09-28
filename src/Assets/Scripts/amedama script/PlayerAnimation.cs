using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField]Animator PlayerAnimator;
    [SerializeField]Rigidbody playerRigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(playerRigidbody.velocity.y);

        if (playerRigidbody.velocity.y < -10)
        {
            PlayerAnimator.SetTrigger("Falling");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Terrain")
        {

            PlayerAnimator.SetTrigger("OnGround");

        }        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Terrain")
        {

            PlayerAnimator.ResetTrigger("OnGround");
            PlayerAnimator.SetTrigger("BombHit");
            
        }
    }
}
