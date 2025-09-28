using UnityEngine;

public class EnemyActBase : MonoBehaviour
{
    /*
    [Header("基底クラス")]
    [SerializeField] bool a;
    [Header("ここから派生クラス"),Space]
    */

    public virtual void Act_Start()
    {
        return;
    }


    public virtual void Act_FixedUpdate()
    {
        return;
    }
}
