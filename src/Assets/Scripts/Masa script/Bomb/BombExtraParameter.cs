using UnityEngine;

public class BombExtraParameter : MonoBehaviour
{ 
    /// 爆弾のパラメータを他のクラスの数値を参照して追加するクラス

    static bool isInitialized = false;　//シーン上にあるかを判断

    /// <summary>
    ///  今後条件によって爆弾の数値を換える場合ここで取得させる
    /// </summary>
    void Awake()
    {
        isInitialized = true;
    }

    /// <summary>
    /// 爆弾が与える力の大きさを追加する数値を取得
    /// 別のクラスのパラメータを参照していく
    /// </summary>
    /// <returns>力の大きさを追加する数値</returns>
    public static float GetAddStrange()
    {
        //シーン上にあるかを判断
        if (!isInitialized)
        {
            Debug.LogWarning("BombExtraParameterがシーンに存在しない状態");
            return 0;
        }

        // 他のクラスがない場合 0 を返す
        return 0;
    }
}
