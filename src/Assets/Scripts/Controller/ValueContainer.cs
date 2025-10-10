using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueContainer : MonoBehaviour
{
    // コントローラーから受け取るデータ
    public int rot;
    public int button;
    public int t_rad;

    private const float oneseg = 1.94343f;

    private void FixedUpdate()
    {
        // Debug.Log(get_rad() + ", " + get_button() + ", " + get_Trad());
        Debug.Log(rot * oneseg);
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
