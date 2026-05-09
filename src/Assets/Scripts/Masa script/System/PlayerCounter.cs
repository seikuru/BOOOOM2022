using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static PlayerCountData;
using static RankingData;

[System.Serializable]
public class PlayerCountData
{
    [System.Serializable]
    public class OneDayPlayer
    {
        public string Name;
        public int[] Count;
    }

    public OneDayPlayer[] days;
}
public class PlayerCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI counter_text;

    [SerializeField]
    private string file_name = "/PlayerCountData.json";

    private string file_path;

    private static PlayerCountData data;

    private static string today_text;

    public void AddPlayer(int stage_index)
    {
        if (stage_index < 0 || stage_index > 2)
            return;

        bool today_check = true;

        for (int i = 0; i < data.days.Length; i++)
        {
            if (data.days[i].Name == today_text)
            {
                data.days[i].Count[stage_index]++;
                today_check = false;
                break;
            }
        }

        if (today_check)
        {
            Array.Resize(ref data.days, data.days.Length + 1);
            data.days[data.days.Length - 1] = new OneDayPlayer();
            data.days[data.days.Length - 1].Name = today_text;

            Array.Resize(ref data.days[data.days.Length - 1].Count, 3);

            for (int i = 0; i < data.days.Length; i++)
            {
                data.days[data.days.Length - 1].Count[i] = 0;
            }

            data.days[data.days.Length - 1].Count[stage_index] = 1;
        }

        SaveFile();
    }

    void CreateData()
    {
        if (data != null)
            return;

        data = new PlayerCountData();

        data.days = new OneDayPlayer[0];

        SaveFile();
    }


    void LordFile()
    {
        //データが存在しない場合
        if (!File.Exists(file_path))
        {
            Debug.Log("データが存在しません");
        }
        //データが存在する場合
        if (File.Exists(file_path))
        {
            string json = File.ReadAllText(file_path);
            PlayerCountData loadedData = JsonUtility.FromJson<PlayerCountData>(json);

            data = loadedData;
        }
    }

    void SaveFile()
    {
        // JSONに変換
        string json = JsonUtility.ToJson(data, true);

        // ファイルに保存
        File.WriteAllText(file_path, json);
    }

    private void Awake()
    {
        DateTime dt = DateTime.Now;
        today_text = dt.ToString("yyyyMMdd");

        file_path = Application.persistentDataPath + file_name;

        Debug.Log(file_path);

        LordFile();
        CreateData();
    }

    private void Start()
    {
        int sum = 0;

        for (int i = 0; i < data.days.Length; i++)
        {
            if (data.days[i].Name == today_text)
            {
                for (int j = 0; j < data.days[i].Count.Length; j++)
                {
                    sum += data.days[i].Count[j];
                }
                break;
            }
        }

        counter_text.SetText("来場者数\n" + sum.ToString());
    }
}
