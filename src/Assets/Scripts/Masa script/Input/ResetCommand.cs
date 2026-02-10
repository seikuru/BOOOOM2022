using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCommand : MonoBehaviour
{
    [SerializeField]
    ScoreRanking scoreRanking;

    private int index = 0;

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

    void Update()
    {
        foreach (KeyCode key in command)
        {
            if (Input.GetKeyDown(key))
            {
                CheckInput(key);
                break;
            }
        }
    }

    void CheckInput(KeyCode key)
    {
        if (key == command[index])
        {
            index++;

            if (index >= command.Length)
            {
                Debug.Log("成功！");
                index = 0;
                scoreRanking.ResetData();
            }
        }
        else
        {
            // もし最初のキーなら1からやり直す
            index = (key == command[0]) ? 1 : 0;
        }
    }
}
