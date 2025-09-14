using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField]Animator PlayerAnimator;
    [SerializeField]Rigidbody playerRigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerAnimator.SetInteger("PlayerState", 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (playerRigidbody.velocity.y <= -10)
        {
            PlayerAnimator.SetInteger("PlayerState", 4);
            
        }
        PlayerAnimator.SetFloat("VectorY", playerRigidbody.velocity.y);
    }
        

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Terrain")
        {

            PlayerAnimator.SetTrigger("OnGround");
            PlayerAnimator.SetInteger("PlayerState", 1);

        }        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Terrain")
        {

            PlayerAnimator.ResetTrigger("OnGround");
            PlayerAnimator.SetInteger("PlayerState", 2);
        }
    }
}
