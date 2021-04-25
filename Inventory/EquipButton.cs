using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EquipType equipType;
    [SerializeField] private Image icon;
    [SerializeField] private Image slotIcon;
    [SerializeField] private GameObject selectIcon;

    private Equip equipment;
    public static Equip saveEquip;

    public Image MySlotIcon { get => slotIcon; }
    public Equip MyEquip { get => equipment; }
    public GameObject MySelectIcon { get => selectIcon; set => selectIcon = value; }
    //public Equip MySaveEquip { get => saveEquip; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveableIcon is Equip)
            {
                Equip tmp = (Equip)HandScript.MyInstance.MyMoveableIcon;

                if (tmp.MyEquipType == equipType)
                {
                    EquipArmor(tmp);
                }
            }

            foreach (SlotScript slot in BagScript.MyInstance.MySlots)
            {
                if (slot.MySelectIcon.activeSelf == true)
                {
                    slot.MySelectIcon.SetActive(false);
                    slot.DeSelectItem();
                }
            }

            SelectItem();

        }
    }

    public void EquipArmor(Equip equip)
    {
        equip.Remove();
       
        if (equipment != null)
        {
            if (equipment != equip)
            {
                // 장비를 교체해주고
                equip.MySlot.AddItem(equipment);
                CharacterPanel.MyInstance.DeCreaseStat(equipment);
            }
        }


        icon.enabled = true;
        MySlotIcon.enabled = true;

        icon.sprite = equip.MyIcon;
        SlotScript.MyInstance.SetItemColor(equip, MySlotIcon);

        this.equipment = equip;

        if (HandScript.MyInstance.MyMoveableIcon == (equip as IMoveableIcon))
        {
            HandScript.MyInstance.DeleteIcon();
        }
    }

    public void DequipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;

        MySlotIcon.color = Color.white;
        MySlotIcon.enabled = false;

        equipment = null;
        saveEquip = null;
        MySelectIcon.SetActive(false);
    }

    public void SelectItem()
    {
        if (selectIcon.activeSelf == false)
        {
            saveEquip = this.equipment;
            selectIcon.SetActive(true);
            CharacterPanel.MyInstance.SelectEquip(this);
        }
    }

    public void DeSelectItem()
    {
        saveEquip = null;
        MySelectIcon.SetActive(false);
    }

    public void DequipItem()
    {
        if (SlotScript.MyInstance.MySlot == null && saveEquip != null)
        {
            InventoryScript.MyInstance.AddItem(saveEquip);

            CharacterPanel.MyInstance.DequipArmor(saveEquip);

            InventoryScript.MyInstance.MyItemInfo.text = "";
        }
    }
}
