using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboCounter : MonoBehaviour
{
    /// コンボカウンターを管理するシングルトンクラス
    /// ヒット数をカウントし、UI表示とスコア加算を行う

    [SerializeField] CountDownTimer countDownTimer;// スコア加算処理を行うタイマー

    [SerializeField] TextMeshProUGUI TMPro;// コンボ数を表示するUIテキスト

    [SerializeField] static string gobi_text = "Hit!";// コンボ表示時の接尾辞テキスト

    static TextMeshProUGUI staticText;
    static int comboCount; // 現在のコンボ数

    void Start()
    {
        // 初期化処理
        staticText = TMPro;

        ResetCombo();
        comboCount = 0; // コンボ数を初期化
        ComboView(); // 初期表示を更新
    }

    /// <summary>
    /// コンボ数を1増加させる
    /// スコア加算とUI更新も同時に行う
    /// </summary>
    public static void AddCombo()
    {
        comboCount++;
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
        countDownTimer?.AddCountWithCombo(comboCount);
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
}
