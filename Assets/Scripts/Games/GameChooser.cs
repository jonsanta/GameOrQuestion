using UnityEngine;

public class GameChooser : MonoBehaviour {
	private int selected = 0;

	public GameObject game1;
	public GameObject game2;
	public GameObject game3;
	
	
	void Update () {
		selected = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getSelectedGame(); //gets selected game from player script
		loadGame ();
	}

	//ENABLES x Game depending on witch game was randomly selected
	void loadGame (){
		if (selected == 1) {
			game1.SetActive (true);
		}
		if (selected == 2) {
			game2.SetActive (true);
		}
		if (selected == 3) {
			game3.SetActive (true);
		}

	}
}
