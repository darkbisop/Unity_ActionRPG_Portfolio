using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel instance;

    public static CharacterPanel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CharacterPanel>();
            }

            return instance;
        }
    }

    [SerializeField]
    private EquipButton head, belt, boots, beer, ring, timer, sword, shield;
    private EquipButton prevEquipment;

    public EquipButton MySelectButton { get; set; }
    public EquipButton PrevEquipment { get => prevEquipment; set => prevEquipment = value; }
    public UI_StatRadarChart MyChat { get => StatRadarChart; set => StatRadarChart = value; }

    [SerializeField] private UI_StatRadarChart StatRadarChart;
    public Text atkText, defText, hpText, criText, spdText;

    public Stats stats;

    void Awake()
    {
        stats = new Stats(PlayerMovement.MyInstance.AtkPoint, PlayerMovement.MyInstance.DefPoint, 
            PlayerMovement.MyInstance.HpPoint, PlayerMovement.MyInstance.CriPoint, PlayerMovement.MyInstance.StaPoint);
    }

    private void Start()
    {
        StatRadarChart.Setstats(stats);
    }

    public void EquipArmor(Equip equip)
    {
        InCreaseStat(equip);

        switch (equip.MyEquipType)
        {
            case EquipType.helmet:
                head.EquipArmor(equip);
                break;
            case EquipType.Belt:
                belt.EquipArmor(equip);
                break;
            case EquipType.Boots:
                boots.EquipArmor(equip);
                break;
            case EquipType.Beer:
                beer.EquipArmor(equip);
                break;
            case EquipType.Ring:
                ring.EquipArmor(equip);
                break;
            case EquipType.Timer:
                timer.EquipArmor(equip);
                break;
            case EquipType.Sword:
                sword.EquipArmor(equip);
                break;
            case EquipType.Shield:
                shield.EquipArmor(equip);
                break;
        }
    }

    public void DequipArmor(Equip equip)
    {
        DeCreaseStat(equip);

        switch (equip.MyEquipType)
        {
            case EquipType.helmet:
                head.DequipArmor();
                break;
            case EquipType.Belt:
                belt.DequipArmor();
                break;
            case EquipType.Boots:
                boots.DequipArmor();
                break;
            case EquipType.Beer:
                beer.DequipArmor();
                break;
            case EquipType.Ring:
                ring.DequipArmor();
                break;
            case EquipType.Timer:
                timer.DequipArmor();
                break;
            case EquipType.Sword:
                sword.DequipArmor();
                break;
            case EquipType.Shield:
                shield.DequipArmor();
                break;
        }
    }

    private SlotScript slots;

    public void EquipItem(Item item)
    {
        if (item is Equip)
        {
            (item as Equip).EquipArmor();
        }

        InventoryScript.MyInstance.MyItemInfo.text = "";
    }

    public void SelectEquip(EquipButton equipButton)
    {
        if (prevEquipment != null && equipButton != prevEquipment)
        {
            prevEquipment.MySelectIcon.SetActive(false);
            prevEquipment.MySlotIcon.color = Color.white;
        }

        if (equipButton.MyEquip != null)
        {
            InventoryScript.MyInstance.MyItemInfo.text = equipButton.MyEquip.GetDescription();
        }
        else
        {
            InventoryScript.MyInstance.MyItemInfo.text = "";
        }

        prevEquipment = equipButton;
    }

    public void InCreaseStat(Equip equip)
    {
        PlayerMovement.MyInstance.AtkPoint += equip.MyAtk;
        PlayerMovement.MyInstance.MyAttack += equip.MyAtk;

        PlayerMovement.MyInstance.DefPoint += equip.MyDef;
        PlayerMovement.MyInstance.MyDefence += equip.MyDef;

        PlayerMovement.MyInstance.HpPoint += equip.MyHP;
        PlayerMovement.MyInstance.MaxHp += equip.MyHP;
        PlayerMovement.MyInstance.MyHealth.MaxValue = PlayerMovement.MyInstance.MaxHp;
        PlayerMovement.MyInstance.MyHealth.CurrentValue = PlayerMovement.MyInstance.MaxHp;

        PlayerMovement.MyInstance.CriPoint += equip.MyCri;
        PlayerMovement.MyInstance.MyCri += equip.MyCri;

        PlayerMovement.MyInstance.StaPoint += equip.MySta;
        PlayerMovement.MyInstance.MaxSta += equip.MySta;
        PlayerMovement.MyInstance.MyStamina.MaxValue = PlayerMovement.MyInstance.MaxSta;

        stats.IncreaseStat(Stats.Type.ATTACK, equip.MyAtk);
        atkText.text = PlayerMovement.MyInstance.MyAttack.ToString();

        stats.IncreaseStat(Stats.Type.DEFENCE, equip.MyDef);
        defText.text = PlayerMovement.MyInstance.MyDefence.ToString();

        stats.IncreaseStat(Stats.Type.HEALTH, equip.MyHP);
        hpText.text = PlayerMovement.MyInstance.MaxHp.ToString();

        stats.IncreaseStat(Stats.Type.CRI, equip.MyCri);
        criText.text = PlayerMovement.MyInstance.MyCri.ToString();

        stats.IncreaseStat(Stats.Type.STAMINA, equip.MySta);
        spdText.text = PlayerMovement.MyInstance.MyStamina.MaxValue.ToString();
    }

    public void DeCreaseStat(Equip equip)
    {
        PlayerMovement.MyInstance.AtkPoint -= equip.MyAtk;
        PlayerMovement.MyInstance.MyAttack -= equip.MyAtk;

        PlayerMovement.MyInstance.DefPoint -= equip.MyDef;
        PlayerMovement.MyInstance.MyDefence -= equip.MyDef;

        PlayerMovement.MyInstance.HpPoint -= equip.MyHP;
        PlayerMovement.MyInstance.MaxHp -= equip.MyHP;
        PlayerMovement.MyInstance.MyHealth.MaxValue = PlayerMovement.MyInstance.MaxHp;
        PlayerMovement.MyInstance.MyHealth.CurrentValue = PlayerMovement.MyInstance.MaxHp;

        PlayerMovement.MyInstance.CriPoint -= equip.MyCri;
        PlayerMovement.MyInstance.MyCri -= equip.MyCri;

        PlayerMovement.MyInstance.StaPoint -= equip.MySta;
        PlayerMovement.MyInstance.MaxSta -= equip.MySta;
        PlayerMovement.MyInstance.MyStamina.MaxValue = PlayerMovement.MyInstance.MaxSta;

        stats.DecreaseStat(Stats.Type.ATTACK, equip.MyAtk);
        atkText.text = PlayerMovement.MyInstance.MyAttack.ToString();

        stats.DecreaseStat(Stats.Type.DEFENCE, equip.MyDef);
        defText.text = PlayerMovement.MyInstance.MyDefence.ToString();

        stats.DecreaseStat(Stats.Type.HEALTH, equip.MyHP);
        hpText.text = PlayerMovement.MyInstance.MyHealth.MaxValue.ToString();

        stats.DecreaseStat(Stats.Type.CRI, equip.MyCri);
        criText.text = PlayerMovement.MyInstance.MyCri.ToString();

        stats.DecreaseStat(Stats.Type.STAMINA, equip.MySta);
        spdText.text = PlayerMovement.MyInstance.MyStamina.MaxValue.ToString();
    }
}
