using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;

    public static SaveManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveManager>();
            }

            return instance;
        }
    }

    [SerializeField] private Item[] items;
    [SerializeField] private ActionButton[] actionButtons;

    private EquipButton[] equipment;
    public static bool isChange = false;

    void Awake()
    {
        equipment = FindObjectsOfType<EquipButton>();
    }

    void Start()
    {
        if (isChange == true)
        {
            Load();
            isChange = false;
        }

        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(this);
    }

    public void ChangeDemonScene()
    {
        Save();
        StartCoroutine(FadeOut.MyInstance.Fade(1.0f));
        StartCoroutine(BossTest_GiantDemon_Desert());
    }

    public void ChangeDragonScene()
    {
        Save();
        StartCoroutine(FadeOut.MyInstance.Fade(1.0f));
        StartCoroutine(BossTest_Dragon());
    }

    public void testChangeTownScene()
    {
        isChange = true;
        Save();
        LoadSceneManager.LoadScene("BossTest_GiantDemon_Town");
    }

    IEnumerator BossTest_GiantDemon_Desert()
    {
        yield return new WaitForSeconds(1.5f);
        LoadSceneManager.LoadScene("BossTest_GiantDemon_Desert");
    }

    IEnumerator BossTest_Dragon()
    {
        yield return new WaitForSeconds(1.5f);
        LoadSceneManager.LoadScene("BossTest_Dragon");
    }

    public void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Create);

            SaveData data = new SaveData();

            SavePlayer(data);
            SaveInventory(data);
            SaveEquipment(data);
            SaveActionButtons(data);
            SaveQuest(data);
            SaveQuestGiver(data);

            bf.Serialize(file, data);

            file.Close();
        }
        catch (System.Exception)
        {
        	
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(PlayerMovement.MyInstance.MyGold, PlayerMovement.MyInstance.MyHealth.CurrentValue,
            PlayerMovement.MyInstance.MyHealth.MaxValue, PlayerMovement.MyInstance.AtkPoint, PlayerMovement.MyInstance.DefPoint,
            PlayerMovement.MyInstance.CriPoint, PlayerMovement.MyInstance.MyStamina.CurrentValue, PlayerMovement.MyInstance.MyStamina.MaxValue);
    }

    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.MyInstance.GetAllItems();

        foreach (SlotScript slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyName, slot.MyObserverStackItem.Count, slot.MyIndex));
        }
    }

    private void SaveEquipment(SaveData data)
    {
        foreach (EquipButton button in equipment)
        {
            if (button.MyEquip != null)
            {
                data.MyEquipmentData.Add(new EquipmentData(button.MyEquip.MyName, button.name, button.MyEquip.MyItemQuality,
                    button.MyEquip.MyAtk, button.MyEquip.MyDef, button.MyEquip.MyHP, button.MyEquip.MyCri, button.MyEquip.MySta));
            }
        }
    }

    private void SaveActionButtons(SaveData data)
    {
        for (int i =0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].MyUseable != null)
            {
                ActionButtonData action;

                if (actionButtons[i].MyUseable is Skill)
                {
                    action = new ActionButtonData((actionButtons[i].MyUseable as Skill).MySkillName, false, i);
                }
                else
                {
                    action = new ActionButtonData((actionButtons[i].MyUseable as Item).MyName, true, i);
                }

                data.MyActionButtonData.Add(action);
            }
        }
    }

    private void SaveQuest(SaveData data)
    {
        foreach (QuestInfo quest in Questlog.MyInstance.MyQuests)
        {
            data.MyQuestData.Add(new QuestData(quest.MyQuestName, quest.MyDescription, 
                quest.MyCollectObjectives, quest.MyKillObjectives, quest.MyQuestGiver.MyQuestGiverId));
        }
    }

    private void SaveQuestGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiver questGiver in questGivers)
        {
            data.MyQuestGiverData.Add(new QuestGiverData(questGiver.MyQuestGiverId, questGiver.MyCompltedQuests));
        }
    }

    public void Load()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            LoadPlayer(data);
            LoadInventory(data);
            LoadEquipment(data);
            LoadActionButtons(data);
            LoadQuest(data);
            LoadQuestGiver(data);
        }
        catch (System.Exception e)
        {

        }
    }

    private void LoadPlayer(SaveData data)
    {
        PlayerMovement.MyInstance.MyGold = data.MyPlayerData.MyGold;
        PlayerMovement.MyInstance.MyHealth.InitProgress(data.MyPlayerData.MyHp, data.MyPlayerData.MyMaxHp);
        PlayerMovement.MyInstance.MyStamina.InitProgress(data.MyPlayerData.MySta, data.MyPlayerData.MyMaxSta);
    }

    private void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.MyInventoryData.MyItems)
        {
            Item item = Array.Find(items, x => x.MyName == itemData.MyTitle);

            for (int i = 0; i < itemData.MyStackCount; i++)
            {
                InventoryScript.MyInstance.PlaceInSpecific(item, itemData.MySlotIndex);
            }
        }
    }

    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData equipmentData in data.MyEquipmentData)
        {
            EquipButton bt = Array.Find(equipment, x => x.name == equipmentData.MyType);
            bt.EquipArmor(Array.Find(items, x => x.MyName == equipmentData.Mytitle) as Equip);

            bt.MyEquip.MyItemQuality = equipmentData.MyItemQuality;
            SlotScript.MyInstance.SetItemColor(bt.MyEquip, bt.MySlotIcon);

            bt.MyEquip.MyAtk = equipmentData.MyAtk;
            bt.MyEquip.MyDef = equipmentData.MyDef;
            bt.MyEquip.MyHP = equipmentData.MyHP;
            bt.MyEquip.MyCri = equipmentData.MyCri;
            bt.MyEquip.MySta = equipmentData.MySta;

            CharacterPanel.MyInstance.InCreaseStat(bt.MyEquip);

            //PlayerMovement.MyInstance.MyAttack += bt.MyEquip.MyAtk;
            //PlayerMovement.MyInstance.MyDefence += bt.MyEquip.MyDef;
            //PlayerMovement.MyInstance.MyCri += bt.MyEquip.MyCri;

            //Stats stats = new Stats(PlayerMovement.MyInstance.AtkPoint, PlayerMovement.MyInstance.DefPoint,
            //PlayerMovement.MyInstance.HpPoint, PlayerMovement.MyInstance.CriPoint, PlayerMovement.MyInstance.StaPoint);

            //CharacterPanel.MyInstance.MyChat.Setstats(stats);

            //PlayerMovement.MyInstance.AtkPoint += (bt.MyEquip.MyAtk * 0.2f);
            //CharacterPanel.MyInstance.atkText.text = PlayerMovement.MyInstance.MyAttack.ToString();

            //PlayerMovement.MyInstance.DefPoint += (bt.MyEquip.MyDef * 0.2f);
            //CharacterPanel.MyInstance.defText.text = PlayerMovement.MyInstance.MyDefence.ToString();

            //PlayerMovement.MyInstance.HpPoint += (bt.MyEquip.MyHP * 0.2f);
            //CharacterPanel.MyInstance.hpText.text = PlayerMovement.MyInstance.MyHealth.MaxValue.ToString();

            //PlayerMovement.MyInstance.CriPoint += (bt.MyEquip.MyCri * 0.2f);
            //CharacterPanel.MyInstance.criText.text = PlayerMovement.MyInstance.MyCri.ToString();

            //PlayerMovement.MyInstance.StaPoint += (bt.MyEquip.MySta * 0.2f);
            //CharacterPanel.MyInstance.spdText.text = PlayerMovement.MyInstance.MyStamina.MaxValue.ToString();

            //stats.SetStatAmount(Stats.Type.ATTACK, PlayerMovement.MyInstance.AtkPoint);
            //stats.SetStatAmount(Stats.Type.DEFENCE, PlayerMovement.MyInstance.DefPoint);
            //stats.SetStatAmount(Stats.Type.CRI, PlayerMovement.MyInstance.CriPoint);
        }
    }

    private void LoadActionButtons(SaveData data)
    {
        foreach (ActionButtonData buttonData in data.MyActionButtonData)
        {
            if (buttonData.IsItem)
            {
                actionButtons[buttonData.MyIndex].SetUseable(InventoryScript.MyInstance.GetUseables(buttonData.MyAction));
            }
            else
            {
                actionButtons[buttonData.MyIndex].SetUseable(SkillBook.MyInstance.GetSpell(buttonData.MyAction));
            }
        }
    }

    private void LoadQuest(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestData questData in data.MyQuestData)
        {
            QuestGiver qg = Array.Find(questGivers, x => x.MyQuestGiverId == questData.MyQuestGiverId);
            QuestInfo q = Array.Find(qg.MyQuests, x => x.MyQuestName == questData.MyTitle);

            q.MyQuestGiver = qg;
            q.MyCollectObjectives = questData.MyCollectObjectives;
            q.MyKillObjectives = questData.MyKillObjectives;

            Questlog.MyInstance.AcceptQuest(q);
        }
    }

    private void LoadQuestGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach (QuestGiverData questGiverData in data.MyQuestGiverData)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.MyQuestGiverId == questGiverData.MyQuestGiverId);

            questGiver.MyCompltedQuests = questGiverData.MyCompleteQuest;
            questGiver.UpdateQuestStatus(QuestScript.MyInstance.MyQuest);
        }
    }
}
