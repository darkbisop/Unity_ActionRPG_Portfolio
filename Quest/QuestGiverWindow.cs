using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    private static QuestGiverWindow instance;

    public static QuestGiverWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<QuestGiverWindow>();
            }

            return instance;
        }
    }

    private QuestGiver questGiver;

    [SerializeField] private Transform questArea;
    [SerializeField] private GameObject questPrefabs;
    [SerializeField] private GameObject acceptBtn, backBtn, completeBtn, questNameList;
    [SerializeField] private QuestReward questReward;

    private List<GameObject> quests = new List<GameObject>();
    private QuestInfo selectQuest;

    public void ShowQuest(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach (GameObject go in quests)
        {
            Destroy(go);
        }
        questArea.gameObject.SetActive(true);
        questNameList.SetActive(false);

        foreach (QuestInfo quest in questGiver.MyQuests)
        {
            if (quest != null)
            {
                GameObject go = Instantiate(questPrefabs, questArea);
                go.GetComponent<Text>().text = quest.MyQuestName;

                go.GetComponent<VendorQuestScript>().MyQuest = quest;

                quests.Add(go);

                if (Questlog.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    go.GetComponent<Text>().text += "(C)";
                }
                else if (Questlog.MyInstance.HasQuest(quest))
                {
                    Color c = go.GetComponent<Text>().color;

                    c.a = 0.5f;

                    go.GetComponent<Text>().color = c;
                }
            }          
        }
    }

    public void ShowQuestList(QuestInfo quest)
    {
        this.selectQuest = quest;

        if (Questlog.MyInstance.HasQuest(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if (Questlog.MyInstance.HasQuest(quest) == false)
        {
            acceptBtn.SetActive(true);
        }

        backBtn.SetActive(true);
        questArea.gameObject.SetActive(false);
        questNameList.SetActive(true);

        string objectives = string.Empty;
        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }

        questNameList.GetComponent<Text>().text = string.Format("<b>{0}</b>\n<size=35>{1}</size>\n", quest.MyQuestName, quest.MyDescription);
        questReward.QuestAddItem(quest.MyQuestReward);
    }

    public void CompleteQuest()
    {
        if (selectQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if (selectQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyCompltedQuests.Add(selectQuest.MyQuestName);
                    questGiver.MyQuests[i] = null;
                    selectQuest.MyQuestGiver.UpdateQuestStatus(selectQuest);
                    //selectQuest = null;
                }
            }

            foreach (CollectObjective o in selectQuest.MyCollectObjectives)
            {
                InventoryScript.MyInstance.itemCountChangeEvent -= new ItemCountChanged(o.UpdateItemCount);
                o.Complete();
            }

            foreach (KillObjective o in selectQuest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmEvent -= new KillConfirm(o.UpdateKillCount);
            }

            Questlog.MyInstance.RemoveQuest(selectQuest.MyQuestScript);
            InventoryScript.MyInstance.AddItem(selectQuest.MyQuestReward);
            Back();
        }
    }
    public void Back()
    {
        acceptBtn.SetActive(false);
        backBtn.SetActive(false);
        ShowQuest(questGiver);
        completeBtn.SetActive(false);
    }

    public void Accept()
    {
        Questlog.MyInstance.AcceptQuest(selectQuest);
        Back();
    }

    public override void Open(NPC npc)
    {
        ShowQuest((npc as QuestGiver));
        base.Open(npc);
    }

    public override void Close(NPC npc)
    {
        completeBtn.SetActive(false);
        ShowQuest((npc as QuestGiver));
        base.Close(npc);
    }
}
