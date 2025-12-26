using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
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
    [SerializeField] private ManageSelectUI selectUI;

    SignalChangeDetector signalChangeDetector_;

    bool isReady_ = false;

    private void Awake()
    {
        buttonYStep_ = 92;
        buttonXOffset_ = 240;

        serialPortListup_ = GetComponent<SerialPortListup>();
    }

    void Start()
    {
        
    }

    public System.Collections.IEnumerator CreateButton()
    {
        yield return new WaitUntil(() => serialPortListup_ != null && serialPortListup_.isCompleted);
        
        for (int i = 0; i < serialPortListup_.portNum; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab_, canvasTransform_);
            // 位置をずらして配置
            buttonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -buttonYStep_ * i);

            // ボタンのテキストを変更
            Button buttonComp = buttonObj.GetComponent<Button>();
            TextMeshProUGUI label = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            label.text = serialPortListup_.COMPortName_[i];
            label.color = new Color32(0xFD, 0xFD, 0xFD, 0xFF); // ボタンの文字の色
            label.font = japaneseFont_; // ボタンの文字のフォント

            GameObject descriptionObj = Instantiate(descriptionPrefab_, canvasTransform_);
            descriptionObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(180, -buttonYStep_ * i - 15);

            TextMeshProUGUI descriptionLabel = descriptionObj.GetComponentInChildren<TextMeshProUGUI>();
            descriptionLabel.text = serialPortListup_.COMPortDetail_[i];
            descriptionLabel.color = new Color32(0xFD, 0xFD, 0xFD, 0xFF);
            descriptionLabel.font = japaneseFont_;

            int index = i;
            buttonComp.onClick.AddListener(() => OnButtonClicked(index)); // iを直接渡すと全てのボタンに最後の値が渡る。クロージャ問題。
            buttonComp.onClick.AddListener(() => selectUI.OnClose());

            if (i == 0)
            {
                selectedIndex_ = 0;
                EventSystem.current.SetSelectedGameObject(buttonObj);
                // USBImagePrefab_ = Instantiate(USBImagePrefab_, canvasTransform_);
                // USBImagePrefab_.GetComponent<RectTransform>().anchoredPosition = buttonObj.GetComponent<RectTransform>().anchoredPosition + new Vector2(-150, 0);
            }
        }

        GameObject exitButtonObj = Instantiate(buttonPrefab_, canvasTransform_);

        exitButtonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -buttonYStep_ * serialPortListup_.portNum);

        // ボタンのテキストを変更
        Button exitButtonComp = exitButtonObj.GetComponent<Button>();
        TextMeshProUGUI exitLabel = exitButtonObj.GetComponentInChildren<TextMeshProUGUI>();
        exitLabel.text = "閉じる";
        exitLabel.color = new Color32(0xFD, 0xFD, 0xFD, 0xFF);
        exitLabel.font = japaneseFont_;

        exitButtonComp.onClick.AddListener(() => OnButtonClicked(serialPortListup_.portNum));
        exitButtonComp.onClick.AddListener(() => selectUI.OnClose());

        signalChangeDetector_ = new SignalChangeDetector(selectedIndex_);

        isReady_ = true;
    }

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

        if(buttonIndex == serialPortListup_.portNum)
        {
            // 終了ボタンが押されたときの処理
            // Application.Quit();
        }
        else
        {
            // COMポートが選択されたときの処理
            // Debug.Log("Selected COM Port: " + serialPortListup_.COMPortName_[buttonIndex]);
            // ここにCOMポートを使用する処理を追加
            PassCOMPort.selectedCOMPortName = serialPortListup_.COMPortName_[selectedIndex_];
            PassCOMPort.selectedCOMPortDetail = serialPortListup_.COMPortDetail_[selectedIndex_];
        }
    }

    public void DeleteButton()
    {
        for(int i = 0; i < serialPortListup_.portNum; i++)
        {
            Destroy(canvasTransform_.GetChild(0).gameObject);
            Destroy(canvasTransform_.GetChild(0).gameObject);
        }
    }
}
