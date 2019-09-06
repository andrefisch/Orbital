using UnityEngine;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float revolutionSpeed;
	public float inOutSpeed;
	public Text loseText;

	private Rigidbody2D rb2d;

    // Game Stuff
    private GameObject gameManager;
    private EnemyTracker enemyTracker;

    // Prefabs
    public GameObject pickup;
    public float pickupRange;
    public GameObject explosion;

    public static bool gameOver;

	// Use this for initialization
	void Start()
	{
        gameManager = GameObject.Find("GameManager");
        enemyTracker = gameManager.GetComponent<EnemyTracker>();

		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();

        gameOver = false;

        Invoke("CreateNewPickup", 2f);
	}

    void Update()
    {
        if (!gameOver)
        {
            float moveHorizontal = Input.GetAxis ("Horizontal");
            float moveVertical = Input.GetAxis ("Vertical");
            Vector3 zAxis = new Vector3(0, 0, 1);
            if (Time.deltaTime > 0)
            {
                transform.RotateAround(Vector3.zero, zAxis, revolutionSpeed * -moveHorizontal);
            }
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, inOutSpeed * -moveVertical);
        }
    }


	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{

	}

	//OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
	void OnTriggerEnter2D(Collider2D other) 
	{
		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag("PickUp")) 
		{
            //other.gameObject.transform.position = newPosition;
            Destroy(other.gameObject);
            Color gold = new Color(1.0f, 0.7f, 0.0f, 1);
            Explode(gold, other.gameObject);
            Invoke("CreateNewPickup", 2f);
            IncrementScore();
		}
        // These will kill you
        if (other.gameObject.CompareTag("Red") || other.gameObject.CompareTag("Blue") || other.gameObject.CompareTag("Green"))
        {
            Explode(Color.black, gameObject);
            Destroy(other.gameObject);
            if (other.gameObject.CompareTag("Red"))
            {
                Explode(Color.red, other.gameObject);
            }
            else if (other.gameObject.CompareTag("Blue"))
            {
                Explode(Color.blue, other.gameObject);
            }
            else if (other.gameObject.CompareTag("Green"))
            {
                Explode(Color.green, other.gameObject);
            }
            enemyTracker.gameOver = true;
            gameOver = true;
        }
        if (other.gameObject.CompareTag("Gold"))
        {
            Destroy(other.gameObject);
            Color gold = new Color(1.0f, 0.7f, 0.0f, 1);
            Explode(gold, other.gameObject);
            IncrementScore();
        }
	}

    void IncrementScore()
    {
        //Add one to the current value of our count variable.
        enemyTracker.score = enemyTracker.score + 1;
    }

    void CreateNewPickup()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");
        if (pickups.Length == 0)
        {
            float range = pickupRange;
            Vector3 newPosition;
            do
            {
                newPosition = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
            } while (Vector3.Distance(newPosition, GameObject.Find("Player1").transform.position) < 7 ||
                    Vector3.Distance(newPosition, GameObject.Find("Player2").transform.position) < 7 ||
                    Vector3.Distance(newPosition, gameManager.transform.position) > pickupRange);
            Instantiate(pickup, newPosition, Quaternion.identity);
        }
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
