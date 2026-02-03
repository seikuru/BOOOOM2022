using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    /// 複数ボタンの選択状態を管理する基底クラス
    /// カーソル移動、SE再生、決定入力を共通処理としてまとめる   

    [SerializeField] protected Button[] Buttons;
    [SerializeField] protected GameObject cursol;
    [SerializeField] protected AudioSource audioSE;
    [SerializeField] protected AudioClip audioClipUp;
    [SerializeField] protected AudioClip audioClipDown;
    [SerializeField] protected int currentButtonIndex = 0;

    // Indexを変更せず初期化などに使う値
    static readonly int ResetIndex = -99;

    /// <summary>
    /// ボタンのIndexを移動させる
    /// </summary>
    /// <param name="buttonIndexAdd">位置</param>
    public virtual void ButtonSelectMove(int buttonIndexAdd)
    {
        // ボタンが1つ以下なら処理不要
        if (Buttons.Length < 2)
            return;

        // 移動時のSE再生（ResetIndex指定時は鳴らさない）
        if (audioSE != null && buttonIndexAdd != ResetIndex)
        {
            if (buttonIndexAdd < 0 && audioClipUp != null)
            {
                audioSE.PlayOneShot(audioClipUp);
            }
            if (buttonIndexAdd > 0 && audioClipDown != null) 
            {
                audioSE.PlayOneShot(audioClipDown);
            }
        }

        // Index更新
        currentButtonIndex += buttonIndexAdd;
        bool sameCheck = false;

        // 下限チェック
        if (currentButtonIndex < 0)
        {
            currentButtonIndex = 0; sameCheck = true;
        }

        // 上限チェック
        if (currentButtonIndex >= Buttons.Length)
        {
            currentButtonIndex = Buttons.Length - 1; sameCheck = true;
        }

        // カーソルを選択中ボタン位置へ移動
        if (cursol != null)
            cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;

        // 実際に選択が変わった場合のみ派生クラス処理を呼ぶ
        if (!sameCheck)
            ButtonSelectoverride();
    }

    /// <summary>
    /// 選択変更時の追加処理用（派生クラスでオーバーライド）
    /// </summary>
    protected virtual void ButtonSelectoverride()
    {
        return;
    }

    /// <summary>
    /// 決定入力（現在選択中ボタンを実行）
    /// </summary>
    public void EnterButton()
    {
        ButtonInvoke();
    }

    /// <summary>
    /// 現在のボタンIndexのonClickを呼び出す
    /// </summary>
    private void ButtonInvoke()
    {
        Buttons[currentButtonIndex].onClick.Invoke();
    }

    /// <summary>
    /// 外部からIndexを直接設定する
    /// </summary>
    public void SetIndex(int num) => currentButtonIndex = num;

    /// <summary>
    /// エディタ上でのデバッグ操作用入力
    /// </summary>
    protected virtual void DebugInput()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ButtonSelectMove(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ButtonSelectMove(-1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ButtonInvoke();
        }
#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        DebugInput();
#endif
    }
}
