using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementButtons : MonoBehaviour
{
	public float revolutionSpeed;
	public float inOutSpeed;
    private Vector3 zAxis;
   
    // Game Stuff
    private GameObject player1;
    private PlayerController playerController1;
    private GameObject player2;
    private PlayerController playerController2;
    private bool down;
    private bool up;
    private bool right;
    private bool left;
    private float moveRight;
    private float moveLeft;
    private float moveUp;
    private float moveDown;
    private float moveHorizontal;
    private float moveVertical;

    void Start()
    {
        player1 = GameObject.Find("Player1");
        playerController1 = player1.GetComponent<PlayerController>();
        player2 = GameObject.Find("Player2");
        playerController2 = player2.GetComponent<PlayerController>();
        zAxis = new Vector3(0, 0, 1);
        down = false;
        up = false;
        right = false;
        left = false;
        moveHorizontal = 0f;
        moveVertical = 0f;
    }
 
    void Update()
    {
        // Make the player accelerate instead of bringing them to full speed immediately
        //{{{
        if (!PlayerController.gameOver)
        {
            if (up)
            {
                moveUp += 0.05f;
            }
            else 
            {
                moveUp -= 0.05f;
            }
            if (down)
            {
                moveDown -= 0.05f;
            }
            else 
            {
                moveDown += 0.05f;
            }
            if (left)
            {
                moveLeft += 0.05f;
            }
            else 
            {
                moveLeft -= 0.05f;
            }
            if (right)
            {
                moveRight -= 0.05f;
            }
            else 
            {
                moveRight += 0.05f;
            }
            // If the magnitude of any speed is ever greater than 1 or less than 0 make those speeds 1 or 0
            if (moveUp > 1f)
            {
                moveUp = 1f;
            }
            else if (moveUp < 0f)
            {
                moveUp = 0f;
            }
            if (moveDown < -1f)
            {
                moveDown = -1f;
            }
            else if (moveDown > 0f)
            {
                moveDown = 0f;
            }
            if (moveRight > 1f)
            {
                moveRight = 1f;
            }
            else if (moveRight < 0f)
            {
                moveRight = 0f;
            }
            if (moveLeft < -1f)
            {
                moveLeft = -1f;
            }
            else if (moveLeft > 0f)
            {
                moveLeft = 0f;
            }
            //}}}
            moveHorizontal = moveLeft + moveRight;
            moveVertical = moveUp + moveDown;
            // IN OUT MOVEMENT WORKS PERFECTLY
            playerController1.transform.position = Vector3.MoveTowards(playerController1.transform.position, Vector3.zero, inOutSpeed * -moveVertical);
            playerController2.transform.position = Vector3.MoveTowards(playerController2.transform.position, Vector3.zero, inOutSpeed * -moveVertical);
            // ROTATIONAL MOVEMENT WORKS PERFECTLY
            if (Time.deltaTime > 0)
            {
                playerController1.transform.RotateAround(Vector3.zero, zAxis, revolutionSpeed * moveHorizontal);
                playerController2.transform.RotateAround(Vector3.zero, zAxis, revolutionSpeed * moveHorizontal);
            }

            // Turn off all movement flags when we unclick everything
            if (Input.GetMouseButtonUp(0))
            {
                down = false;
                up = false;
                right = false;
                left = false;
            }
        }
        //Debug.Log(moveVertical);
    }

    public void UpButton()
    {
        Debug.Log("Clicking on UP");
        up = true;
    }

    public void DownButton()
    {
        Debug.Log("Clicking on DOWN");
        down = true;
    }

    public void RightButton()
    {
        Debug.Log("Clicking on RIGHT");
        right = true;
    }

    public void LeftButton()
    {
        Debug.Log("Clicking on LEFT");
        left = true;
    }
}
