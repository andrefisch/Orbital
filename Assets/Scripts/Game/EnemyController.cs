using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed;

    public GameObject explosion;

    private GameObject gameManager;
    private EnemyTracker enemyTracker;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        enemyTracker = gameManager.GetComponent<EnemyTracker>();

        startPosition = GetComponent<Transform>().position;        
        endPosition = -startPosition;
        float enemySpeedRange = enemyTracker.enemySpeedRange;
        speed = enemyTracker.averageEnemySpeed + Random.Range(-enemySpeedRange, enemySpeedRange);

        if (gameObject.CompareTag("Gold"))
        {
            GameObject gameManager = GameObject.Find("GameManager");
            EnemyTracker enemyTracker = gameManager.GetComponent<EnemyTracker>();
            gameObject.GetComponent<EnemyController>().speed = gameObject.GetComponent<EnemyController>().speed * 2;
            //gameObject.GetComponent<EnemyController>().endPosition = enemyTracker.RandomSpotOnWall();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosition, speed);
        //if (Mathf.Abs(transform.position.x - endPosition.x) <= speed && Mathf.Abs(transform.position.y - endPosition.y) <= speed)
        // SEEMS TO WORK
        if (IsOutsideBoard())
        {
            Destroy(gameObject);
        }
    }

	void OnTriggerEnter2D(Collider2D other) 
	{
        // Objects of the same color annihilate each other. Otherwise
        // Red   -   Random deflection
        // Blue  -   speed up
        // Green -   Go back
        if (other.gameObject.CompareTag("Red"))
        {
            // If a red hits itself it explodes
            if (gameObject.CompareTag("Red"))
            {
                Destroy(gameObject);
                Explode(Color.red, gameObject);
            }
            // Otherwise it doubles the speed
            else if (gameObject.CompareTag("Blue"))
            {
                AccelerateEnemy(2f, other.gameObject);
            }
            else if (gameObject.CompareTag("Green"))
            {
                AccelerateEnemy(2f, other.gameObject);
            }
            // If it hits a gold one destroy it but not the gold one
            else if (gameObject.CompareTag("Gold"))
            {
                AccelerateEnemy(2f, other.gameObject);
                Destroy(other.gameObject);
                Explode(Color.red, other.gameObject);
            }
            // Destroy the orange one and triple this one
            else if (gameObject.CompareTag("Orange"))
            {
                Destroy(other.gameObject);
                Color orange = new Color(1.0f, 0.549f, 0.0f, 1);
                Explode(orange, other.gameObject);
                enemyTracker.SpawnEnemy(1, gameObject.transform.position);
                enemyTracker.SpawnEnemy(1, gameObject.transform.position);
            }
        }
        if (other.gameObject.CompareTag("Blue"))
        {
            // If a blue hits a blue it explodes
            if (gameObject.CompareTag("Blue"))
            {
                Destroy(gameObject);
                Explode(Color.blue, gameObject);
            }
            // Otherwise it sends it back
            else if (gameObject.CompareTag("Red"))
            {
                AccelerateEnemy(-1f, other.gameObject);
            }
            else if (gameObject.CompareTag("Blue"))
            {
                AccelerateEnemy(-1f, other.gameObject);
            }
            // If it hits a gold one destroy it but not the gold one
            else if (gameObject.CompareTag("Gold"))
            {
                AccelerateEnemy(1.2f, other.gameObject);
                RedirectEnemy(other.gameObject);
                Destroy(other.gameObject);
                Explode(Color.blue, other.gameObject);
            }
        }
        if (other.gameObject.CompareTag("Green"))
        {
            // If a green hits a green it explodes
            if (gameObject.CompareTag("Green"))
            {
                Destroy(gameObject);
                Explode(Color.green, gameObject);
            }
            // Otherwise redirect it and accelerate a little
            else if (gameObject.CompareTag("Red"))
            {
                RedirectEnemy(other.gameObject);
                AccelerateEnemy(1.2f, other.gameObject);
            }
            else if (gameObject.CompareTag("Green"))
            {
                RedirectEnemy(other.gameObject);
                AccelerateEnemy(1.2f, other.gameObject);
            }
            // If it hits a gold one destroy it but not the gold one
            else if (gameObject.CompareTag("Gold"))
            {
                AccelerateEnemy(-1f, other.gameObject);
                Destroy(other.gameObject);
                Explode(Color.green, other.gameObject);
            }
        }
	}

    // Has the enemy gotten past the wall?
    bool IsOutsideBoard()
    {
        // Find the largest value of the end position (how far away the particle can go)
        float max = Mathf.Max(Mathf.Abs(endPosition.x), Mathf.Abs(endPosition.y));
        // If any of its current positions are greater than that number it has gone to far
        return (Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y)) >= max);
    }

    // Multiply enemy speed by multiplier
    void AccelerateEnemy(float multiplier, GameObject gameObject)
    {
        EnemyController enemyController = gameObject.GetComponent<EnemyController>();
        enemyController.speed = enemyController.speed * multiplier;
    }

    // Choose a new random end position for the enemy
    void RedirectEnemy(GameObject gameObject)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        EnemyTracker enemyTracker = gameManager.GetComponent<EnemyTracker>();
        gameObject.GetComponent<EnemyController>().endPosition = enemyTracker.RandomSpotOnWall();
    }

    public GameObject Explode(Color color, GameObject gameObject) 
    {
        GameObject go = (GameObject)Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
	    ParticleSystem ps = go.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
		main.startColor = color;
        return go;
    }
}
