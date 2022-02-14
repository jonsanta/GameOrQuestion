using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Cofre : MonoBehaviour {

	private Animator anim;

	public GameObject[] rewards = new GameObject[5]; //contains different objects can be displayed inside the chest

	private int result; 

	private bool open = false;

	// Use this for initialization
	void Start () {
		anim = this.gameObject.GetComponent<Animator>(); //sets animator
		result = Random.Range(0, 5); //random object inside chest
	}

	void Update()
    {
		if(open)
        {
			OpcionesCofre(); //displays object inside chest
        }
	}

	public void Open() //open chest
    {
		anim.SetTrigger("Open"); //enable animation
		open = true;
		Destroy(GameObject.Find("OpenButton"));
		Destroy(GameObject.Find("ContinueButton"));
	}

	public void Continue()
    {
		SceneManager.LoadScene("Escena1"); //leave chest scene
	}


	void OpcionesCofre(){
		Player player = GameObject.Find("Player").GetComponent<Player>();

		if(result == 0) // +1 Life (ITEM)
        {
			rewards[0].SetActive(true);
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
			{
				if (player.getLives() < 3)
					player.notifyScore("lifegain", +1);
				else player.notifyScore("No hay espacio para mas vidas");
			}
		}
		else if( result == 1) // -1 Life (ITEM)
        {
			rewards[1].SetActive(true);
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
				player.notifyScore("Life", -1);
		}
		else if(result == 2) // new Question (Item)
        {
			rewards[2].SetActive(true);
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
				SceneManager.LoadScene("EscenaPregunta");
		}
		else if(result == 3) // new minigame (Item)
        {
			rewards[3].SetActive(true);
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
			{
				player.setSelectedGame(Random.Range(1, 4)); //Selects Random Game
				SceneManager.LoadScene("Escena2");
			}
		}
		else if(result == 4) // +10 Coins (Item)
        {
			rewards[4].SetActive(true);
			if (anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
			{
				player.notifyScore("Money", +10);
			}
		}
	}
}
