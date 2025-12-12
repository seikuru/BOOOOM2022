using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] SerialHandler serialHandler;
    [SerializeField] ValueContainer vCon;

    void Start()
    {
        serialHandler.OnDataReceived += OnDataRecieved;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDataRecieved(string message)
    {
        if (message == null) return;
        if (message.Length >= 10 && message[0] == 'S' && message[14] == 'E') // "S rot(3) button(1) center(1) rad(4) rad(4) E" -> 15
        {
            // Debug.Log(message);
            string recData;
            int t;

            // rot
            recData = message.Substring(1, 3);
            int.TryParse(recData, out t);
            vCon.rot += t;

            // button
            recData = message.Substring(4, 1);
            int.TryParse(recData, out t);
            vCon.button = t;

            // center
            recData = message.Substring(5, 1);
            int.TryParse(recData, out t);
            vCon.center = t;

            // rad
            recData = message.Substring(6, 4);
            int.TryParse(recData, out t);
            vCon.t_rad[0] = t;

            recData = message.Substring(10, 4);
            int.TryParse(recData, out t);
            vCon.t_rad[1] = t;
        }
    }
}
