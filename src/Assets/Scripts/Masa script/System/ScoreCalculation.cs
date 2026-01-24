using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCalculation : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeSpeed = 0.1f;
    [SerializeField] TextMeshProUGUI Bouns;
    [SerializeField] TextMeshProUGUI Total;
    [SerializeField] SpriteRenderer BackSprite;
    [SerializeField] MackLine[] mackLines;
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] GameObject[] NoizeObjects;
    [SerializeField] GameObject[] KirakiraObjects;

    [Serializable]
    public class MackLine
    {
        public LineRenderer lineRenderer;
        public Color DefaltColor;
        public Color NoizeColor;

        public void SetColor(float clamp01)
        {
            Gradient g = new Gradient();

            Color LerpColor = Color.Lerp(NoizeColor,DefaltColor, clamp01);

            g.SetKeys(
                new GradientColorKey[] 
                {
                    new GradientColorKey(LerpColor, 0f),
                    new GradientColorKey(LerpColor, 1f)
                },
                new GradientAlphaKey[] 
                {
                    new GradientAlphaKey(LerpColor.a, 0f),
                    new GradientAlphaKey(LerpColor.a, 1f)
                }
            );

            lineRenderer.colorGradient = g;

            lineRenderer.startColor = LerpColor;
            lineRenderer.endColor = LerpColor;
        }
    }

    MusicType[] types = { MusicType.Drums, MusicType.Bass, MusicType.Melody, MusicType.Chord };

    void Start()
    {
        //fadeImage.gameObject.SetActive(true);
        //StartCoroutine(FedeColorChenge());

        int bonus = ScoreManager.GetBonus();
        int score = ScoreManager.GetScore();

        Bouns.SetText(bonus.ToString());
        Total.SetText((bonus + score).ToString());

        int allCurrent = 0, allmax = 0;
        int current = 0, max = 1;
        foreach (var type in types)
        {
            ScoreManager.TakeMusicValue(type, ref current, ref max);
            allCurrent += current;
            allmax += max;
        }

        float clamp = Mathf.Clamp01((float)allCurrent / allmax);
        if (clamp < 1)
        {
            PlayerAnimator.SetBool("GameClear", false);
            PlayerAnimator.SetBool("ResultOn", true);

            foreach (var noize in NoizeObjects)
            {
                noize.SetActive(true);
            }
        }
        else
        {
            PlayerAnimator.SetBool("GameClear", true);
            PlayerAnimator.SetBool("ResultOn", true);

            foreach (var kirakira in KirakiraObjects)
            {
                kirakira.SetActive(true);
            }
        }

            BackSprite.color = new Color()
            {
                r = 1f - (1f - clamp) / 2f,
                g = 1f - (1f - clamp) / 2f,
                b = 1f - (1f - clamp) / 2f,
                a = 1f
            };

        foreach (var line in mackLines)
        {
            line.SetColor(clamp);
        }
    }

    IEnumerator FedeColorChenge()
    {
        var currentColor = fadeImage.color;
        currentColor.a = 1f;

        while (fadeImage.color.a > 0.0f)
        {
            currentColor.a -= Time.fixedDeltaTime * fadeSpeed;
            fadeImage.color = currentColor;
            yield return null;
        }

        Debug.Log("フェード完了");
    }

    /*
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
    */
}
