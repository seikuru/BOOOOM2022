using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    SerialPortListup serialPortListup_;
    int selectedIndex_;

    int buttonYStep_;
    int buttonXOffset_;

    public GameObject buttonPrefab_;
    public Transform canvasTransform_;
    public TMP_FontAsset japaneseFont_;
    public GameObject USBImagePrefab_;
    public GameObject descriptionPrefab_;

    SignalChangeDetector signalChangeDetector_;

    bool isReady_ = false;

    private void Awake()
    {
        buttonYStep_ = 92;
        buttonXOffset_ = 240;

        serialPortListup_ = GetComponent<SerialPortListup>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private System.Collections.IEnumerator Start()
    {
        yield return new WaitUntil(() => serialPortListup_ != null && serialPortListup_.isCompleted);

        for (int i = 0; i < serialPortListup_.portNum; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab_, canvasTransform_);
            // 位置をずらして配置
            buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -buttonYStep_ * i);

            // ボタンのテキストを変更
            Button buttonComp = buttonObj.GetComponent<Button>();
            TextMeshProUGUI label = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            label.text = serialPortListup_.COMPortName_[i];
            label.color = new Color32(0xFD, 0xFD, 0xFD, 0xFF); // ボタンの文字の色
            label.font = japaneseFont_; // ボタンの文字のフォント

            GameObject descriptionObj = Instantiate(descriptionPrefab_, canvasTransform_);
            descriptionObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(280, -buttonYStep_ * i - 15);

            TextMeshProUGUI descriptionLabel = descriptionObj.GetComponentInChildren<TextMeshProUGUI>();
            descriptionLabel.text = serialPortListup_.COMPortDetail_[i];
            descriptionLabel.color = new Color32(0xFD, 0xFD, 0xFD, 0xFF);
            descriptionLabel.font = japaneseFont_;

            int index = i;
            buttonComp.onClick.AddListener(() => OnButtonClicked(index)); // iを直接渡すと全てのボタンに最後の値が渡る。クロージャ問題。

            if (i == 0)
            {
                selectedIndex_ = 0;
                EventSystem.current.SetSelectedGameObject(buttonObj);
                USBImagePrefab_ = Instantiate(USBImagePrefab_, canvasTransform_);
                USBImagePrefab_.GetComponent<RectTransform>().anchoredPosition = buttonObj.GetComponent<RectTransform>().anchoredPosition + new Vector2(-150, 0);
            }
        }

        GameObject exitButtonObj = Instantiate(buttonPrefab_, canvasTransform_);

        exitButtonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -buttonYStep_ * serialPortListup_.portNum);

        // ボタンのテキストを変更
        Button exitButtonComp = exitButtonObj.GetComponent<Button>();
        TextMeshProUGUI exitLabel = exitButtonObj.GetComponentInChildren<TextMeshProUGUI>();
        exitLabel.text = "終了";
        exitLabel.color = new Color32(0xFD, 0xFD, 0xFD, 0xFF);
        exitLabel.font = japaneseFont_;

        exitButtonComp.onClick.AddListener(() => OnButtonClicked(serialPortListup_.portNum));

        signalChangeDetector_ = new SignalChangeDetector(selectedIndex_);

        isReady_ = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady_)
            return;

        var current = Keyboard.current;

        if (current.upArrowKey.wasPressedThisFrame)
            selectedIndex_--;
        if (current.downArrowKey.wasPressedThisFrame)
            selectedIndex_++;
        if (current.enterKey.wasPressedThisFrame)
        {
            OnButtonClicked(selectedIndex_);
        }

        if (selectedIndex_ < 0)
        {
            selectedIndex_ = 0;
        }
        else if (selectedIndex_ > serialPortListup_.portNum) // exitボタンがあるので条件の書き方はOK
        {
            selectedIndex_ = serialPortListup_.portNum;
        }

        signalChangeDetector_.Input(selectedIndex_);

        if (signalChangeDetector_.IsChanged())
        {
            USBImagePrefab_.GetComponent<RectTransform>().anchoredPosition = new Vector2(-buttonXOffset_, -buttonYStep_ * selectedIndex_);
        }

        signalChangeDetector_.Update();
    }

    void OnButtonClicked(int buttonIndex)
    {
        if (buttonIndex != selectedIndex_)
        {
            selectedIndex_ = buttonIndex;
            signalChangeDetector_.Input(selectedIndex_);

            if (signalChangeDetector_.IsChanged())
            {
                USBImagePrefab_.GetComponent<RectTransform>().anchoredPosition = new Vector2(-buttonXOffset_, -buttonYStep_ * selectedIndex_);
            }

            signalChangeDetector_.Update();
        }

        if (buttonIndex == serialPortListup_.portNum)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            //Debug.Log("button: " + buttonIndex.ToString());
            //Debug.Log("selected: " + serialPortListup_.COMPortName_[selectedIndex_]);
            PassCOMPort.selectedCOMPortName = serialPortListup_.COMPortName_[selectedIndex_];
            PassCOMPort.selectedCOMPortDetail = serialPortListup_.COMPortDetail_[selectedIndex_];

            serialPortListup_.isCompleted = false;

            // ここでシーンを呼ぶ
            // 例えば以下
            //SceneManager.LoadScene("SampleScene");
        }
    }
}
