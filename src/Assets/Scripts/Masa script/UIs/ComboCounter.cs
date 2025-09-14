using UnityEngine;
using UnityEngine.UI;

public class ComboCounter : MonoBehaviour
{
    /// コンボカウンターを管理するシングルトンクラス
    /// ヒット数をカウントし、UI表示とスコア加算を行う

    [SerializeField] CountDownTimer countDownTimer;// スコア加算処理を行うタイマー

    [SerializeField] Text text;// コンボ数を表示するUIテキスト

    [SerializeField] string gobi_text = "Hit!";// コンボ表示時の接尾辞テキスト

    int comboCount; // 現在のコンボ数

    // シングルトン用のインスタンス
    [HideInInspector]
    static ComboCounter Instance;

    // 外部からインスタンスを取得するプロパティ
    [HideInInspector]
    public static ComboCounter GetCounter => Instance;

    private void Awake()
    {
        // シングルトンパターンの初期化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 複数生成を防止
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // 初期化処理
        ResetCombo();
        comboCount = 0; // コンボ数を初期化
        ComboView(); // 初期表示を更新
    }

    /// <summary>
    /// コンボ数を1増加させる
    /// スコア加算とUI更新も同時に行う
    /// </summary>
    public void AddCombo()
    {
        comboCount++;
        AddScore();
        ComboView();
    }

    /// <summary>
    /// コンボ数をリセットする
    /// </summary>
    public void ResetCombo() 
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
    void ComboView()
    {
        //Debug.Log(comboCount);

        // UI要素の存在確認
        if (text == null)
            return;

        // コンボ数に応じた表示切り替え
        if (comboCount == 0)
            text.text = string.Empty;  // 0の場合は非表示
        else
            text.text = comboCount.ToString() + gobi_text; // 数値+接尾辞で表示
    }
}
