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

    /// <summary>
    /// 前の状態から何度回っているか(10なら時計回りに10度)
    /// </summary>
    /// <returns>回った度数</returns>
    public float get_rad()
    {
        return rot * oneseg;
    }

    /// <summary>
    /// rotをリセット
    /// </summary>
    public void reset_rad()
    {
        rot = 0;
    }

    /// <summary>
    /// ボタンのフラグを取得
    /// </summary>
    /// <returns>押してあるなら 1 </returns>
    public int get_button()
    {
        return button;
    }

    /// <summary>
    /// タッチした時の角度を取得
    /// 中心からの距離は取れないので角度のみ、中心はドーナツ状のように取れない。
    /// </summary>
    /// <returns>タッチした時の角度</returns>
    public int get_Trad() 
    { 
        return t_rad; 
    }
}
