using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] QuestInfo[] quests;
    [SerializeField] Sprite question, questionSilver, exclamation;
    [SerializeField] SpriteRenderer statusRenderer;
    [SerializeField] private int questGiverId;

    public QuestInfo[] MyQuests { get => quests; }
    private List<string> compltedQuests = new List<string>();
    

    public List<string> MyCompltedQuests
    {
        get
        {
            return compltedQuests;
        }

        set
        {
            compltedQuests = value;

            foreach (string title in compltedQuests)
            {
                for (int i = 0; i < quests.Length; i++)
                {
                    if (quests[i] != null && quests[i].MyQuestName == title)
                    {
                        quests[i] = null;
                    }
                }
            }
        }
    }

    public int MyQuestGiverId { get => questGiverId; }

    private void Start()
    {
        foreach (QuestInfo quest in quests)
        {
            quest.MyQuestGiver = this;
        } 
    }

    public void UpdateQuestStatus(QuestInfo test)
    {
        int count = 0;
        foreach (QuestInfo quest in quests)
        {
            if (quest != null)
            {
                if (test == null)
                {
                    if (!quest.IsComplete && Questlog.MyInstance.HasQuest(quest))
                    {
                        statusRenderer.sprite = questionSilver;
                    }
                    else if (!Questlog.MyInstance.HasQuest(quest))
                    {
                        statusRenderer.sprite = exclamation;
                        break;
                    }
                }
                else if (test != null)
                {
                    if (test.IsComplete)
                    {
                        statusRenderer.sprite = question;
                        break;
                    }
                    else if (!quest.IsComplete && Questlog.MyInstance.HasQuest(quest))
                    {
                        statusRenderer.sprite = questionSilver;
                    }
                    else if (!Questlog.MyInstance.HasQuest(quest))
                    {
                        statusRenderer.sprite = exclamation;
                        break;
                    }
                }
            }
            else
            {
                count++;

                if (count == quests.Length)
                {
                    statusRenderer.enabled = false;
                }
            }
        }
    }
}
