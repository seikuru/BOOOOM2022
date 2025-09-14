using UnityEngine;

public class enemyMove : MonoBehaviour
{

    enum MoveType//動き方
    {
        tracking,//追従
        RepeatHorizontal//交互に揺れを繰り返す
    };

    [SerializeField] float MovementSpeed = 1.0f;
    [SerializeField] float MoveInterval = 1.0f;//特定の動き方を指定した際、その動きの間隔をどうするのか
    [SerializeField] MoveType moveType = MoveType.tracking;//動き方を指定する。    
    
    [HideInInspector] public bool willDestroy = false;
    //爆弾が起動してからDestroyが実行されるまでの数秒に処理が行われないようにするためのflag
    
    GameObject Player;
    Rigidbody rb;
    int Count = 0;
    bool invert = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody>();

        switch (moveType)
        {
            case MoveType.RepeatHorizontal:

                rb.AddForce(transform.right * MovementSpeed, ForceMode.Impulse);
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!willDestroy)
        {
            if (Count >= 50 * MoveInterval)//Count50回 = 1秒
            {
                switch (moveType)
                {

                    case MoveType.tracking:


                        transform.LookAt(Player.transform.position);
                        rb.AddForce(transform.forward * MovementSpeed, ForceMode.Impulse);

                        break;


                    case MoveType.RepeatHorizontal:

                        if (invert)
                        {
                            rb.linearVelocity = Vector3.zero;
                            rb.AddForce(transform.right * MovementSpeed, ForceMode.Impulse);
                        }
                        else
                        {
                            rb.linearVelocity = Vector3.zero;
                            rb.AddForce(-transform.right * MovementSpeed, ForceMode.Impulse);
                        }

                        invert = !invert;
                        break;

                }

                Count = 0;
            }
        }

        Count += 1;
    }
}
