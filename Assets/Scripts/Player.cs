using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class Player : MonoBehaviour {
	private bool shopOppened = false;

	private int selectedGame = 0; //wich game will load on Minigame Scene

	private int score;

	private int money;

	private ISet<int> questionsNoRepeat = new HashSet<int>();

	private GameObject[] livesSpriteArr;
	private int maxLives = 3;
	private int lives;
	//private int lastLives;

	public Sprite fullHeartSprite;
	public Sprite emptyHeartSprite;

	public GameObject scoreNotif; // notification that will show the game result
	public Sprite[] scoreSprites;

	private static Player playerInstance;

	public AudioClip scorePopUp;

	void Start()
	{
		lives = maxLives;
		livesSpriteArr = new GameObject[maxLives];
	}

	void Awake()
	{
		//this object will never be destroyed
		DontDestroyOnLoad(this);
		//if duplicated exists
		if (playerInstance == null)
		{
			playerInstance = this; //make this one the main one
		}
		else
		{
			//Destroy others
			Destroy(gameObject);
		}

	}

	void Update()
	{
		Screen.SetResolution(1080, 1920, true, 120);
		updateLives();
	}

	void updateLives()
	{
		if (GameObject.Find("Heart1") != null)
		{
			//Money to UI
			GameObject.Find("DineroUI").GetComponent<Text>().text = money.ToString();

			//Score to UI
			GameObject.Find("Score").GetComponent<Text>().text = score.ToString();

			//Detect Life UI when available
			livesSpriteArr[0] = GameObject.Find("Heart1");
			livesSpriteArr[1] = GameObject.Find("Heart2");
			livesSpriteArr[2] = GameObject.Find("Heart3");

			for (int x = 0; x < maxLives; x++)
			{
				if (x < lives)
				{
					livesSpriteArr[x].GetComponent<Image>().sprite = fullHeartSprite; //sum new lives
				}
				else
				{
					livesSpriteArr[x].GetComponent<Image>().sprite = emptyHeartSprite; //subtract lives
				}
			}
		}
	}
	public void notifyScore(string result) // just text result (Draw or no more space for lives)
	{
		scoreNotif.GetComponentInChildren<Image>().enabled = false;

		if (!scoreNotif.activeSelf) //Runs the Score Apply just once
		{
			Invoke("nextScene", 3f);
			GetComponent<AudioSource>().PlayOneShot(scorePopUp);
			scoreNotif.GetComponentInChildren<Text>().text = result;
		}

		scoreNotif.SetActive(true);
	}

	public void notifyScore(string result, int value) //
	{
		string text ="";
		Image setImage = scoreNotif.GetComponentInChildren<Image>();
		setImage.enabled = true;

		if (!scoreNotif.activeSelf) //Score Apply just once
        {
			Invoke("nextScene", 3f);
			GetComponent<AudioSource>().PlayOneShot(scorePopUp);

			if (result.ToLower() == "score") //score increase
			{
				score += value;
				text = "Has ganado ";
				setImage.sprite = scoreSprites[0];
			}
			else if (result.ToLower() == "money") //money increase
			{
				money += value;
				text = "Has ganado ";
				setImage.sprite = scoreSprites[2];
			}
			else if (result.ToLower() == "life") //life decrease
			{
				lives += value;
				text = "Has perdido ";
				value = Mathf.Abs(value) * (1); //once applied, become number to positive
				setImage.sprite = scoreSprites[1];
			}
			else if (result.ToLower() == "lifegain") //life increase
			{
				lives += value;
				text = "Has ganado ";
				setImage.sprite = scoreSprites[1];
			}

			scoreNotif.GetComponentInChildren<Text>().text = text + value.ToString();
		}
		scoreNotif.SetActive(true);
	}

	void nextScene()
	{
		scoreNotif.SetActive(false);
		SceneManager.LoadScene("Escena1", LoadSceneMode.Single);
	}
	public int getLives()
	{
		return lives;
	}

	public void setLives(int lives)
	{
		this.lives += lives;
	}

	public int getMoney()
	{
		return money;
	}

	public void setMoney (int money)
	{
		this.money += money;
	}

	public int getSelectedGame()
    {
		return selectedGame;
    }

	public void setSelectedGame(int selectedGame)
    {
		this.selectedGame = selectedGame;
    }


	public bool getShopOppened()
    {
		return shopOppened;
    }

	public void setShopOppened(bool shopOppened)
    {
		this.shopOppened = shopOppened;
    }

	public bool isQuestionRepeated(int num)
    {
		if (questionsNoRepeat.Contains(num)) return true; //If question has been displayed before
        else //If not, add the question to the displayed answers list
        {
			questionsNoRepeat.Add(num);
			return false;
        }
    }
}
