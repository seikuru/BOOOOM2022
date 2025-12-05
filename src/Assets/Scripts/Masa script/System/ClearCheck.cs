using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCheck : MonoBehaviour
{
    [SerializeField] BGMControll BGMControll;
    [SerializeField] GameClearSetiingTest GameClearSetiingTest;

    public void Check()
    {
        if (BGMControll == null || GameClearSetiingTest == null)
            return;
        if (BGMControll.AllCollectBGMCheck)
        {
            GameClearSetiingTest.GameClear();
        }
        else
        {
            GameClearSetiingTest.GameOver();
        }
    }

}
