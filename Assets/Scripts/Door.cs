using UnityEngine.SceneManagement;
using UnityEngine;
public class Door : MonoBehaviour
{

	private Animator anim;
	private int result; //whats behind the door
	private bool openned = false; //door openned

	private Player player;

	void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
		anim = this.gameObject.GetComponent<Animator>();
		result = Random.Range(0, 9); // Randomize what is behind the door
		player.setSelectedGame(0); // sets the selected game to none
	}

	// Update is called once per frame
	void Update()
    {
		if (openned) openDoor();
    }

	void OnMouseDown()
    {
		if (player.getLives() > 0 && !player.getShopOppened()) // if still got lives. Can open door
		{
			anim.SetTrigger("Open");
			openned = true;
		}
	}
	void openDoor()
	{
		
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Openned")) //if animation ended
		{
			switch (result) //1-2-3-4-5 : question game scene, 6-7 : minigames scene, 8: chest scene
			{
				case 6:

				case 7:
					// Selects wich minigame will be loaded
					player.setSelectedGame(Random.Range(1, 4));
					SceneManager.LoadScene("Escena2", LoadSceneMode.Single);
					break;
				case 8:
					SceneManager.LoadScene("EscenaCofre", LoadSceneMode.Single);
					break;
				default:
					SceneManager.LoadScene("EscenaPregunta", LoadSceneMode.Single);
					break;
			}
		}

	}
}

