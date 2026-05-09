using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoralMute : MonoBehaviour
{
    [SerializeField]
    AudioSource AudioSource;

    private void Start()
    {
        AudioSource.Play();
    }

    public void BGMFadeOut() => StartCoroutine(FadeOut_TutorialBGM());

    // Start is called before the first frame update
    IEnumerator FadeOut_TutorialBGM()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (AudioSource.volume > 0.0f)
        {
            AudioSource.volume -= 0.1f;
            yield return wait;
        }
    }
}
