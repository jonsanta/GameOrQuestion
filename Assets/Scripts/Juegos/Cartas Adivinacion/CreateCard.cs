using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateCard : MonoBehaviour {
	private int couples; // how much cuples has the user done

	public GameObject cardPrefab;

	public Texture2D[] textures;
	public Texture2D coverTexture;

	private Card showedCard = null; //first selected card
	private Card pair = null; //second selected card

	private bool isShowable = false; //can more cards be shown?

	private List<GameObject> cards = new List<GameObject> (); //contains the whole card GameObjects

	private float timer; //if timer ends ----> Game lost

	public AudioClip correctPair;

	void Start(){
		createCards(); // Instantiate cards
	}

    void Update()
    {
		Player player = GameObject.Find("Player").GetComponent<Player>();
		GameObject.Find("UITimer").GetComponent<Text>().text = (35f -timer).ToString("F0");

		if (timer >= 35) //if timer ends
        {
			player.notifyScore("Life", -1);
			isShowable = false;
			GetComponent<AudioSource>().mute = true;
		}
		else if (couples == cards.Count / 2) //if all pairs have been uncovered
        {
			player.notifyScore("Score", +10);
			GetComponent<AudioSource>().mute = true;
		}
		else if (!GameObject.Find("StartGameButton")) timer += Time.deltaTime;
	}
    public void createCards(){ //generate cards

		for (int x = 0; x < 4; x++) { // 4 columns
			for (int y = 0; y < 4; y++) { // 4 rows
				float f = 8.4f / 4; //distance between cards
				Vector3 posicionTemp = new Vector3 (y * f, 0, x * f); // original position
				GameObject cartaTemp = Instantiate (cardPrefab, posicionTemp, Quaternion.Euler(new Vector3(0, 180, 0)));
				cartaTemp.GetComponent<Card>().setCreateCard(this);
				cartaTemp.GetComponent<Card>().setCoverTexture(coverTexture);
				cartaTemp.GetComponent<Card>().setPosition(posicionTemp);
				cards.Add(cartaTemp);
			}
		}

		for (int i = 0; i < cards.Count; i++)
		{
			cards[i].GetComponent<Card>().setCardTexture(textures[(i) / 2]); //set card texture
		}

		shuffleCards();
	}
	void shuffleCards(){ //change card positions
		int rnd;

		for (int i = 0; i < cards.Count; i++) {
			rnd = Random.Range (i, cards.Count);

			cards[i].transform.position = cards[rnd].transform.position;
			cards[rnd].transform.position = cards[i].GetComponent<Card> ().getPosition();

			cards[i].GetComponent<Card>().setPosition(cards[i].transform.position);
			cards[rnd].GetComponent<Card>().setPosition(cards[rnd].transform.position);
		}
	}

	public void click (Card _carta, Texture2D texture){
		if (showedCard == null) { //first card clicked
			showedCard = _carta;
		}//second card clicked and no pair
		else if(showedCard.GetComponent<MeshRenderer>().material.mainTexture != texture){
			_carta.showCover(); //cover cards
			showedCard.showCover();
			showedCard = null;
			isShowable = false; //cards cant be uncovered until before invoke reaches
		}
		else //second card clicked and you got pair
        {
			pair = _carta;
			GetComponent<AudioSource>().PlayOneShot(correctPair, 5f);
			Invoke("destroyCards", 1f); //destroy both cards after 1 second
			couples++; //add new cuple into score
			isShowable = false; //cards cant be uncovered until cards are destroyed
		}
	}
	void destroyCards() //destroy both cards
    {
		Destroy(showedCard.gameObject);
		Destroy(pair.gameObject);
		isShowable = true; // cards can be uncovered
	}

	void startGame()
    {
		for (int x = 0; x < cards.Count; x++)
		{
			cards[x].GetComponent<Card>().showCover(); //cover all the cards
		}
		Destroy(GameObject.Find("StartGameButton")); //once game started destroy start game button
		GetComponent<AudioSource>().mute = false;
	}

	public void startButton() //start game button
    {

		for (int x = 0; x < cards.Count; x++)
		{
			Texture2D texture = cards[x].GetComponent<Card>().getCardTexture();
			cards[x].GetComponent<MeshRenderer>().material.mainTexture = texture; //show card value textures
		}

		Invoke ("startGame", 2f); //after 2 seconds cover all the cards
	}

	public bool getIsShowable()
    {
		return isShowable;
    }

	public void setIsShowable(bool isShowable)
    {
		this.isShowable = isShowable;
    }
}
