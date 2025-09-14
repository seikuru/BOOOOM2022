using UnityEngine;

public class HitTerrain : MonoBehaviour
{
    //弾などが地形に当たった際に消滅するようにするスクリプト

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Terrain")
        {
            Destroy(gameObject);
        }
    }
}
