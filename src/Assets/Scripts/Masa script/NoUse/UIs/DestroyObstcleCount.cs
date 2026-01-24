using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestroyObstcleCount : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI textuGUI;
    [SerializeField]
    string BackText = "/5";
    static int count;
    private void Start()
    {
        count = 0;
    }

    static public void DestroyAddCount()
    {
        count++;
    }

    private void Update()
    {
        textuGUI.SetText(count.ToString() + BackText);
    }
}
