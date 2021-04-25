using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorQuestScript : MonoBehaviour
{
    public QuestInfo MyQuest { get; set; }

    public void SelectQuest()
    {
        QuestGiverWindow.MyInstance.ShowQuestList(MyQuest);
    }
}
