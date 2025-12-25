using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
//using static UnityEditor.LightingExplorerTableColumn;

public class SerialPortListup : MonoBehaviour
{
    public int portNum { get; private set; }
    public string[] portName { get; private set; }

    public bool isCompleted { get; private set; }

    Process process_;
    static readonly string FolderPath = Application.streamingAssetsPath + "/Apps";
    static readonly string FilePath = FolderPath + "/GetCOMPortInfo.exe";

    int ExistingCOMPort_;
    public List<string> COMPortName_;
    public List<string> COMPortDetail_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        ExistingCOMPort_ = 0;
        COMPortName_ = new List<string>();
        COMPortDetail_ = new List<string>();

        process_ = new Process();

        //if (!File.Exists(FilePath))
        //{
        //    UnityEngine.Debug.LogError("SerialPortName.exeが見つかりません: " + FilePath);
        //    // ここで IsCompleted を true にしておくと、少なくともUIは出せる
        //    isCompleted = true;
        //    return;
        //}

        // プロセスを起動するときに使用する値のセットを指定
        process_.StartInfo = new ProcessStartInfo
        {
            FileName = FilePath,                        // 起動するファイルのパスを指定する
            UseShellExecute = false,                    // プロセスの起動にオペレーティング システムのシェルを使用するかどうか(既定値:true)
            WorkingDirectory = FolderPath,              // 開始するプロセスの作業ディレクトリを取得または設定する(既定値:"")
            RedirectStandardInput = true,               // StandardInput から入力を読み取る(既定値：false)
            RedirectStandardOutput = true,              // 出力を StandardOutput に書き込むかどうか(既定値：false)
            CreateNoWindow = true,                      // プロセス用の新しいウィンドウを作成せずにプロセスを起動するかどうか(既定値：false)
            StandardOutputEncoding = Encoding.UTF8,
            //StandardOutputEncoding = Encoding.GetEncoding("shift-jis"),
        };

        // 外部プロセスのStandardOutput ストリームに行を書き込む度に発火されるイベント
        process_.OutputDataReceived += OnStandardOut;

        //外部プロセスの終了を検知する
        process_.EnableRaisingEvents = true;
        process_.Exited += DisposeProcess;

        // プロセスを起動する
        process_.Start();
        process_.BeginOutputReadLine();
    }

    void OnStandardOut(object sender, DataReceivedEventArgs e)
    {
        // プロセス終了時などで null が来ることがあるので防御
        if (string.IsNullOrEmpty(e.Data))
            return;

        // 必ずCOMポート名→COM詳細の順でセットで表示される
        if (e.Data[0] == 'N')
        {
            COMPortName_.Add(e.Data.Substring(1)); // 先頭1文字（'N'）を削除
            ExistingCOMPort_++;
        }
        else if (e.Data[0] == 'D')
        {
            COMPortDetail_.Add(e.Data.Substring(1)); // 先頭1文字（'D'）を削除
        }
    }

    void DisposeProcess(object sender, EventArgs e)
    => DisposeProcess();

    void DisposeProcess()
    {
        if (process_ == null)
            return;

        try
        {
            // ExitedイベントのときにはすでにHasExited == true
            if (!process_.HasExited)
            {
                process_.StandardInput.Close();
                process_.CloseMainWindow();
            }
        }
        catch (Exception)
        {
            // 終了タイミングによっては例外になるので握りつぶす
        }

        process_.Dispose();
        process_ = null;

        portNum = ExistingCOMPort_;

        isCompleted = true;
    }

}
