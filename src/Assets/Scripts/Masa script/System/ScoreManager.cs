using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;

    static int ScoreValue;

    // Start is called before the first frame update
    void Start()
    {
        ScoreValue = 0;
        ScoreText?.SetText(ScoreValue.ToString());
    }

    public void AddScore(int _add)
    {
        ScoreValue += _add;
        ScoreText?.SetText(ScoreValue.ToString());
    }

    public void AddScoreDestroy()
    {
        AddScore(500);
    }

    public void AddScoreBonus(int _time)
    {
        AddScore(_time / 10 + 50);
    }

    public static int GetScore() => ScoreValue;
}
