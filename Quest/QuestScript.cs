using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    private static QuestScript instance;

    public static QuestScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<QuestScript>();
            }

            return instance;
        }
    }

    public QuestInfo MyQuest { get; set; }
    private bool MarkedComplete = false;

    public void SelectQuest()
    {
        GetComponent<Text>().color = Color.red;
        Questlog.MyInstance.ShowDescription(MyQuest);
    }

    public void DeSelectQuest()
    {
        GetComponent<Text>().color = Color.white;
    }

    public void IsComplete()
    {
        if (MyQuest.IsComplete && MarkedComplete == false)
        {
            MarkedComplete = true;
            GetComponent<Text>().text += "(C)";
            MessageFeedManager.MyInstance.WriteMessage(string.Format("{0} (Complete)", MyQuest.MyQuestName));
        }
        else if (MyQuest.IsComplete == false)
        {
            MarkedComplete = false;
            GetComponent<Text>().text = MyQuest.MyQuestName;
        }
    }
}
