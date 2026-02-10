using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCommand : MonoBehaviour
{
    [SerializeField]
    ScoreRanking scoreRanking;

    private KeyCode[] command = new KeyCode[]
     {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
     };

    private int index = 0;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(command[index]))
            {
                index++;

                if (index >= command.Length)
                {
                    Debug.Log("コマンド成功！");
                    index = 0;
                    scoreRanking.ResetData();
                }
            }
            else
            {
                // 間違えたらリセット
                index = 0;
            }
        }
    }
}
