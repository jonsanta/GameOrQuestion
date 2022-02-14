using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlackJack : MonoBehaviour {

	private Animator deck_anim; //deck animator
	public GameObject deck; //animated deck 

	public Transform cardInstantiatePosition; //card animation end position

	public Texture[] cards = new Texture[13]; //cards textures

	private int userPoints;
	private int botPoints;

	private int cardToInt; //index number of cards texture[]

	private bool newCardAsked = false; //card animation is running
	private bool moveCardToHand = false; //deck animation ended ---> card to hand movement on

	private bool botTurn = false; // if true, bot is playing

	private GameObject cardInstance; //temporal card data

	private int current= 1; // how much cards dealed for actual player

	private int[] numberOfCards = new int[2]; //how much cards have been dealed for each player 
	// [0] player, [1] bot
	void Start () {
		deck_anim = deck.GetComponentInParent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject.Find("BjScore").GetComponent<Text>().text = userPoints.ToString();
		GameObject.Find("BjScoreBot").GetComponent<Text>().text = botPoints.ToString();

		if (newCardAsked && !botTurn) cardAnimation("userPosition", ref userPoints); //run card deal for user
		else if(botTurn)
        {
			if (botPoints >= 17) // if bot has 17 or more points
			{
				numberOfCards[1] = current; //set dealed cards for bot
				evaluatePoints(); //end game
			}else
            {
				if (!newCardAsked) cardToInt = newCard(); //ask for new card

				cardAnimation("botPosition", ref botPoints); //run card deal animation for bot
			}
		}
	}

	int newCard() //new card
    {
		int result = Random.Range(0, cards.Length); //generate random int for texture
		Material[] mats = deck.GetComponent<MeshRenderer>().materials;
		mats[1].mainTexture = cards[result]; //set texture
		deck.GetComponent<MeshRenderer>().materials = mats;

		//start dealing animation
		deck_anim.SetTrigger("NewCard");
		GetComponent<AudioSource>().PlayDelayed(0.75f);
		newCardAsked = true;

		return result; //return texture[] index
    }

	int cardValues(int value) //give values to texture[] index
    {
		if (value == 0)
		{
			if(!botTurn) enableAceButtons("AceAs11", "AceAs1",  true);//ask for ace value
			else
            {
				int temp = botPoints;
				if (temp + 11 < 17) return 11;
				else return 1;
            }
			return value;
		}
		else if (value > 9) return 10; //figures = 10 points
		else return value + 1; //index starts as 0 -----> Texture[7] ---> 8 points
    }

	void enableAceButtons(string str, string str2, bool flag) //enable Ace value select buttons
    {
		string[] arr = {str, str2};
		for(int x = 0; x < arr.Length; x++)
        {
			GameObject button = GameObject.Find(arr[x]);
			button.GetComponent<Image>().enabled = flag;
			button.GetComponent<Button>().enabled = flag;
			button.GetComponentInChildren<Text>().enabled = flag;
		}
		newCardAsked = true;
	}
	void cardAnimation(string targetName, ref int score)// target name= switch hand position
	{//if card animation is running
		if (deck_anim.GetCurrentAnimatorStateInfo(0).IsName("CardOut") && cardInstance == null)
		{
			Material[] mat = new Material[2]; // create new unlinked material
			Material cardMat = new Material(Shader.Find("Diffuse"));
			cardMat.mainTexture = cards[cardToInt];
			//instantiate new card copy
			cardInstance = (GameObject)Instantiate(deck, cardInstantiatePosition.position, deck.transform.rotation, deck.transform.parent);
			cardInstance.transform.localScale = deck.transform.localScale;

			MeshRenderer ren = cardInstance.GetComponent<MeshRenderer>();
			mat = ren.materials;
			mat[1] = cardMat;
			ren.materials = mat; //set new unlinek material
		}
		else if(deck_anim.GetCurrentAnimatorStateInfo(0).IsName("CardToHand"))
		{
			moveCardToHand = true; //move card to hand
			GetComponent<AudioSource>().Play();
		}
		else if (moveCardToHand)
        {
			Transform target = GameObject.Find(targetName+current).transform; //set hand position

			if (Vector3.Distance(cardInstance.transform.position, target.position) < 0.1f) //if reaches the waypoint
			{
				current++; //new waypoint
				cardInstance = null; //new card can be handled
				newCardAsked = false; 
				moveCardToHand = false;
				score += cardValues(cardToInt); //take real value from texture[] index
			}
			else cardInstance.transform.position = Vector3.MoveTowards(cardInstance.transform.position, target.position, Time.deltaTime * 20);
		}

		if (userPoints > 21) //if user boosted
		{
			Player player = GameObject.Find("Player").GetComponent<Player>();
			player.notifyScore("Life", -1);
		}
	}

	void evaluatePoints()
    {
		Player player = GameObject.Find("Player").GetComponent<Player>();

		if (botPoints > 21 || userPoints > botPoints) // user wins
			player.notifyScore("Score", +10);
		else if(botPoints > userPoints) //user loses
			player.notifyScore("Life", -1);
		else if (botPoints == 21 && userPoints == 21) //point draw
		{
			if (numberOfCards[0] == 2 && numberOfCards[1] != 2) // user blackjack
				player.notifyScore("Score", +10);
			else if (numberOfCards[1] == 2 && numberOfCards[0] != 2) //bot blackjack
				player.notifyScore("Life", -1);
			else player.notifyScore("Empate"); //draw
		}
		else player.notifyScore("Empate"); //draw
	}

	public void aceAs11Button() //User selects ACE as 11 points
	{
		userPoints += 11;
		enableAceButtons("AceAs11", "AceAs1", false);
		newCardAsked = false;
	}

	public void aceAs1Button() //User selects ACE as 1 point
	{
		userPoints += 1;
		enableAceButtons("AceAs11", "AceAs1", false);
		newCardAsked = false;
	}
	public void newCardButton() // user asking for new card
    {
		if (!newCardAsked) cardToInt = newCard();
    }

	public void stickButton() //user sticks
    {
		if (!newCardAsked)
		{
			botTurn = true; //bot turn starts
			numberOfCards[0] = current; //sets amount of cards dealed for player
			current = 1; //resets amount of cards
			Destroy(GameObject.Find("NextCardButton")); //buttons destroyed
			Destroy(GameObject.Find("StickButton"));
		}
    }
}
