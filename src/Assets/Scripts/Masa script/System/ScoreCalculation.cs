using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCalculation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Bouns;
    [SerializeField] TextMeshProUGUI Total;
    [SerializeField] BGMControll controll;

    MusicType[] types = { MusicType.Drums, MusicType.Bass, MusicType.Melody, MusicType.Chord };

    void Start()
    {
        int bonus = ScoreManager.GetBonus();
        int score = ScoreManager.GetScore();

        Bouns.SetText(bonus.ToString());
        Total.SetText((bonus + score).ToString());

        StartCoroutine(BGMEnable());
    }

    IEnumerator BGMEnable()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var type in types)
        {
            int getValue = 0, MaxValue = 1;
            ScoreManager.TakeMusicValue(type, ref getValue, ref MaxValue);

            while (getValue > 0) 
            { 
                getValue--;
                controll.ResultBGMPlay(type);
            }
        }
    }
}
