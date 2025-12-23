using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonFocus : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // 初期状態ではオフ
        }
    }

    // フォーカスが当たったとき（キーボードで選択されたとき）
    public void OnSelect(BaseEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    // フォーカスが外れたとき
    public void OnDeselect(BaseEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
