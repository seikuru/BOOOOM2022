using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minethrow : MonoBehaviour
{


    [SerializeField] GameObject mineObject;
    [SerializeField] float ThrowPower = 10.0f;
    [SerializeField] float ThrowAngle = 45.0f;
    [SerializeField] float ThrowInterval = 1.0f;
    [SerializeField] float DistanceTarget = 50.0f;
    [SerializeField] float DestroyTime = 10.0f;
    [SerializeField] float FlightTIme = 1.0f;

    GameObject player;
    WaitForSeconds waitForSeconds;
    Coroutine coroutine;
    Vector3 ProjectionVector = Vector3.zero;
    float Gravity = -9.8f;
    bool CanThrow = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        waitForSeconds = new WaitForSeconds(ThrowInterval);
        
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Vector3.Distance(player.transform.position, this.transform.position) <= DistanceTarget && CanThrow == true)
        {
            coroutine = StartCoroutine(ThrowMine());
            CanThrow = false;
        }
        else
        {

        }
    }

    IEnumerator ThrowMine()
    {
        yield return waitForSeconds;

        
        GameObject mine;
        Destroy(mine = Instantiate(mineObject, transform.position + ((player.transform.position - transform.position).normalized) * 5, Quaternion.identity), DestroyTime);
        ProjectionVector = CalculateVelocity(this.gameObject.transform.position, player.transform.position);
        Debug.Log(ProjectionVector);
        mine.GetComponent<Rigidbody>().AddForce(ProjectionVector,ForceMode.Impulse);
        
        CanThrow = true;
      
    }

    private Vector3 CalculateVelocity(Vector3 SpawnPoint, Vector3 PrayerPoint)
    {
        float rad = ThrowAngle * Mathf.PI / 180;
        float x = Vector2.Distance(new Vector2(SpawnPoint.x, SpawnPoint.z), new Vector2(PrayerPoint.x, PrayerPoint.z));
        float y = PrayerPoint.y - SpawnPoint.y;
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) - y)));

        if (float.IsNaN(speed))
        {
            return Vector3.zero;
        }
        else
        {
            return new Vector3(PrayerPoint.x - SpawnPoint.x, x * Mathf.Tan(rad), PrayerPoint.z - SpawnPoint.z).normalized * speed;
        }
    }

}
