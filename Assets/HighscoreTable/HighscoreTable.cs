/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighscoreTable : MonoBehaviour 
{

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private string posText = "posText";
    private string scoreText = "scoreText";
    private string enemiesText = "enemiesText";
    private string timeText = "timeText";
    private string nameText = "nameText";

    public string tableName;
    public int numScores;

    private void Awake() 
    {
        ArcadeScores();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ArcadeScores()
    {
        tableName = "ArcadeHighScores";
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        GameObject[] scoreEntries = GameObject.FindGameObjectsWithTag("HighScoreEntry");
        foreach (GameObject entry in scoreEntries) 
        {
            Destroy(entry);
        }

        //PlayerPrefs.DeleteKey("ArcadeHighScores");
        string jsonString = PlayerPrefs.GetString(tableName);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) 
        {
            /*
            // Debug.Log("Initializing ARCADE table with default values...");
            int score = 50;
            AddHighscoreEntry(tableName, score, score * 10, "10:00", "CYRUS");
            score = 30;
            AddHighscoreEntry(tableName, score, score * 10, "6:00", "RIENZI");
            score = 40;
            AddHighscoreEntry(tableName, score, score * 10, "8:00", "TIM");
            score = 10;
            AddHighscoreEntry(tableName, score, score * 10, "2:00", "ETHAN");
            score = 20;
            AddHighscoreEntry(tableName, score, score * 10, "4:00", "JAMIE");
            */
            // Reload
            jsonString = PlayerPrefs.GetString(tableName);
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }


        // Sort entry list by Score
        if (highscores != null)
        {
            // Debug.Log("Sorting the ARCADE entries");
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++) 
            {
                for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) 
                {
                    if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) 
                    {
                        // Swap
                        HighscoreEntry tmp = highscores.highscoreEntryList[i];
                        highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                        highscores.highscoreEntryList[j] = tmp;
                    }
                }
            }

            // Only keep first X entries
            List<HighscoreEntry> listy = new List<HighscoreEntry>();
            for (int i = 0; i < numScores && i < highscores.highscoreEntryList.Count; i++)
            {
                listy.Add(highscores.highscoreEntryList[i]);
            }

            highscores.highscoreEntryList = listy;

            highscoreEntryTransformList = new List<Transform>();
            foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) 
            {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }
        }
    }

    public void MasochistScores()
    {
        tableName = "MasochistHighScores";
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        GameObject[] scoreEntries = GameObject.FindGameObjectsWithTag("HighScoreEntry");
        foreach (GameObject entry in scoreEntries) 
        {
            Destroy(entry);
        }

        //PlayerPrefs.DeleteKey(tableName);
        string jsonString = PlayerPrefs.GetString(tableName);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) 
        {
            // Debug.Log("Initializing MASOCHIST table with default values...");
            /*
            int score = 20;
            AddHighscoreEntry(tableName, score, score * 10, "2:00", "CYRUS");
            score = 12;
            AddHighscoreEntry(tableName, score, score * 10, "1:20", "RIENZI");
            score = 16;
            AddHighscoreEntry(tableName, score, score * 10, "1:40", "TIM");
            score = 4;
            AddHighscoreEntry(tableName, score, score * 10, "0:30", "ETHAN");
            score = 8;
            AddHighscoreEntry(tableName, score, score * 10, "1:00", "JAMIE");
            */
            // Reload
            jsonString = PlayerPrefs.GetString(tableName);
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }

        if (highscores != null)
        {
            // Debug.Log("Sorting the MASOCHIST entries");
            // Sort entry list by Score
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++) 
            {
                for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) 
                {
                    if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) 
                    {
                        // Swap
                        HighscoreEntry tmp = highscores.highscoreEntryList[i];
                        highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                        highscores.highscoreEntryList[j] = tmp;
                    }
                }
            }

            // Only keep first X entries
            List<HighscoreEntry> listy = new List<HighscoreEntry>();
            for (int i = 0; i < numScores && i < highscores.highscoreEntryList.Count; i++)
            {
                listy.Add(highscores.highscoreEntryList[i]);
            }

            highscores.highscoreEntryList = listy;

            highscoreEntryTransformList = new List<Transform>();
            foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) 
            {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) 
    {
        float templateHeight = 31f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) 
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find(posText).GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;
        entryTransform.Find(scoreText).GetComponent<Text>().text = score.ToString();

        int enemies = highscoreEntry.enemies;
        entryTransform.Find(enemiesText).GetComponent<Text>().text = enemies.ToString();

        string time = highscoreEntry.time;
        entryTransform.Find(timeText).GetComponent<Text>().text = time;

        string name = highscoreEntry.name;
        entryTransform.Find(nameText).GetComponent<Text>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        
        // Highlight First
        if (rank == 1) 
        {
            entryTransform.Find(posText).GetComponent<Text>().color = Color.green;
            entryTransform.Find(scoreText).GetComponent<Text>().color = Color.green;
            entryTransform.Find(enemiesText).GetComponent<Text>().color = Color.green;
            entryTransform.Find(timeText).GetComponent<Text>().color = Color.green;
            entryTransform.Find(nameText).GetComponent<Text>().color = Color.green;
        }

        // Set tropy
        switch (rank) 
        {
            default:
                entryTransform.Find("trophy").gameObject.SetActive(false);
                break;
            case 1:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color(1.0f, 0.824f, 0.0f, 1);
                break;
            case 2:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color(0.776f, 0.776f, 0.776f, 1);
                break;
            case 3:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color(0.718f, 0.435f, 0.337f, 1);
                break;

        }

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(string gameMode, int score, int enemies, string time, string name) 
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, enemies = enemies, time = time, name = name};
        
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString(gameMode);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) 
        {
            // There's no stored table, initialize
            highscores = new Highscores() 
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString(gameMode, json);
        PlayerPrefs.Save();
    }

    private class Highscores 
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    /*
     * Represents a single High score entry
     * */
    [System.Serializable] 
    private class HighscoreEntry 
    {
        public int score;
        public int enemies;
        public string time;
        public string name;
    }
}
