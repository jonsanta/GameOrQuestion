using System.Xml;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class Question : MonoBehaviour
{
    public GameObject questionGObj;
    public GameObject[] buttons = new GameObject[4];

    private string question = null;

    public List<int> order = new List<int>();

    private bool answerSelected = false;


    void Update()
    {
        while (order.Count < 4)
        {
            int temp = Random.Range(0, 4);//new random number
            if (!order.Contains(temp)) order.Add(temp);
        }

        while(question == null)readXML(order.ToArray()); //The question and answers will be filled
        questionGObj.GetComponent<Text>().text = question; //Question displayed

        // if user selects an answer ---> all buttons disabled
        if (answerSelected) 
            foreach(GameObject btn in buttons) 
                btn.GetComponent<Button>().enabled = false;
    }

    void readXML(int[] order)
    {
        string[] arr = new string[5]; // Empty array

        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(Resources.Load<TextAsset>("Questions").text); //load XML document from TextAssets

        XmlNodeList questions = xdoc.SelectSingleNode("Game").ChildNodes; //Root element childs


        int index;

        do index = (int)Random.Range(1f, questions.Count); //Select random question and detect if has been displayed before, if so do look for a new question
        while (GameObject.Find("Player").GetComponent<Player>().isQuestionRepeated(index));

        foreach (XmlElement item in questions)
        {
            if (item.Attributes["id"].Value == index.ToString())
            {
                for(int x = 0; x < item.ChildNodes.Count; x++)
                {
                    XmlNode node = item.ChildNodes[x]; //save each child in variable
                    switch(node.Name)
                    {
                        //if xml element name
                        case "Question": 

                            question = node.InnerText; //sets question
                            break;
                        case "CorrectAnswer":
                            buttons[order[x-1]].GetComponent<Answer>().setIsTrueAnswer(); //sets correct answer
                            goto default;
                        default:
                            buttons[order[x-1]].GetComponentInChildren<Text>().text = node.InnerText; //fills buttons whit the answer text
                            buttons[order[x-1]].GetComponent<Answer>().setQuestion(this);
                            break;
                    }
                }
            }
        }
    }

    public void setAnswerSelected(bool answerSelected)
    {
        this.answerSelected = answerSelected;
    }
}
