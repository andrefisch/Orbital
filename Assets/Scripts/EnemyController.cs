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
            enemyTracker.enemyCount = enemyTracker.enemyCount - 1;
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
                Destroy(other.gameObject);
                enemyTracker.enemyCount = enemyTracker.enemyCount - 1;
                Explode(Color.red);
            }
            // Otherwise it doubles the speed
            else if (gameObject.CompareTag("Blue"))
            {
                EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
                enemyController.speed = enemyController.speed * 2f;
            }
            else if (gameObject.CompareTag("Green"))
            {
                EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
                enemyController.speed = enemyController.speed * 2f;
            }
        }
        if (other.gameObject.CompareTag("Blue"))
        {
            // If a blue hits a blue it explodes
            if (gameObject.CompareTag("Blue"))
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
                enemyTracker.enemyCount = enemyTracker.enemyCount - 1;
                Explode(Color.blue);
            }
            // Otherwise it redirects it
            else if (gameObject.CompareTag("Red"))
            {
                GameObject gameManager = GameObject.Find("GameManager");
                EnemyTracker enemyTracker = gameManager.GetComponent<EnemyTracker>();
                int enemyStartDistance = enemyTracker.enemyStartDistance;
                other.gameObject.GetComponent<EnemyController>().endPosition = enemyTracker.RandomSpotOnWall();
            }
            else if (gameObject.CompareTag("Green"))
            {
                GameObject gameManager = GameObject.Find("GameManager");
                EnemyTracker enemyTracker = gameManager.GetComponent<EnemyTracker>();
                int enemyStartDistance = enemyTracker.enemyStartDistance;
                other.gameObject.GetComponent<EnemyController>().endPosition = enemyTracker.RandomSpotOnWall();
            }
        }
        if (other.gameObject.CompareTag("Green"))
        {
            // If a green hits a green it explodes
            if (gameObject.CompareTag("Green"))
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
                enemyTracker.enemyCount = enemyTracker.enemyCount - 1;
                Explode(Color.green);
            }
            // Otherwise it sends it back
            else if (gameObject.CompareTag("Red"))
            {
                EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
                enemyController.speed = -enemyController.speed;
            }
            else if (gameObject.CompareTag("Blue"))
            {
                EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
                enemyController.speed = -enemyController.speed;
            }
        }
	}
    bool IsOutsideBoard()
    {
        // Find the largest value of the end position (how far away the particle can go)
        float max = Mathf.Max(Mathf.Abs(endPosition.x), Mathf.Abs(endPosition.y));
        // If any of its current positions are greater than that number it has gone to far
        return (Mathf.Max(Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y)) >= max);
    }

    public GameObject Explode(Color color) 
    {
        GameObject go = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity);
	    ParticleSystem ps = go.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
		main.startColor = color;
        return go;
    }
}
