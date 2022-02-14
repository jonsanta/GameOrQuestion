using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Glass : MonoBehaviour
{
    private BallGame ballGame; //will contain the game main controller BallGame script
    public GameObject ball; //ball ingame object

    private int position; //contains the actual position (0,1,2)
    private int target; //where will the glass move
    private int speed; //movement speed

    private bool isBallInside = false; //do the glass contain the ball?
    private bool ready = false; //once the start button is pressed this will be true (Glasses will take positions, etc..)
    private bool isGrounded = false; //is the glass touching the table?

    private int current = 0; //waypoint index

    private bool reached = false; //reached target
    private bool glassUp = false;

    void Update()
    {
        if (glassUp)
        {
            ball.GetComponent<MeshRenderer>().enabled = true; //enables the ball
            if (transform.position.y < 4f) transform.position += new Vector3(0, 1.5f, 0) * Time.deltaTime; //raises up the glass showing whats under
            else
            {
                if (isBallInside)
                    GameObject.Find("Player").GetComponent<Player>().notifyScore("Score", +10);
                else
                    GameObject.Find("Player").GetComponent<Player>().notifyScore("Life", -1);
            }
        }
        else if (ready) setGame(); //prepares the game
        else if (isBallInside) ball.transform.position = GameObject.Find("BallPosition" + position).transform.position;
    }

    public void moveGlass()
    {
        string[] moveTo = new string[3];

        for (int x = 1; x < 3; x++)//looking for the waypoints to the target
        {
            moveTo[x - 1] = position.ToString() + x.ToString() + target.ToString();
        }
        moveTo[2] = target.ToString();

        if (current >= moveTo.Length) //if reaches the array length ---> reaches Target
        {
            position = target; // new position = target
            if (isBallInside) ball.transform.position = GameObject.Find("BallPosition" + position).transform.position; //if ball inside, new position for ball
            reached = true;
        }
        else if (Vector3.Distance(transform.position, GameObject.Find(moveTo[current]).transform.position) < 0.1f) //if reaches the waypoint
        {
            current++; //new waypoint
        }

        if (current < moveTo.Length) //keep moving to next waypoint
        {
            transform.position = Vector3.MoveTowards(transform.position, GameObject.Find(moveTo[current]).transform.position, Time.deltaTime * speed);
        }
    }

    void setGame()
    {
        if (transform.position.y < 3.32f)
        {
            isGrounded = true; //if the glass is down
            ball.GetComponent<MeshRenderer>().enabled = false; // disables ball render
            ready = false;
        }
        else transform.position -= new Vector3(0, 1.5f, 0) * Time.deltaTime; //glass down at the start of the game
    }

    void OnMouseDown()
    {
        ballGame.endGame(this); //once the game has ended ----> user will select one of the 3 glasses ---> (this) will be selected as the chosen one
    }

    public int getPosition ()
    {
        return position;
    }

    public void setPosition(int position)
    {
        this.position = position;
    }

    public void setIsBallInside(bool isBallInside)
    {
        this.isBallInside = isBallInside;
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public void setReady(bool ready)
    {
        this.ready = ready;
    }

    public void setTarget(int target)
    {
        this.target = target;
    }

    public void setBallGame(BallGame ballGame)
    {
        this.ballGame = ballGame;
    }

    public void setCurrent(int current)
    {
        this.current = current;
    }

    public void setSpeed(int speed)
    {
        this.speed = speed;
    }

    public bool getReached()
    {
        return reached;
    }

    public void setReached(bool reached)
    {
        this.reached = reached;
    }

    public void setGlassUp(bool glassUp)
    {
        this.glassUp = glassUp;
    }
}
