using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboCounter : MonoBehaviour
{
    /// コンボカウンターを管理するstaticクラス
    /// ヒット数をカウントし、UI表示とスコア加算を行う

    // [SerializeField] CountDownTimer countDownTimer;// スコア加算処理を行うタイマー

    [SerializeField] TextMeshProUGUI TMPro;// コンボ数を表示するUIテキスト

    [SerializeField] RectTransform GaugeRectTF; 

    [SerializeField] static string gobi_text = "Hit!";// コンボ表示時の接尾辞テキスト

    [SerializeField] static float TimeLimit = 10f; // コンボが終了するまでの時間

    static TextMeshProUGUI staticText;
    static RectTransform RectTF;
    static int comboCount; // 現在のコンボ数
    static float TimeCounter = 0;

    static Vector3 RectPos;
    static float sizeDelta_x;


    void Start()
    {
        // 初期化処理
        staticText = TMPro;
        RectTF = GaugeRectTF;

        // 初期数値を取得
        RectPos = RectTF.position;
        sizeDelta_x = RectTF.sizeDelta.x;

        ResetCombo();
        comboCount = 0; // コンボ数を初期化
        ComboView(); // 初期表示を更新

        ComboGaugeView(0f);

        TimeCounter = 0;
    }

    /// <summary>
    /// コンボ数を1増加させる
    /// スコア加算とUI更新も同時に行う
    /// </summary>
    public static void AddCombo()
    {
        comboCount++;
        TimeCounter = TimeLimit;

        ComboGaugeView(1f);

        //AddScore();
        ComboView();
    }

    /// <summary>
    /// コンボ数をリセットする
    /// </summary>
    public static void ResetCombo() 
    {
        comboCount = 0; // コンボ数を初期化
        ComboView(); // 初期表示を更新
    }

    /// <summary>
    /// 現在のコンボ数に応じてスコアを加算
    /// </summary>
    void AddScore()
    {
        //countDownTimer?.AddCountWithCombo(comboCount);
    }

    /// <summary>
    /// コンボ数のUI表示を更新
    /// コンボ数が0の場合は空文字、それ以外は数値+接尾辞を表示
    /// </summary>
    static void ComboView()
    {
        //Debug.Log(comboCount);

        // UI要素の存在確認
        if (staticText == null)
            return;

        // コンボ数に応じた表示切り替え
        if (comboCount == 0)
            staticText.SetText(string.Empty);  // 0の場合は非表示
        else
            staticText.SetText(comboCount.ToString() + gobi_text); // 数値+接尾辞で表示
    }

    static void ComboGaugeView(float Value_01)
    {
        if (RectTF == null)
            return;

        RectTF.position = new Vector3()
        {
            x = (RectPos.x - sizeDelta_x / 2) + (sizeDelta_x / 2) * Value_01,
            y = RectPos.y,
            z = RectPos.z,
        };

        RectTF.sizeDelta = new Vector2()
        {
            x = sizeDelta_x * Value_01,
            y = RectTF.sizeDelta.y,
        };
    }

    private void FixedUpdate()
    {
        if (TimeCounter <= 0)
            return;

        TimeCounter -= Time.fixedDeltaTime;

        ComboGaugeView(Mathf.Clamp01(TimeCounter / TimeLimit));

        if (TimeCounter <= 0)
        {
            ResetCombo();
        }
    }
}
