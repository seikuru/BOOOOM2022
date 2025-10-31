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
    void FixedUpdate()
    {
        
        // è„â∫ÇÃìÆÇ´
        var VecY = playerRigidbody.velocity.y;
        if (Mathf.Abs(VecY) < 0.1f) VecY = 0;
        PlayerAnimator.SetFloat("VectorY", VecY);

        // êÖïΩï˚å¸ÇÃà⁄ìÆó 
        var VertVec = playerRigidbody.velocity;
        VertVec.y = 0;
        var VertMov = VertVec.magnitude;
        if (VertMov < 0.1f) VertMov = 0;
        PlayerAnimator.SetFloat("VerticalMove", VertMov);
    }
        

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Terrain"Å@|| collision.transform.tag == "Floor")
        {
            PlayerAnimator.SetTrigger("OnGround");
            PlayerAnimator.SetBool("Ground", true);
        }        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Terrain" || collision.transform.tag == "Floor")
        {
            PlayerAnimator.ResetTrigger("OnGround");
            PlayerAnimator.SetBool("Ground", false);
        }
    }

    public void onThrow()
    {
        // Debug.Log("throw");
        PlayerAnimator.SetTrigger("OnThrow");
    }

    public void BombHit()
    {
        Debug.Log("Bomb Hit");
        PlayerAnimator.SetTrigger("BombHit");
    }
}
