using UnityEngine;
using TMPro;

public class EnemyCount : MonoBehaviour
{

    TextMeshProUGUI EnemyCountText;
    [SerializeField]public int int_EnemyCount = 30;
    int int_StartEnemyCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyCountText = GetComponent<TextMeshProUGUI>();
        int_StartEnemyCount = int_EnemyCount;
    }

    public void ChangeTextInt(int ToIntText)
    {
        string text = (int_StartEnemyCount - ToIntText).ToString() +"/"+ int_StartEnemyCount.ToString();
        EnemyCountText.text = text;

    }
}
