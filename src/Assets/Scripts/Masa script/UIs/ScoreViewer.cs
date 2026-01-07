using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreViewer : MonoBehaviour
{
    [SerializeField] MusicType musicType;
    [SerializeField] TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        ScoreUpdate();
    }

    public void ScoreUpdate()
    {
        int current = 0,max = 1;
        ScoreManager.TakeMusicValue(musicType, ref current, ref  max);
        scoreText.SetText(current.ToString() + "/" + max.ToString());
    }
}
