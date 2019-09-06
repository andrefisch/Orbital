using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyTracker : MonoBehaviour
{
    // Enemy Stuff
    public int enemyStartDistance;
    public int cometFreq;
    public int orangeFreq;
    public float averageEnemySpeed;
    public float enemySpeedRange;
    public int maxEnemies;
    public int moreEnemiesInterval;

    // Timer stuff
    public float timer;
    public float spawnInterval;

    // Enemies
    int enemiesSpawned;
    public GameObject redEnemy;
    public GameObject blueEnemy;
    public GameObject greenEnemy;
    public GameObject orangeEnemy;
    public GameObject goldComet;

    // Game Stuff
    private GameObject player;
    private PlayerController playerController;
    private GameObject canvas;
    private GameMenus gameMenus;
    public GameObject loseCanvas;
    public GameObject scoreEntry;
    public InputField field;
    public string inputName;
    public Text loseText;
    public Text gameText;
    public bool arcadeMode;
    public bool masochistMode;
    public int score;
    public int numScores;
    public bool gameOver;
    public bool scoreAdded;
    public float gameOverTimer;

    
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        scoreAdded = false;
        enemiesSpawned = 0;

        player = GameObject.Find("Player1");
        playerController = player.GetComponent<PlayerController>();

        score = 0;
        enemyStartDistance = 20;
        Time.timeScale = 1f;

        gameText.enabled = true;
        loseCanvas.SetActive(false);
        scoreEntry.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
        if (arcadeMode)
        {
            maxEnemies = (score / moreEnemiesInterval) + 1;
        }
        else if (masochistMode)
        {
            maxEnemies = ((score + 43) / moreEnemiesInterval + 1);
        }

        //spawnInterval = 2f / maxEnemies;
        // if there are ever less enemies on the board than there are supposed to be, create a new one
        if (FindAllEnemies() < maxEnemies && timer >= spawnInterval)
        {
            //Debug.Log("Spawning a new enemy. Timer says " + timer);
            SpawnEnemy(0, RandomSpotOnWall());
            timer = 0;
        }
        // Release enemies on the interval of 1/maxEnemies so it increases over time
        // But doesn't release multiple enemies at once
        timer += Time.deltaTime;
        if (!gameOver)
        {
            gameText.text = SetGameText(TimeSurvived());
        }
        // If less than a second goes by after the loss, keep the game rolling so we can watch the final
        // explosion
        else if (gameOver && gameOverTimer < 0.75f)
        {
            gameOverTimer += Time.deltaTime;
        }
        // After a second pause the game and display the menu
        else
        {
            gameText.enabled = false;
            loseCanvas.SetActive(true);
            Time.timeScale = 0f;
            loseText.text = "GAME OVER!\nFinal stats:\n" + SetGameText(TimeSurvived());
            if (!scoreAdded)
            {
                // This will be used for the enemies survived field, but since the last enemy killed us, we decrement it by one
                enemiesSpawned = enemiesSpawned - 1;
                // This part should only happen once
                // - Check to see if the score is in the top 10
                // - If it is, enable the text input field with 5 letters to ask for name
                // - Then add score
                scoreAdded = true;
                if (arcadeMode)
                {
                    if (IsHighScore("Arcade", score))
                    {
                        scoreEntry.SetActive(true);
                    }
                }
                else //if (masochistMode)
                {
                    if (IsHighScore("Masochist", score))
                    {
                        scoreEntry.SetActive(true);
                    }
                }
            }
        }
    }

    public void SpawnEnemy(int enemyType, Vector3 startPosition)
    {
        enemiesSpawned = enemiesSpawned + 1;
        int gold = 0;
        int orange = 0;

        // If enemy type is 0, choose randomly. Otherwise create selected type
        if (enemyType == 0)
        {
            // Choose a red, blue, or green enemy randomly
            // 1 = red
            // 2 = blue
            // 3 = green
            if (arcadeMode)
            {
                if (score <= moreEnemiesInterval * 2)
                {
                    enemyType = 1;
                }
                else if (score > moreEnemiesInterval * 2 && score <= moreEnemiesInterval * 4)
                {
                    enemyType = UnityEngine.Random.Range(1, 3);
                    gold = UnityEngine.Random.Range(1, cometFreq);
                }
                else //if (score > moreEnemiesInterval * 4 && score <= moreEnemiesInterval * 6)
                {
                    enemyType = UnityEngine.Random.Range(1, 4);
                    gold = UnityEngine.Random.Range(1, cometFreq);
                }
                /*
                else //if (score > moreEnemiesInterval * 6)
                {
                    enemyType = Random.Range(1, 4);
                    gold = Random.Range(1, cometFreq);
                    orange = Random.Range(1, orangeFreq);
                }
                */
            }
            else //if (masochistMode)
            {
                enemyType = UnityEngine.Random.Range(1, 4);
                gold = UnityEngine.Random.Range(1, cometFreq);
                //orange = Random.Range(1, orangeFreq);
            }
        }

        // it we get the orange enemy, send it. otherwise if we get the gold comet send it. otherwise send a normal enemy
        if (orange == 1 || enemyType == 5)
        {
            Instantiate(orangeEnemy, startPosition, Quaternion.identity);
        }
        else if (gold == 1 || enemyType == 4)
        {
            Instantiate(goldComet, startPosition, Quaternion.identity);
            // This one doesn't count as an enemy since it is helpful
            enemiesSpawned = enemiesSpawned - 1;
        }
        else if (enemyType == 1)
        {
            //Debug.Log("Created new Red Enemy at position " + startPosition);
            CreateNewEnemy(redEnemy, startPosition);
        }
        else if (enemyType == 2)
        {
            //Debug.Log("Created new Blue Enemy at position " + startPosition);
            CreateNewEnemy(blueEnemy, startPosition);
        }
        else if (enemyType == 3)
        {
            //Debug.Log("Created new Green Enemy at position " + startPosition);
            CreateNewEnemy(greenEnemy, startPosition);
        }
    }

    // Choose a random spot on a random wall
    // Usually for the enemy or comets to spawn from or go to
    public Vector3 RandomSpotOnWall()
    {
        int startWall;
        Vector3 startPosition;
        // Choose which wall it starts on
        // 1 = right 
        // 2 = top 
        // 3 = left
        // 4 = bottom 
        startWall = UnityEngine.Random.Range(1, 5);
        if (startWall == 1)
        {
            startPosition = new Vector3(enemyStartDistance, UnityEngine.Random.Range(-enemyStartDistance, enemyStartDistance), 0);
        }
        else if (startWall == 2)
        {
            startPosition = new Vector3(UnityEngine.Random.Range(-enemyStartDistance, enemyStartDistance), enemyStartDistance, 0);
        }
        else if (startWall == 3)
        {
            startPosition = new Vector3(-enemyStartDistance, UnityEngine.Random.Range(-enemyStartDistance, enemyStartDistance), 0);
        }
        else //if (startWall == 4)
        {
            startPosition = new Vector3(UnityEngine.Random.Range(-enemyStartDistance, enemyStartDistance), -enemyStartDistance, 0);
        }
        return startPosition;
    }

    public string TimeSurvived()
    {
        string minutes = Mathf.Floor(Time.timeSinceLevelLoad / 60).ToString("0");
        string seconds = (Time.timeSinceLevelLoad % 60).ToString("00");
        return minutes + ":" + seconds;
    }

	public string SetGameText(string timeSurvived)
	{
		return "Score: " + score.ToString() + "\nTime Survived: " + timeSurvived + "\nEnemies Survived: " + enemiesSpawned;
	}
    
    public void CreateNewEnemy(GameObject enemy, Vector3 startPosition)
    {
        if (FindAllEnemies() < maxEnemies)
        {
            Instantiate(enemy, startPosition, Quaternion.identity);
        }
    }

    public int FindAllEnemies()
    {
        int reds = GameObject.FindGameObjectsWithTag("Red").Length;
        int blues = GameObject.FindGameObjectsWithTag("Blue").Length;
        int greens = GameObject.FindGameObjectsWithTag("Green").Length;
        int oranges = GameObject.FindGameObjectsWithTag("Orange").Length;
        return reds + blues + greens + oranges;
    }

    // Load top x scores and get the lowest one
    public bool IsHighScore(string gameMode, int score)
    {
        gameMode = gameMode + "HighScores";
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

        // if there are any high scores in the table compare the last score to the lowest score already in the table
        if (highscores.highscoreEntryList.Count > numScores)
        {
            // Sort
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
            int scoreToBeat = highscores.highscoreEntryList[highscores.highscoreEntryList.Count - 1].score;
            return score > scoreToBeat;
        }
        // If there are no scores in the table return true
        else
        {
            return true;
        }

    }

    public void AddHighscoreEntry(string gameMode, int score, int enemies, string time, string name) 
    {
        gameMode = gameMode + "HighScores";
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString(gameMode);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, enemies = enemies, time = time, name = name};
        // If there isn't a high score list yet, make one
        if (highscores == null) 
        {
            // There's no stored table, initialize
            highscores = new Highscores() 
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }
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

    public void SubmitScore()
    {
        inputName = field.text.ToUpper();
        if (arcadeMode)
        {
            AddHighscoreEntry("Arcade", score, enemiesSpawned, TimeSurvived(), inputName);
            SceneManager.LoadScene("HighScores");
        }
        if (masochistMode)
        {
            Debug.Log("Entering high score for MASOCHIST mode");
            AddHighscoreEntry("Masochist", score, enemiesSpawned, TimeSurvived(), inputName);
            SceneManager.LoadScene("HighScores");
        }
    }
}
