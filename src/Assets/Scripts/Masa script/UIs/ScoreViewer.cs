using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour
{
    [SerializeField] MusicType musicType;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] RectMask2D rectMask2D;
    [SerializeField] RectTransform KonbRT;
    [SerializeField] float RightPaddingMax = 560;

    // Start is called before the first frame update
    void Start()
    {
        ScoreReflection();
    }

    public void ScoreReflection()
    {
        int current = 0,max = 1;
        ScoreManager.TakeMusicValue(musicType, ref current, ref  max);

        scoreText.SetText(current.ToString() + "/" + max.ToString());

        float clamp = Mathf.Clamp01((float)current / max);

        var vector4 = rectMask2D.padding;
        vector4.z = RightPaddingMax - RightPaddingMax * clamp;
        rectMask2D.padding = vector4;

        KonbRT.position = new(KonbRT.position.x + RightPaddingMax * clamp, KonbRT.position.y); 
    }
}
