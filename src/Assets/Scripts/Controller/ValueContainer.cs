using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueContainer : MonoBehaviour
{
    // コントローラーから受け取るデータ
    public int rot;
    public int button;
    public int t_rad;

    private const float oneseg = 7.2375f;

    private void Update()
    {
        // Debug.Log(get_rad() + ", " + get_button() + ", " + get_Trad());
    }

    public float get_rad()
    {
        return rot * oneseg;
    }

    public void reset_rad()
    {
        rot = 0;
    }

    public int get_button()
    {
        return button;
    }

    public int get_Trad() 
    { 
        return t_rad; 
    }
}
