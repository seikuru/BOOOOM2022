using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueContainer : MonoBehaviour
{
    // コントローラーから受け取るデータ
    public int rot;
    public int button;
    public int center;
    public int esc;
    public List<int> in_rad;
    public List<int> out_rad;
    private List<int> oldin_rad;
    private List<int> oldout_rad;
    private int old_center = 0;
    private int list_num = 3;

    private const float oneseg = 1.94343f;

    private void Start()
    {
        in_rad = new List<int>();
        out_rad = new List<int>();
        oldin_rad = new List<int>();
        oldout_rad = new List<int>();
        for(int i = 0; i < list_num; i++)
        {
            in_rad.Add(-999);
            out_rad.Add(-999);
            oldin_rad.Add(-999);
            oldout_rad.Add(-999);
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
    /// escボタンのフラグを取得
    /// </summary>
    /// <returns>押してあるなら 1 </returns>
    public int get_esc()
    {
        return esc;
    }

    /// <summary>
    /// Legacy
    /// 
    /// タッチした時の角度を取得
    /// 中心からの距離は取れないので角度のみ、中心はドーナツ状のように取れない。
    /// </summary>
    /// <returns>タッチした時の角度</returns>
    public int get_Trad() 
    {
        int ret;
        if (in_rad[0] == oldin_rad[0])
        {
            ret = -999;
        } else
        {
            ret = in_rad[0];
        }
        oldin_rad = new(in_rad);
        return ret;
    }

    public List<int> get_inRad()
    {
        return in_rad;
    }

    public List<int> get_outRad()
    {
        return out_rad;
    }

    public bool get_under()
    {
        bool tap = false;

        if(center == 1 && old_center == 0)
            tap = true;

        old_center = center;

        return tap;
    }
}
