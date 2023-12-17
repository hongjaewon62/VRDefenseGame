using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class Score : MonoBehaviour
{
    public static Score score;
    [Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }

    private class HighScores
    {
        public List<HighScoreEntry> highScoreEntryList;
    }

    public Transform rankPanel;
    public Transform rank;
    public Transform myRank;
    public Transform content;
    private List<HighScoreEntry> highScoreList;
    private List<Transform> transformList;
    public bool gameOver = false;

    private void Awake()
    {
        if (score == null)
        {
            score = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        rank.gameObject.SetActive(false);

        //DeleteScore();
        if (PlayerPrefs.GetString("highScoreTable") == "" || !PlayerPrefs.HasKey("highScoreTable"))
        {
            HighScores ahighScores = new HighScores { highScoreEntryList = highScoreList };
            string json = JsonUtility.ToJson(ahighScores);
            PlayerPrefs.SetString("highScoreTable", json);
            PlayerPrefs.Save();
        }

        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScores.highScoreEntryList.Count; j++)
            {
                if (highScores.highScoreEntryList[j].score > highScores.highScoreEntryList[i].score)
                {
                    HighScoreEntry temp = highScores.highScoreEntryList[i];
                    highScores.highScoreEntryList[i] = highScores.highScoreEntryList[j];
                    highScores.highScoreEntryList[j] = temp;
                }
            }
        }

        transformList = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScores.highScoreEntryList)
        {
            CreateHighScoreTransform(highScoreEntry, rank, transformList);
        }
        Debug.LogError(PlayerPrefs.GetString("highScoreTable"));

        //AddHighScoreEntry(1, "10");
        //Debug.Log(PlayerPrefs.GetString("highScoreTable"));
    }

    private void CreateHighScoreTransform(HighScoreEntry highScoreEntry, Transform rank, List<Transform> transformList)
    {
        TextMeshProUGUI rankText = rank.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = rank.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = rank.GetChild(2).GetComponent<TextMeshProUGUI>();

        float templateHeight = 60f;

        int rankNum = transformList.Count + 1;
        int score = highScoreEntry.score;
        string name = highScoreEntry.name;
        rankText.text = rankNum.ToString();
        nameText.text = score.ToString();
        scoreText.text = name;

        Transform transform = Instantiate(rank, rankPanel);
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, (-templateHeight * transformList.Count) - 30);        // 스크롤 뷰 사용 시 중심이 달라 -30을 해줌
        transform.gameObject.SetActive(true);

        transformList.Add(transform);
    }

    public void AddHighScoreEntry(int score, string name)              // 점수 추가
    {
        // 생성
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name };

        // 로드
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        // 추가
        highScores.highScoreEntryList.Add(highScoreEntry);

        // 저장
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();

        int playerRank = 0;

        for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScores.highScoreEntryList.Count; j++)
            {
                if (highScores.highScoreEntryList[j].score > highScores.highScoreEntryList[i].score)
                {
                    HighScoreEntry temp = highScores.highScoreEntryList[i];
                    highScores.highScoreEntryList[i] = highScores.highScoreEntryList[j];
                    highScores.highScoreEntryList[j] = temp;
                }
            }

            if (highScores.highScoreEntryList[i].score == score && highScores.highScoreEntryList[i].name == name)
            {
                playerRank = i + 1; // 플레이어의 순위
            }
        }

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        transformList = new List<Transform>();
        foreach (HighScoreEntry ScoreEntry in highScores.highScoreEntryList)
        {
            CreateHighScoreTransform(ScoreEntry, rank, transformList);
            MyRankingView(highScoreEntry, playerRank);
        }
    }

    private void DeleteScore()      // 초기화
    {
        PlayerPrefs.DeleteKey("highScoreTable");

        HighScores highScores = new HighScores { highScoreEntryList = highScoreList };
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highScoreTable"));
    }

    private void MyRankingView(HighScoreEntry highScoreEntry, int highScore)
    {
        TextMeshProUGUI rankText = myRank.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = myRank.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = myRank.GetChild(2).GetComponent<TextMeshProUGUI>();

        int rankNum = highScore;
        int score = highScoreEntry.score;
        string name = highScoreEntry.name;
        rankText.text = rankNum.ToString();
        nameText.text = score.ToString();
        scoreText.text = name;
    }

    public void AddScore()
    {
        AddHighScoreEntry(GameManager.gameManager.score, GameManager.gameManager.studentId);
    }
}
