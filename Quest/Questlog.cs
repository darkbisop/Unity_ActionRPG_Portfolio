using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questlog : MonoBehaviour
{
    private static Questlog instance;

    public static Questlog MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Questlog>();
            }

            return instance;
        }
    }

    public List<QuestInfo> MyQuests { get => quests; set => quests = value; }

    [SerializeField] private GameObject questPrefabs;
    [SerializeField] private Transform questParent;
    [SerializeField] private Text questDescription;
    [SerializeField] private Text questCount;
    [SerializeField] private int maxQuestCount;
    [SerializeField] private QuestReward questReward;

    private int currentCount;
    private QuestInfo selected;
    private List<QuestScript> questScripts = new List<QuestScript>();
    private List<QuestInfo> quests = new List<QuestInfo>();

    private CanvasGroup canvasGroup;

    private void Start()
    {
        questCount.text = currentCount + "/" + maxQuestCount; 
    }

    public void AcceptQuest(QuestInfo quest)
    {
        if (currentCount < maxQuestCount)
        {
            currentCount++;
            questCount.text = currentCount + "/" + maxQuestCount;

            foreach (CollectObjective o in quest.MyCollectObjectives)
            {
                InventoryScript.MyInstance.itemCountChangeEvent += new ItemCountChanged(o.UpdateItemCount);

                o.UpdateItemCount();
            }

            foreach (KillObjective o in quest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmEvent += new KillConfirm(o.UpdateKillCount);
            }

            quests.Add(quest);
            GameObject go = Instantiate(questPrefabs, questParent);

            QuestScript qs = go.GetComponent<QuestScript>();
            quest.MyQuestScript = qs;
            qs.MyQuest = quest;

            questScripts.Add(qs);

            go.GetComponent<Text>().text = quest.MyQuestName;

            CheckCompletion();
        }
        
    }
     
    public void UpdateSeleted()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(QuestInfo quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.DeSelectQuest();
            }

            string title = quest.MyQuestName;

            selected = quest;

            string objectives = string.Empty;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            foreach (Objective obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("<b>{0}</b>\n<size=40>{1}</size>\n\nObjectives\n<size=40>{2}</size>", title, quest.MyDescription, objectives);
            questReward.QuestAddItem(quest.MyQuestReward);
        }
    }

    public void CheckCompletion()
    {
        foreach (QuestScript qs in questScripts)
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus(qs.MyQuest);
            qs.IsComplete();

            if (qs.MyQuest.IsComplete)
            {
                return;
            }
        }
    }

    public bool HasQuest(QuestInfo quest)
    {
        return quests.Exists(x => x.MyQuestName == quest.MyQuestName);
    }

    public void AbandonQuest()
    {
        foreach (CollectObjective o in selected.MyCollectObjectives)
        {
            InventoryScript.MyInstance.itemCountChangeEvent -= new ItemCountChanged(o.UpdateItemCount);
        }

        foreach (KillObjective o in selected.MyKillObjectives)
        {
            GameManager.MyInstance.killConfirmEvent -= new KillConfirm(o.UpdateKillCount);
        }

        RemoveQuest(selected.MyQuestScript);
    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        quests.Remove(qs.MyQuest);
        questReward.RemoveQuestReward(qs.MyQuest.MyQuestReward);
        questDescription.text = string.Empty;
        selected = null;
        currentCount--;
        questCount.text = currentCount + "/" + maxQuestCount;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus(selected);
        qs = null;
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
