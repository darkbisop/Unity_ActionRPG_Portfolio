using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public PlayerData MyPlayerData { get; set; }
    public InventoryData MyInventoryData { get; set; }
    public List<EquipmentData> MyEquipmentData { get; set; }
    public List<ActionButtonData> MyActionButtonData { get; set; }
    public List<QuestData> MyQuestData { get; set; }
    public List<QuestGiverData> MyQuestGiverData { get; set; }

    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyEquipmentData = new List<EquipmentData>();
        MyActionButtonData = new List<ActionButtonData>();
        MyQuestData = new List<QuestData>();
        MyQuestGiverData = new List<QuestGiverData>();
    }
}

[Serializable]
public class PlayerData
{
    public float MyHp { get; set; }
    public float MyMaxHp { get; set; }
    public float MyAtk { get; set; }
    public float MyDef { get; set; }
    public float MySta { get; set; }
    public float MyMaxSta { get; set; }
    public float MyCri { get; set; }
    public int MyGold { get; set; }

    public PlayerData(int gold, float hp, float maxHp, float atk, float def, float cri, float sta, float maxSta)
    {
        this.MyGold = gold;
        this.MyHp = hp;
        this.MyMaxHp = maxHp;
        this.MyAtk = atk;
        this.MyDef = def;
        this.MyCri = cri;
        this.MySta = sta;
        this.MyMaxSta = maxSta;
    }
}

[Serializable]
public class InventoryData
{
    public List<ItemData> MyItems { get; set; }

    public InventoryData()
    {
        MyItems = new List<ItemData>();
    }
}

[Serializable]
public class ItemData
{
    public string MyTitle { get; set; }
    public int MyStackCount { get; set; }
    public int MySlotIndex { get; set; }

    public ItemData(string title, int stackCount, int slotIndex)
    {
        this.MyTitle = title;
        this.MyStackCount = stackCount;
        this.MySlotIndex = slotIndex;
    }
}

[Serializable]
public class EquipmentData
{
    public string Mytitle { get; set; }
    public string MyType { get; set; }
    public ItemQuality MyItemQuality { get; set; }
    public int MyAtk { get; set; }
    public int MyDef { get; set; }
    public int MyHP { get; set; }
    public int MyCri { get; set; }
    public int MySta { get; set; }

    public EquipmentData(string title, string type, ItemQuality quality, int atk, int def, int hp, int cri, int spd)
    {
        this.Mytitle = title;
        this.MyType = type;
        this.MyItemQuality = quality;
        this.MyAtk = atk;
        this.MyDef = def;
        this.MyHP = hp;
        this.MyCri = cri;
        this.MySta = spd;
    }
}

[Serializable]
public class ActionButtonData
{
    public string MyAction { get; set; }
    public bool IsItem { get; set; }
    public int MyIndex { get; set; }
    
    public ActionButtonData(string action, bool isItem, int index)
    {
        this.MyAction = action;
        this.IsItem = isItem;
        this.MyIndex = index;
    }
}

[Serializable]
public class QuestData
{
    public string MyTitle { get; set; }
    public string MyDescription { get; set; }
    public CollectObjective[] MyCollectObjectives { get; set; }
    public KillObjective[] MyKillObjectives { get; set; }
    public int MyQuestGiverId { get; set; }

    public QuestData(string title, string description, CollectObjective[] collective, KillObjective[] killobject, int giverId)
    {
        this.MyTitle = title;
        this.MyDescription = description;
        this.MyCollectObjectives = collective;
        this.MyKillObjectives = killobject;
        this.MyQuestGiverId = giverId;
    }
}

[Serializable]
public class QuestGiverData
{
    public List<string> MyCompleteQuest { get; set; }
    public int MyQuestGiverId { get; set; }

    public QuestGiverData(int questGiverId, List<string> completeQuest)
    {
        this.MyQuestGiverId = questGiverId;
        this.MyCompleteQuest = completeQuest;
    }

}
