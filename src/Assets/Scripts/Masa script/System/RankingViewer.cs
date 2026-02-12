using TMPro;
using UnityEngine;

public class RankingViewer : MonoBehaviour
{
    [SerializeField]
    string sceneName = "ichi_yama";

    [SerializeField]
    bool GetCurrentMode = false;

    [SerializeField]
    TextMeshProUGUI[] scoreTexts;

    static readonly int RankingValue = 5;

    public void SetRanking(int currentValue = -1)
    {
        if (scoreTexts == null || scoreTexts.Length != RankingValue)
            return;

        int[] score = GetCurrentMode ?
            ScoreRanking.GetRankingDataBeforeMode() :
            ScoreRanking.GetRankingData(sceneName);

        if (score == null || score.Length != RankingValue)
            return;

        for (int i = 0; i < RankingValue; i++)
            scoreTexts[i].SetText(score[i].ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        SetRanking();
    }
}
