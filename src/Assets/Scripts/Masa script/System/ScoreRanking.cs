using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static RankingData;

[System.Serializable]
public class RankingData
{
    [System.Serializable]
    public class GameMode
    {
        public string Name;
        public int[] Score;
    }

    public GameMode[] mode;
}
public class ScoreRanking : MonoBehaviour
{
    [SerializeField]
    private string fileName = "/RankingData.json";

    [SerializeField]
    private string[] gameModeName = new string[]{ "ichi_yama", "Ame_Hard" };

    [SerializeField]
    private int[] ResetScore = new int[5] { 25000, 22000, 19800, 18400, 15000 };

    private string filePath;

    private static int[] ResetScoreDammy;

    private static RankingData data;
    private static string currentGameMode;

    public static void SetCurrentGameMode()
    {
        currentGameMode = SceneManager.GetActiveScene().name;
    }

    public void AddHighScore(int score)
    {
        for (int i = 0; i < gameModeName.Length; i++)
        {
            if (currentGameMode == data.mode[i].Name)
            {
                int[] temp = new int[6];

                for (int j = 0; j < 5; j++)
                    temp[j] = data.mode[i].Score[j];

                temp[5] = score;

                System.Array.Sort(temp);
                System.Array.Reverse(temp);

                for (int j = 0; j < 5; j++)
                    data.mode[i].Score[j] = temp[j];

                break;
            }     
        }

        SaveFile();
    }


    void CreateData()
    {
        if (data != null && data.mode.Length == gameModeName.Length)
            return;

        data = new RankingData();

        data.mode = new GameMode[gameModeName.Length];

        for (int i = 0; i < gameModeName.Length; i++)
        {
            data.mode[i] = new GameMode();
            data.mode[i].Name = gameModeName[i];
            data.mode[i].Score = (int[])ResetScore.Clone();
        }

        SaveFile();
    }


    void SaveFile()
    {
        // JSONに変換
        string json = JsonUtility.ToJson(data, true);

        // ファイルに保存
        File.WriteAllText(filePath, json);
    }


    void LordFile()
    {
        //データが存在しない場合
        if (!File.Exists(filePath))
        {
            Debug.Log("データが存在しません");
        }
        //データが存在する場合
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            RankingData loadedData = JsonUtility.FromJson<RankingData>(json);

            data = loadedData;
        }
    }

    void Awake()
    {
        ResetScoreDammy = ResetScore;
        filePath = Application.persistentDataPath + fileName;

        Debug.Log(filePath);

        LordFile();
        CreateData();
    }

    public static int[] GetRankingData(string sceneName)
    {
        if (data == null || data.mode == null)
            return ResetScoreDammy;

        for (int i = 0; i < data.mode.Length; i++)
        {
            if (sceneName == data.mode[i].Name)
            {
                if(data.mode[i].Score != null && data.mode[i].Score.Length == 5)
                    return data.mode[i].Score;
            }
        }

        return ResetScoreDammy;
    }

    public static int[] GetRankingDataBeforeMode()
    {
        if (data == null || data.mode == null)
            return ResetScoreDammy;

        for (int i = 0; i < data.mode.Length; i++)
        {
            if (currentGameMode == data.mode[i].Name)
            {
                if (data.mode[i].Score != null && data.mode[i].Score.Length == 5)
                    return data.mode[i].Score;
            }
        }

        return ResetScoreDammy;
    }
}
