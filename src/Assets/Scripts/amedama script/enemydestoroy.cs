using TMPro;
using UnityEngine;

public class enemydestoroy : MonoBehaviour
{
    [SerializeField] float DestroyDelay = 0.0f;

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject, DestroyDelay);
    }
}
