using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPadRapidFire : UITableInput
{
    /// タッチパッド（マルチタッチ）入力を使用したUIテーブル操作クラス
    /// UITableInputを継承し、複数の指による同時タッチ操作を処理
    /// 最新のタッチを優先した回転制御とタップによる投擲を実現
    /// 外側リングを押しながら内側を触ると連射状態に

    [Header("TouchPadRapidFire")]
    [Space]
    [SerializeField] float TraceDistance = 80f;

    enum TouchSide
    {
        OutRange,
        Inside,
        Outside,
    }

    /// <summary>
    /// タッチ情報を格納するクラス
    /// 各タッチの状態と履歴を管理
    /// </summary>
    class TouchInfo
    {
        public int ID; // タッチのユニークID（fingerId）
        public float BeganTime; // タッチ開始時刻
        public TouchSide InTableSide; // テーブルの範囲でタッチを開始したか
        public Vector3 BeganPos; // タッチ開始位置
        public Vector3 CurrentPos; // 現在のタッチ位置
    };

    Dictionary<int, TouchInfo> TouchDictionary = new Dictionary<int, TouchInfo>(); // 全タッチ情報の辞書

    int TableRotateID = -1; // 現在テーブル回転を制御しているタッチのID
    int TableTraceID = -1; // なぞり状態のタッチのID

    float TraceValue = 0f;

    void Start()
    {
        TableRotateID = -1; // 回転制御タッチIDを初期化
        TableTraceID = -1;　// なぞり状態タッチのIDを初期化
        TouchDictionary = new Dictionary<int, TouchInfo>(); // タッチ辞書を初期化
    }

    /// <summary>
    /// UI位置を画面範囲内にクランプ
    /// タッチ座標が画面外に出ることを防ぐ
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

    TouchSide CheckTouchSide(Vector3 touchPosition)
    {
        if (IsInInnerTableRect(touchPosition))// テーブル内側範囲内判定
        { 
            return TouchSide.Inside;
        }
        if (IsInOuterTableRect(touchPosition))// テーブル外側範囲内判定
        {
            return TouchSide.Outside;
        }

        return TouchSide.OutRange;
    }

    /// <summary>
    /// タッチ入力処理のメイン関数
    /// 基底クラスのInputOperateをオーバーライドしてマルチタッチ処理を実装
    /// </summary>
    protected override void InputOperate()
    {
        // タッチ入力（スマートフォン用）
        if (Input.touchCount > 0)
        {
            // タッチをフェーズごとに仕分けするためのリスト
            List<Touch> beganList = new(); // 新しく開始されたタッチ
            List<Touch> TouchList = new(); // 継続中のタッチ（移動・静止）
            List<Touch> endedList = new(); // 終了したタッチ

            // 現在の全タッチを走査してフェーズごとに分類
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began)
                    beganList.Add(touch);// 新しく触れた

                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    TouchList.Add(touch);// 移動中 or 静止中

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    endedList.Add(touch);// 離された or キャンセルされた
            }

            // タッチ終了処理
            // Ended → Began の順に処理しておくと、同一 fingerId の再利用にも安全に利用できる
            foreach (var t in endedList)
            {
                // 画面座標をクランプして取得
                TouchDictionary.Remove(t.fingerId);
            }

            bool IsTouchOutside = false;

            // 新規タッチ開始処理
            foreach (var t in beganList)
            {
                // 画面座標をクランプして取得
                Vector3 touchPosition = InputTouchClamp(t.position);

                TouchSide touchSide = CheckTouchSide(touchPosition);// テーブル範囲内判定

                // タッチ情報を辞書に登録
                TouchDictionary.Add(t.fingerId, new()
                {
                    ID = t.fingerId,
                    BeganTime = Time.time,// 開始時刻を記録（優先度判定用）
                    InTableSide = touchSide,
                    BeganPos = touchPosition,
                    CurrentPos = touchPosition,
                });

                if (touchSide == TouchSide.Outside)
                    IsTouchOutside = true;

                // タップ開始時の爆弾発射処理
                ShotTable(touchPosition);
            }

            // 外側
            float OutBeganTime = -1f; // 最新のタッチ時刻
            int OutBeganID = -1; // 最新のタッチID

            // 内側
            float InBeganTime = -1f; // 最新のタッチ時刻
            int InBeganID = -1; // 最新のタッチID

            // タッチ移動処理
            foreach (var t in TouchList)
            {
                if (TouchDictionary.TryGetValue(t.fingerId, out var value))
                {
                    // 現在位置を更新
                    value.CurrentPos = InputTouchClamp(t.position);

                    if (value.InTableSide == TouchSide.Outside)
                    {
                        IsTouchOutside = true;

                        // 外側かつ最も新しいタッチを優先
                        if (OutBeganTime < value.BeganTime)
                        {
                            OutBeganID = value.ID;
                            OutBeganTime = value.BeganTime;
                        }
                    }
                    else
                    {
                        // 内側かつ最も新しいタッチを優先
                        if (InBeganTime < value.BeganTime)
                        {
                            InBeganID = value.ID;
                            InBeganTime = value.BeganTime;
                        }
                    }
                }
            }
            if(IsTouchOutside && OutBeganID != -1)
            {
                // 操作対象のタッチが切り替わった場合、基準角度を更新
                if (TableRotateID != OutBeganID)
                {
                    UpdateAngleTableRect(); // 現在の角度を保存

                    TableRotateID = OutBeganID; // 制御タッチIDを更新

                    // 新しいタッチの開始位置を現在位置に更新（滑らかな切り替えのため）
                    TouchDictionary[OutBeganID].BeganPos = TouchDictionary[OutBeganID].CurrentPos;
                }

                // 回転処理を実行
                RotateTable(TouchDictionary[OutBeganID].BeganPos, TouchDictionary[OutBeganID].CurrentPos);

                if(InBeganID != -1)
                {
                    // なぞり対象のタッチが切り替わった場合、基準位置を更新
                    if (TableTraceID != InBeganID)
                    {
                        TableTraceID = InBeganID; // 制御タッチIDを更新

                        TraceValue = 0f;
                    }

                    TraceValue += Vector3.Distance(TouchDictionary[InBeganID].BeganPos,
                                               TouchDictionary[InBeganID].CurrentPos);

                    // 新しいタッチの開始位置を現在位置に更新（滑らかな切り替えのため）
                    TouchDictionary[InBeganID].BeganPos = TouchDictionary[InBeganID].CurrentPos;

                    if (TraceValue > TraceDistance)
                    {
                        TraceValue = 0f;

                        // 爆弾発射処理
                        ShotTable(TouchDictionary[InBeganID].CurrentPos,true);
                    }
                }
            }

            // テーブル回転処理
            else if (InBeganID != -1)
            {
                // 操作対象のタッチが切り替わった場合、基準角度を更新
                if (TableRotateID != InBeganID)
                {
                    UpdateAngleTableRect(); // 現在の角度を保存

                    TableRotateID = InBeganID; // 制御タッチIDを更新

                    // 新しいタッチの開始位置を現在位置に更新（滑らかな切り替えのため）
                    TouchDictionary[InBeganID].BeganPos = TouchDictionary[InBeganID].CurrentPos;
                }
                // 回転処理を実行
                RotateTable(TouchDictionary[InBeganID].BeganPos, TouchDictionary[InBeganID].CurrentPos);
            }
            else
            {
                // 有効なタッチがない場合、現在の角度を保存だけ行う
                UpdateAngleTableRect();
            }
        }
    }
}

