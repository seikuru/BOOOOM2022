using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueContainer : MonoBehaviour
{
    // コントローラーから受け取るデータ
    public int rot;
    public int button;
    public int center;
    public List<int> t_rad;
    private List<int> old_rad;
    private int list_num = 2;

    private const float oneseg = 1.94343f;

    private void Start()
    {
        t_rad = new List<int>();
        old_rad = new List<int>();
        for(int i = 0; i < list_num; i++)
        {
            t_rad.Add(-999);
            old_rad.Add(-999);
        }
    }

    private void FixedUpdate()
    {
        // Debug.Log(get_rad() + ", " + get_button() + ", " + get_Trad());
        // Debug.Log(rot * oneseg);
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
        int ret;
        if (t_rad[0] == old_rad[0])
        {
            ret = -999;
        } else
        {
            ret = t_rad[0];
        }
        old_rad = new(t_rad);
        return ret;
    }
}
