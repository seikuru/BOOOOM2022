// Unityでシリアル通信、Arduinoと連携する雛形
// シリアル通信を制御するクラス
// 例えば空のGameObjectでも作って、それに関連付けする

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO.Ports; // これを通すために、Api Compatibility Levelの設定を変更
using System.Threading;
public class SerialHandler : MonoBehaviour
{
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    // COM10以上は\\\\.\\を付加しないと開けない。
    // portNameに直接代入するとなぜか失敗するので、ここでいったん別の変数に代入
    // string myPortName = "\\\\.\\COM37";
    string myPortName = "\\\\.\\COM9";
    public int bitRate = 115200;

    public string portName;
    private SerialPort serialPort_;
    private Thread thread_;
    private bool isRunning_ = false;

    private string message_;
    private bool isNewMessageReceived_ = false;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        portName = myPortName;
        Open();
    }

    void Update()
    {
        if (isNewMessageReceived_)
        {
            // Debug.Log("Serial message recieve");
            OnDataReceived(message_);
        }
        isNewMessageReceived_ = false;
    }

    void OnDestroy()
    {
        Close();
    }

    private void Open()
    {
        serialPort_ = new SerialPort(portName, bitRate, Parity.None, 8, StopBits.One);

        serialPort_.RtsEnable = true;
        serialPort_.DtrEnable = true;

        serialPort_.Open();

        isRunning_ = true;

        thread_ = new Thread(Read);
        thread_.Start();
    }

    private void Close()
    {
        isNewMessageReceived_ = false;
        isRunning_ = false;

        if (thread_ != null && thread_.IsAlive)
        {
            //thread_.Join();
            thread_.Abort();
        }

        if (serialPort_ != null && serialPort_.IsOpen)
        {
            serialPort_.Close();
            serialPort_.Dispose();
        }
    }

    private void Read()
    {
        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
        {
            try
            {
                message_ = serialPort_.ReadLine();
                isNewMessageReceived_ = true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    public void Write(string message)
    {
        try
        {
            serialPort_.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}
