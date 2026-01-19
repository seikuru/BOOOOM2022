using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] SerialHandler serialHandler;
    [SerializeField] ValueContainer vCon;
    [SerializeField] private int TouchOffset = 0;

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
        if (message.Length >= 31 && message[0] == 'S'/* && message[14] == 'E'*/) // "S rot(3) bomb(1) center(1) esc(1) INdeg1(4) INdeg2(4) INdeg3(4) OUTdeg1(4) OUTdeg2(4) OUTdeg(4) E \n" -> 32
        {
            // Debug.Log(message);
            string recData;
            int t;

            // rot
            recData = message.Substring(1, 3);
            int.TryParse(recData, out t);
            vCon.rot += t;

            // bomb
            recData = message.Substring(4, 1);
            int.TryParse(recData, out t);
            vCon.button = t;

            // center
            recData = message.Substring(5, 1);
            int.TryParse(recData, out t);
            vCon.center = t;

            // esc
            recData = message.Substring(6, 1);
            int.TryParse(recData, out t);
            vCon.esc = t;

            // in rad
            int inradHead = 7;
            for(int i = 0; i < 3; i++)
            {
                recData = message.Substring(inradHead + i * 4, 4);
                int.TryParse(recData, out t);
                vCon.in_rad[i] = t + TouchOffset;
            }

            int outradHead = 19;
            for(int i = 0; i < 3; i++)
            {
                recData = message.Substring(outradHead + i * 4, 4);
                int.TryParse(recData, out t);
                vCon.out_rad[i] = t + TouchOffset;
            }
        }
    }
}
