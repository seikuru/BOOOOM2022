using UnityEngine;

public class MouseInput : UITableInput
{
    /// マウス入力を使用したUIテーブル操作クラス
    /// UITableInputを継承し、マウスのクリック・ドラッグ操作を処理
    /// 左クリックで投擲、ドラッグで回転操作を実現

    Vector3 mousePosition;// 現在のマウス位置（ドラッグ中の位置）
    Vector3 mouseDownPosition;// マウスクリック開始位置

    /// <summary>
    /// UI位置を画面範囲内にクランプ
    /// マウス座標が画面外に出ることを防ぐ
    /// </summary>
    /// <param name="UIposition">クランプ対象のUI座標</param>
    /// <returns>画面範囲内にクランプされた座標</returns>
    Vector3 InputTouchClamp(Vector3 UIposition)
    {
        return new Vector3()
        {
            x = Mathf.Clamp(UIposition.x, 0, WidthClampMax), // X座標を画面幅内に制限
            y = Mathf.Clamp(UIposition.y, 0, HeightClampMax), // Y座標を画面高さ内に制限
            z = 0 // Z座標は常に0（2D UI用）
        };
    }

    /// <summary>
    /// マウス入力処理のメイン関数
    /// 基底クラスのInputOperateをオーバーライドして具体的な入力処理を実装
    /// </summary>
    protected override void InputOperate()
    {
        // マウス左ボタン押下時の処理
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPosition = InputTouchClamp(Input.mousePosition); // クリック位置を記録
            ShotTable(mouseDownPosition); // 投擲処理を実行
        }

        // マウス左ボタン押下継続中の処理（ドラッグ中）
        if (Input.GetMouseButton(0))
        {
            mousePosition = InputTouchClamp(Input.mousePosition); // 現在位置を取得
            RotateTable(mouseDownPosition, mousePosition); // 回転処理を実行
        }

        // マウス左ボタン離した時の処理
        if (Input.GetMouseButtonUp(0))
        {
            mouseDownPosition = Vector3.zero; // クリック開始位置をリセット
            mousePosition = Vector3.zero; // 現在位置をリセット
            UpdateAngleTableRect(); // テーブル角度を更新（次回操作の基準値として保存）
        }
    }
}
