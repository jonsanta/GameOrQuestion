using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
	//Buttons and text UI
	public GameObject buyLivesButton;
	public GameObject moneyHackButton;
	public GameObject notificationText;

	private Player player;

	private bool isNotificationEnabled = false; // enables the timer
	private float notificationTimer; //timer during which the notification will be displayed

	void Update(){
		Notificacion ();
	}

	void Notificacion(){
		if (isNotificationEnabled == true) {
			notificationTimer += Time.deltaTime; //timer enabled and text active
			notificationText.SetActive (true);
		}
		else
		{
			notificationTimer = 0f;
			notificationText.SetActive(false); //text disabled when flag is false
		}

		if (notificationTimer > 2f) //timer ends
		{
			isNotificationEnabled = false;
		}
	}

	public void Onclick(){ //pressing the shop button will show a new menu
		player = GameObject.Find("Player").GetComponent<Player>();

		buyLivesButton.SetActive (!buyLivesButton.activeSelf); 
		moneyHackButton.SetActive (!moneyHackButton.activeSelf);
		player.setShopOppened(buyLivesButton.activeSelf);
	}

	void Comprado(string message, Color color){//recives and shows message
		isNotificationEnabled = true;

		notificationText.GetComponent<Text>().color = color;
		notificationText.GetComponent<Text>().text = message;

		buyLivesButton.SetActive (false);
		moneyHackButton.SetActive (false);
	}

	public void buyLives(){

		if (player.getLives() < 3 && player.getMoney() >= 10)
		{
			player.setLives(+1);//purchasing 1 life
			player.setMoney(-10);
			Comprado("Has comprado 1 vida", Color.green);
		}
		else if (player.getLives() >= 3) 
		{
			Comprado("No puedes comprar mas vidas", Color.red); //3 lifes limit
        }
        else 
		{
			Comprado("No tienes suficientes monedas", Color.red);
		}
		player.setShopOppened(false);
	}

	public void moneyHack(){
		player.setMoney(+10);
		Comprado("10 monedas añadidas", Color.green);
		player.setShopOppened(false);
	}
}
