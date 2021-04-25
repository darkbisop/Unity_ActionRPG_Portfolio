using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangeEvent;

    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    /// <summary>
    /// 인스펙터창에서 아이템을 추가
    /// </summary>
    [SerializeField] private Item[] items;
    [SerializeField] private Text itemInfo;
    [SerializeField] private Text money;
    [SerializeField] private CanvasGroup canvasGroup;

    private SlotScript fromSlot, slots;

    BagItem bag;

    public SlotScript FromSlot
    {
        get => fromSlot;

        set
        {
            fromSlot = value;

            if (value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
                fromSlot.MySlotIcon.color = Color.grey;
            }
        }
    }

    public Text MyItemInfo { get => itemInfo; set => itemInfo = value; }

    private void Awake()
    {
        bag = (BagItem)Instantiate(items[0]);
        bag.Init(12);
        bag.Use();
        //AddItem((HealthPotion)Instantiate(items[1]));
        //AddItem((Equip)Instantiate(items[2]));
        //AddItem((Equip)Instantiate(items[3]));
        //AddItem((Equip)Instantiate(items[4]));
        //AddItem((Equip)Instantiate(items[5]));
        //AddItem((Equip)Instantiate(items[6]));
        //AddItem((Equip)Instantiate(items[7]));
        //AddItem((Equip)Instantiate(items[8]));
        //AddItem((Equip)Instantiate(items[9]));
    }

    private void LateUpdate()
    {
        money.text = PlayerMovement.MyInstance.MyGold.ToString();
    }

    /// <summary>
    /// 인벤토리에서 아이템을 추가 한다
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        if (item.MyItemStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return;
            }
        }

        PlaceInEmpty(item);
    }

    private bool PlaceInStack(Item item)
    {
        foreach (SlotScript slot in bag.MyBagScript.MySlots)
        {
            if (slot.StackItem(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }

        return false; 
    }

    private void PlaceInEmpty(Item item)
    {
        // 빈슬롯이 있다면 아이템을 추가함
        // 인벤토리->가방->슬롯순으로 들어감
        if (bag.MyBagScript.AddItem(item))
        {
            OnItemCountChanged(item);
            return;
        }
    }

    public void PlaceInSpecific(Item item, int slotIndex)
    {
        bag.MyBagScript.MySlots[slotIndex].AddItem(item);
    }

    /// <summary>
    /// 인벤토리창에서 아이템 정보를 보여줌
    /// </summary>
    /// <param name="slot"></param>
    public void ShowDescription(SlotScript slot)
    {
        if (slots != null && slots != slot)
        {
            slots.MySelectIcon.SetActive(false);

            if (slots.MyItem is Equip)
            {
                SlotScript.MyInstance.SetItemColor(slots.MyItem, slots.MySlotIcon);
            }
        }

        if (slot.MyItem != null)
            itemInfo.text = string.Format("{0}\n", slot.MyItem.GetDescription());
        else itemInfo.text = string.Format("{0}", "");

        slots = slot;
    }

    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (SlotScript slot in BagScript.MyInstance.MySlots)
        {
            if (slot.IsEmpty == false && slot.MyItem.GetType() == type.GetType())
            {
                foreach (Item item in slot.MyObserverStackItem)
                {
                    useables.Push(item as IUseable);
                }
            }
        }

        return useables;
    }

    public IUseable GetUseables(string type)
    {
        foreach (SlotScript slot in BagScript.MyInstance.MySlots)
        {
            if (slot.IsEmpty == false && slot.MyItem.MyName == type)
            {
                return (slot.MyItem as IUseable);
            }
        }

        return null;
    }

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangeEvent != null)
        {
            itemCountChangeEvent.Invoke(item);
        }
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;

        foreach (SlotScript slot in BagScript.MyInstance.MySlots)
        {
            if (slot.IsEmpty == false && slot.MyItem.MyName == type)
            {
                itemCount += slot.MyObserverStackItem.Count;
            }
        }

        return itemCount;
    }

    public void CheckOpenInventory()
    {
        if (canvasGroup.alpha == 0)
        {
            itemInfo.text = string.Format("{0}", "");

            if (CharacterPanel.MyInstance.PrevEquipment != null)
            {
                if (CharacterPanel.MyInstance.PrevEquipment.MySelectIcon.activeSelf == true)
                {
                    CharacterPanel.MyInstance.PrevEquipment.DeSelectItem();
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
        }
    }

    public Stack<Item> GetItems(string type, int count)
    {
        Stack<Item> items = new Stack<Item>();

        foreach (SlotScript slot in BagScript.MyInstance.MySlots)
        {
            if (slot.IsEmpty == false && slot.MyItem.MyName == type)
            {
                foreach (Item item in slot.MyObserverStackItem)
                {
                    items.Push(item);

                    if (items.Count == count)
                    {
                        return items;
                    }
                }
            }
        }

        return items;
    }

    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> slots = new List<SlotScript>();

        foreach (SlotScript slot in bag.MyBagScript.MySlots)
        {
            if (slot.IsEmpty == false)
            {
                slots.Add(slot);
            }
        }

        return slots;
    }
}
