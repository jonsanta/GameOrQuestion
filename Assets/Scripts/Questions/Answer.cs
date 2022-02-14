using UnityEngine;
using UnityEngine.SceneManagement;

public class Answer : MonoBehaviour
{
    private bool isTrueAnswer = false; //if this object has the correct answer
    private Question question;
    // Start is called before the first frame update

    public void setIsTrueAnswer ()
    {
        isTrueAnswer = true;
    }
    public void OnClick() //Answer Selected
    {
        if(isTrueAnswer)
        {
            GameObject.Find("Player").GetComponent<Player>().notifyScore("score", +10);
        }
        else 
        {
            GameObject.Find("Player").GetComponent<Player>().notifyScore("life", -1);
        }
        question.setAnswerSelected(true);
    }

    public void setQuestion(Question question)
    {
        this.question = question;
    }
}
