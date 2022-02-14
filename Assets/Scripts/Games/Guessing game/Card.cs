using UnityEngine;

public class Card : MonoBehaviour {
	private CreateCard createCard;

	private Vector3 position; //world position for card

	private Texture2D cardTexture; //uncovered texture
	private Texture2D coverTexture; //cover texture

    void OnMouseDown(){ //uncover card
		if(createCard.getIsShowable() && GetComponent<MeshRenderer>().material.mainTexture != cardTexture)
        {
			GetComponent<MeshRenderer>().material.mainTexture = cardTexture;
			createCard.click(this, cardTexture);
		}
	}

	public void showCover(){
		Invoke ("Cover", 1f); // cover card after 1 second
	}

	void Cover(){
		GetComponent<MeshRenderer> ().material.mainTexture = coverTexture;
		createCard.setIsShowable(true); //cards covered ---> new cards can be uncovered
	}
	public void setCardTexture(Texture2D cardTexture)
	{
		this.cardTexture = cardTexture;
	}
	public void setCoverTexture(Texture2D coverTexture)
	{
		this.coverTexture = coverTexture;
	}
	public Vector3 getPosition()
    {
		return position;
    }

	public void setPosition(Vector3 position)
    {
		this.position = position; 
    }

	public Texture2D getCardTexture()
    {
		return cardTexture;
    }
	
	public void setCreateCard(CreateCard createCard)
    {
		this.createCard = createCard;
    }
}
