using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI CoinText;
    [SerializeField]
    Text CoinTextLegacy;
    static TextMeshProUGUI Text;
    static Text TextLegacy;
    static int Count = 0;

    private void Start()
    {
        //Count++;
        Text = CoinText;
        TextLegacy = CoinTextLegacy;

    }

    public static void AddCount()
    {
        Count++;
        if (Text != null)
        {
            Text.SetText("Coin:" + Count.ToString());
        }
        if (TextLegacy != null)
        {
            TextLegacy.text = "Coin:" + Count.ToString();
        }
    }
}
