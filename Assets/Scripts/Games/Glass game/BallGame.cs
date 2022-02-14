using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGame : MonoBehaviour
{
    public GameObject[] glasses = new GameObject[3]; //contains the 3 glass objects
    private Glass[] selGlasses = new Glass[2]; //contains the selected ones to make the movement

    private bool movementReady = true; //will commit next moves
    private bool gameEnded = false;

    private int times; //how much times will move glasses (RANDOM)
    private int count = 0; // will count how much moves has done
    private int speed; //movement speed (RANDOM)

    private bool glassSelectedByUser; //prevents being able to select more than one glass

    void Start()
    {
        times = Random.Range(16, 28); 
        speed = Random.Range(16, 21);

        int index = Random.Range(0, 3); //wich glass will contain the ball
        glasses[index].GetComponent<Glass>().setIsBallInside(true);
        glasses[index].GetComponent<Glass>().setPosition(index);
    }

    public void startGameButton()
    {
        for (int x = 0; x < glasses.Length; x++)//sets all the starting data for the glasses (positions, speed movements, etc)
        {
            glasses[x].GetComponent<Glass>().setPosition(x);
            glasses[x].GetComponent<Glass>().setReady(true);
            glasses[x].GetComponent<Glass>().setSpeed(speed);
            glasses[x].GetComponent<Glass>().setBallGame(this);
        }
        movementReady = false; //next movement can be done
        Destroy(GameObject.Find("StartBallGameButton"));
    }

    void Update()
    {
        if (!movementReady && count < times) //if next movement can be done
        {
            selectGlasses(); //make new moves data
        }else
        {
            if (count == times)
            {
                GetComponent<AudioSource>().mute = true; //end sound
                gameEnded = true; //game end
            }
            else if (glasses[0].GetComponent<Glass>().getIsGrounded()) moveGlasses(); //if the glasses are grounded
        }
        
    }

    void selectGlasses()
    {
        int[] randomGlassArr = new int[2]; //whill save 2 of the 3 glasses
        int arrIndex = 0; //used as index

        while (arrIndex < 2)
        {
            int result = Random.Range(0, 3); // Selects one randome glass

            if (result != randomGlassArr[0]) // cannot repeat glass
            {
                selGlasses[arrIndex] = glasses[result].GetComponent<Glass>(); //saves glass
                randomGlassArr[arrIndex] = result;
                selGlasses[arrIndex].setCurrent(0); //sets waypoints at starting position
                arrIndex += 1;//glass is prepared for movement
            }
        }

        selGlasses[0].setTarget(selGlasses[1].getPosition()); //target for glass 0
        selGlasses[0].setReached(false); //restart waypoints
        selGlasses[1].setTarget(selGlasses[0].getPosition()); //target for glass 1
        selGlasses[1].setReached(false); //restart waypoints
        count++;
        movementReady = true; // no more movements until the actual one ends
    }

    void moveGlasses()
    {
        //move glasses
        selGlasses[0].moveGlass();
        selGlasses[1].moveGlass();
        GetComponent<AudioSource>().mute = false; //start sound
        if (selGlasses[0].getReached() && selGlasses[1].getReached()) //once glasses have reached its target
        {
            movementReady = false;
        }
    }

    public void endGame(Glass glass)
    {
        if(!glassSelectedByUser && gameEnded) //once the player has selected a glass
        {
            glassSelectedByUser = true;
            glass.setGlassUp(true);//glass up, and shows if the ball is inside
        }
    }
}
