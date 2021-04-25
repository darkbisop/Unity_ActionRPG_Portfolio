using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInfo
{
    [SerializeField] private string questName;
    [SerializeField] private string description;
    [SerializeField] private CollectObjective[] collectObjectives;
    [SerializeField] private KillObjective[] killObjectives;
    [SerializeField] private Item questReward;

    public QuestScript MyQuestScript { get; set; }
    public QuestGiver MyQuestGiver { get; set; }

    public string MyQuestName { get => questName; set => questName = value; }
    public string MyDescription { get => description; set => description = value; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; set => collectObjectives = value; }
    public KillObjective[] MyKillObjectives { get => killObjectives; set => killObjectives = value; }
    public Item MyQuestReward { get => questReward; }

    public bool IsComplete
    {
        get
        {
            foreach (Objective obj in collectObjectives)
            {
                if (obj.IsComplete == false)
                {
                    return false;
                }
            }

            foreach (Objective obj in killObjectives)
            {
                if (obj.IsComplete == false)
                {
                    return false;
                }
            }

            return true;
        }
    }

    
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField] private int amount;
    [SerializeField] private string type;
    private int currentAmount;

    public int MyAmount { get => amount; }
    public string MyType { get => type; }
    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }

    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.MyName.ToLower())
        {
            MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(item.MyName);

            if (MyCurrentAmount <= MyAmount)
            {
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0} : {1} / {2}", item.MyName, MyCurrentAmount, MyAmount));
            }

            Questlog.MyInstance.UpdateSeleted();
            Questlog.MyInstance.CheckCompletion();
        }
    }

    public void UpdateItemCount()
    {
        // 인벤토리에 있는 아이템타입을 담아준다
        MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(MyType);

        Questlog.MyInstance.UpdateSeleted();
        Questlog.MyInstance.CheckCompletion();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.MyInstance.GetItems(MyType, MyAmount);

        foreach (Item item in items)
        {
            item.Remove();
        }
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(string type)
    {
        if (MyType == type)
        {
            MyCurrentAmount++;

            if (MyCurrentAmount <= MyAmount)
            {
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0} : {1} / {2}", MyType, MyCurrentAmount, MyAmount));
            }

            Questlog.MyInstance.UpdateSeleted();
            Questlog.MyInstance.CheckCompletion();
        }
    }
}