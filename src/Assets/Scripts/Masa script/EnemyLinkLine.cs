using UnityEngine;
using UnityEngine.VFX;

public class EnemyLinkLine : MonoBehaviour
{
    [SerializeField] VisualEffect EnemyLink;
    [SerializeField] string orbit = "Position2_position";
    [SerializeField] Transform transform_;
    
    // Update is called once per frame
    void Update()
    {
        EnemyLink.SetVector3(orbit, transform_.position);
    }
}
