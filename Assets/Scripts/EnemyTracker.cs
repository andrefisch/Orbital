using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTracker : MonoBehaviour
{
    public int score;

    // Enemy Stuff
    public int enemyCount;
    public int enemyStartDistance;
    public float averageEnemySpeed;
    public float enemySpeedRange;
    public int maxEnemies;
    public int moreEnemiesInterval;

    // Timer stuff
    public float timer;
    public float spawnInterval;

    // Enemies
    public GameObject redEnemy;
    public GameObject blueEnemy;
    public GameObject greenEnemy;

    // Game Stuff
    private GameObject player;
    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player1");
        playerController = player.GetComponent<PlayerController>();

        score = 0;
        enemyCount = 0;
        enemyStartDistance = 20;
    }

    // Update is called once per frame
    void Update()
    {
        // Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        maxEnemies = (score / moreEnemiesInterval) + 1;
        //spawnInterval = 2f / maxEnemies;
        // if there are ever less enemies on the board than there are supposed to be, create a new one
        if (enemyCount < maxEnemies && timer >= spawnInterval)
        {
            Debug.Log("Spawning a new enemy. Timer says " + timer);
            SpawnEnemy();
            timer = 0;
        }
        // Release enemies on the interval of 1/maxEnemies so it increases over time
        // But doesn't release multiple enemies at once
        timer += Time.deltaTime;
    }

    void SpawnEnemy()
    {
        enemyCount = enemyCount + 1;
        Vector3 startPosition;
        int enemyType;

        startPosition = RandomSpotOnWall();

        // Choose a red or blue enemy randomly
        // 1 = red
        // 2 = blue
        // 3 = green
        if (score <= moreEnemiesInterval * 2)
        {
            enemyType = 1;
        }
        else if (score > moreEnemiesInterval * 2 && score <= moreEnemiesInterval * 4)
        {
            enemyType = Random.Range(1, 3);
        }
        else //if (score > 30)
        {
            enemyType = Random.Range(1, 4);
        }

        if (enemyType == 1)
        {
            //Debug.Log("Created new Red Enemy at position " + startPosition);
            Instantiate(redEnemy, startPosition, Quaternion.identity);
        }
        else if (enemyType == 2)
        {
            //Debug.Log("Created new Blue Enemy at position " + startPosition);
            Instantiate(blueEnemy, startPosition, Quaternion.identity);
        }
        else if (enemyType == 3)
        {
            //Debug.Log("Created new Green Enemy at position " + startPosition);
            Instantiate(greenEnemy, startPosition, Quaternion.identity);
        }
        playerController.SetCountText();
    }

    public Vector3 RandomSpotOnWall()
    {
        int startWall;
        Vector3 startPosition;
        // Choose which wall it starts on
        // 1 = right 
        // 2 = top 
        // 3 = left
        // 4 = bottom 
        startWall = Random.Range(1, 5);
        if (startWall == 1)
        {
            startPosition = new Vector3(enemyStartDistance, Random.Range(-enemyStartDistance, enemyStartDistance), 0);
        }
        else if (startWall == 2)
        {
            startPosition = new Vector3(Random.Range(-enemyStartDistance, enemyStartDistance), enemyStartDistance, 0);
        }
        else if (startWall == 3)
        {
            startPosition = new Vector3(-enemyStartDistance, Random.Range(-enemyStartDistance, enemyStartDistance), 0);
        }
        else //if (startWall == 4)
        {
            startPosition = new Vector3(Random.Range(-enemyStartDistance, enemyStartDistance), -enemyStartDistance, 0);
        }
        return startPosition;
    }
}
