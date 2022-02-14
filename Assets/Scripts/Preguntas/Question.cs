using System.Xml;
using UnityEngine.UI;
using UnityEngine;

public class Question : MonoBehaviour
{
    public GameObject questionGObj;
    public GameObject[] buttons = new GameObject[4];

    public  string[] QuestionArray; //saves selected question
    private int[] order = new int[4]; //used for mixing the answers without losing the correct one
    private int orderIndex = 0; 
    private bool orderFlag = false; //this flag will be true if Random.Range repeats a number

    private bool answerSelected = false;

    void Start()
    {
        QuestionArray = readXML(); //The question and answers will be filled
    }


    void Update()
    {
        questionGObj.GetComponent<Text>().text = QuestionArray[0]; //Question displayed

        while (orderFlag || orderIndex < 4) //if flag is true or still need more numbers
        {
            order[orderIndex] = Random.Range(0, 4); //new random number
            orderFlag = false;

            for (int x = 0; x < orderIndex; x++) //loop the numbers we saved
            {
                if (order[orderIndex] == order[x] && orderIndex != x) orderFlag = true; //if repeats number flag true
            }

            if (orderFlag == false) orderIndex += 1; //if not, add the number
        }


        for (int x = 0; x < 4; x++)
        {
            if (order[x] == 0) buttons[x].GetComponent<Answer>().setIsTrueAnswer(); //sets the true answer
            buttons[x].GetComponentInChildren<Text>().text = QuestionArray[order[x] + 1]; //fills buttons whit the answer text
            buttons[x].GetComponent<Answer>().setQuestion(this);
        }

        if (answerSelected && buttons[3].GetComponent<Button>().enabled == true) // if user selects an answer ---> all buttons disabled
        {
            for (int x = 0; x < buttons.Length; x++)
            {
                buttons[x].GetComponent<Button>().enabled = false;
            }
        }
    }

    string[] readXML()
    {
        string[] arr = new string[5]; // Empty array

        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(Resources.Load<TextAsset>("Questions").text); //load XML document from TextAssets

        XmlNode xmlRootNode = xdoc.SelectSingleNode("Game"); //Root Node
        XmlNodeList nOfQuestions = xmlRootNode.ChildNodes; //Number of questions

        int index = (int)Random.Range(1f, nOfQuestions.Count); //Select random question

        foreach (XmlElement item in nOfQuestions)
        {
            if (item.Attributes["id"].Value == index.ToString())
            {
                for(int x = 0; x < item.ChildNodes.Count; x++)
                {
                    arr[x] = item.ChildNodes[x].InnerText; //arr[0] = questions, arr[1] = correct answer, arr[2,3,4] = incorrect answer
                }
            }
        }

        return arr;
    }

    public void setAnswerSelected(bool answerSelected)
    {
        this.answerSelected = answerSelected;
    }
}
