using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManageSelectUI : MonoBehaviour
{
    enum UIstate
    {
        open,
        close,
        opening,
        closing,
        None,
    }

    [SerializeField] private GameObject COM_bg;
    [SerializeField] private SerialPortListup serialPortListup;
    [SerializeField] private ButtonManager buttonManager;
    [SerializeField] private float SceneChangeTime = 1.0f;

    private bool isSelect = false;
    private UIstate currentState = UIstate.None;
    private Vector2 initialBGScale;
    private float t = 0;

    void Start()
    {
        initialBGScale = COM_bg.transform.localScale;
        COM_bg.transform.localScale = new Vector2(0, 0);
        COM_bg.SetActive(false);

        currentState = UIstate.close;
    }

    void Update()
    {
        if (currentState == UIstate.opening)
        {
            t += Time.deltaTime;

            if (t >= SceneChangeTime)
            {
                t = SceneChangeTime;
                currentState = UIstate.open;

                // 画面が開ききったらボタンを配置
                StartCoroutine(buttonManager.CreateButton());
            }

            Vector2 scale = new Vector2(Mathf.Lerp(0, initialBGScale.x, t / SceneChangeTime), Mathf.Lerp(0, initialBGScale.y, t / SceneChangeTime));
            COM_bg.transform.localScale = scale;
        }

        if (currentState == UIstate.closing)
        {
            t += Time.deltaTime;
            if (t >= SceneChangeTime)
            {
                t = SceneChangeTime;
                currentState = UIstate.close;
                COM_bg.SetActive(false);

                
            }
            Vector2 scale = new Vector2(Mathf.Lerp(initialBGScale.x, 0, t / SceneChangeTime), Mathf.Lerp(initialBGScale.y, 0, t / SceneChangeTime));
            COM_bg.transform.localScale = scale;
        }
    }

    public void OnStart()
    {
        currentState = UIstate.opening;
        COM_bg.SetActive(true);
        t = 0;

        // process を先に開始させておく
        serialPortListup.StartProcess();
    }

    public void OnClose()
    {
        foreach (Transform child in buttonManager.canvasTransform_)
        {
            Destroy(child.gameObject);
        }
        currentState = UIstate.closing;
        t = 0;
    }
}
