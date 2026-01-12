using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCalculation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Bouns;
    [SerializeField] TextMeshProUGUI Total;
    
    void Start()
    {
        int bonus = ScoreManager.GetBonus();
        int score = ScoreManager.GetScore();

        Bouns.SetText(bonus.ToString());
        Total.SetText((bonus + score).ToString());
    }
}
