using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EquipType { helmet, Belt, Boots, Ring, Beer, Timer, Sword, Shield }

[CreateAssetMenu(fileName = "Equip", menuName = "Item/Equip", order = 2)]
public class Equip : Item
{
    private static Equip instance;

    public static Equip MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Equip>();
            }

            return instance;
        }
    }

    private ItemQuality quality;

    [SerializeField] private EquipType equipType;
    private int Atk, Def, HP, Cri, Sta;

    internal EquipType MyEquipType { get => equipType; }
    public ItemQuality MyItemQuality { get => quality; set => quality = value; }
    public int MyAtk { get => Atk; set => Atk = value; }
    public int MyDef { get => Def; set => Def = value; }
    public int MyHP { get => HP; set => HP = value; }
    public int MyCri { get => Cri; set => Cri = value; }
    public int MySta { get => Sta; set => Sta = value; }

    private void OnEnable()
    {
        if (MyType == "ShopItem") InitItemStatus();
        else if (MyType == "QuestReward")
        {
            quality = ItemQuality.Legendary;
            Atk = 7;
            Def = 7;
            HP = 7;
            Cri = 7;
            Sta = 7;
        }
    }

    public override string GetDescription()
    {
        string stat = string.Empty;

        if (Atk > 0) stat += string.Format("\n\n +{0} ATK", Atk);
        if (Def > 0) stat += string.Format("\n +{0} DEF", Def);
        if (HP > 0) stat += string.Format("\n +{0} HP", HP);
        if (Cri > 0) stat += string.Format("\n +{0} CRI", Cri);
        if (Sta > 0) stat += string.Format("\n +{0} STA", Sta);

        return string.Format("<color={0}> {1}</color> \n <color=#ffd111>{2} </color>", QualityColor.MyColor[quality], MyName, MyDescription) + stat;
    }

    public void InitItemStatus()
    {
        quality = (ItemQuality)Random.Range((int)ItemQuality.Common, (int)ItemQuality.Total);

        switch (quality)
        {
            case ItemQuality.Common:
                Atk = Random.Range(1, 2);
                Def = Random.Range(1, 2);
                HP = Random.Range(1, 2);
                Cri = Random.Range(1, 2);
                Sta = Random.Range(1, 2);
                break;
            case ItemQuality.Rare:
                Atk = Random.Range(1, 3);
                Def = Random.Range(1, 3);
                HP = Random.Range(1, 3);
                Cri = Random.Range(1, 3);
                Sta = Random.Range(1, 3);
                break;
            case ItemQuality.Unique:
                Atk = Random.Range(1, 4);
                Def = Random.Range(1, 4);
                HP = Random.Range(1, 4);
                Cri = Random.Range(1, 4);
                Sta = Random.Range(1, 4);
                break;
            case ItemQuality.Epic:
                Atk = Random.Range(1, 5);
                Def = Random.Range(1, 5);
                HP = Random.Range(1, 5);
                Cri = Random.Range(1, 5);
                Sta = Random.Range(1, 5);
                break;
            case ItemQuality.Legendary:
                Atk = Random.Range(2, 6);
                Def = Random.Range(2, 6);
                HP = Random.Range(2, 6);
                Cri = Random.Range(2, 6);
                Sta = Random.Range(2, 6);
                break;
        }
    }

    public void EquipArmor()
    {
        CharacterPanel.MyInstance.EquipArmor(this);
    }

    public void DequipArmor()
    {
        CharacterPanel.MyInstance.DequipArmor(this);
    }
}
