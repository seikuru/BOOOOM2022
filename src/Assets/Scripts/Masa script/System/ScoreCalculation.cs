using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCalculation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Total;
    
    void Start()
    {
        Total.SetText(ScoreManager.GetScore().ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
