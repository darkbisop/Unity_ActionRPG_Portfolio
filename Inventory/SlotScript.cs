using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : EventTrigger, IPointerClickHandler, IClickable
{
    public static Item globalItem;
    private static SlotScript instance;

    public static SlotScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SlotScript>();
            }

            return instance;
        }
    }

    [SerializeField] private Image itemicon;
    [SerializeField] private Image slotIcon;
    [SerializeField] private GameObject selectIcon;
    [SerializeField] private Text stackText;

    private bool isPressed = false;
    private float saveChageTime;
    private GameObject uiMenu;

    private ObserverableStack<Item> items = new ObserverableStack<Item>();

    public SlotScript MySlot { get; set; }
    public int MyIndex { get; set; }

    void Start()
    {
        uiMenu = GameObject.Find("Menu");
    }

    /// <summary>
    /// 슬롯이 비어있는가 체크
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            return items.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyItemStackSize)
            {
                return false;
            }

            return true;
        }
    }

    public void Clear()
    {
        int initCount = MyObserverStackItem.Count;

        for (int i = 0; i < initCount; i++)
        {
            InventoryScript.MyInstance.OnItemCountChanged(items.Pop());
        }
    }

    /// <summary>
    /// 아이템 종류 하나씩 꺼낸다
    /// </summary>
    public Item MyItem
    {
        get
        {
            if (IsEmpty == false)
            {
                return items.Peek();
            }

            return null;
        }
    }

    /// <summary>
    /// 아이템의 실제 아이콘
    /// </summary>
    public Image MyIcon
    {
        get
        {
            return itemicon;
        }

        set
        {
            itemicon = value;
        }
    }

    /// <summary>
    /// 아이템 등급을 위한 슬롯 이미지
    /// </summary>
    public Image MySlotIcon
    {
        get
        {
            return slotIcon;
        }

        set
        {
            slotIcon = value;
        }
    }

    /// <summary>
    /// 셀렉트
    /// </summary>
    public GameObject MySelectIcon
    {
        get
        {
            return selectIcon;
        }

        set
        {
            selectIcon = value;
        }
    }

    /// <summary>
    /// 아이템의 갯수
    /// </summary>
    public int MyCount
    {
        get
        {
            return items.Count;
        }
    }

    public Text MyStackText
    {
        get
        {
            return stackText;
        }
    }

    public ObserverableStack<Item> MyObserverStackItem
    {
        get
        {
            return items;
        }
    }

    public bool IsPressed { get => isPressed; set => isPressed = value; }
    public ObserverableStack<Item> MyObserveItems { get => items; set => items = value; }

    private void Awake()
    {
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        SelectItem();
        isPressed = true;
        StartCoroutine(Charge());

        if (InventoryScript.MyInstance.FromSlot != null)
        {
            if (SwapItem(InventoryScript.MyInstance.FromSlot) || PutItemBack() || MergeItem(InventoryScript.MyInstance.FromSlot) ||
                PutOnItem(InventoryScript.MyInstance.FromSlot.items))
            {
                if (HandScript.MyInstance.MyMoveableIcon is HealthPotion)
                {
                    Call_ShiftDown();
                }

                HandScript.MyInstance.Drop();
                //InventoryScript.MyInstance.FromSlot = null;
                selectIcon.SetActive(false);
                isPressed = false; 
            }
        }

        if (CharacterPanel.MyInstance.PrevEquipment != null)
        {
            if (CharacterPanel.MyInstance.PrevEquipment.MySelectIcon.activeSelf == true)
            {
                CharacterPanel.MyInstance.PrevEquipment.DeSelectItem();
            }
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        saveChageTime = 0;
    }

    /// <summary>
    /// 슬롯에 아이템을 집어 넣는다
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        items.Push(item);

        itemicon.sprite = item.MyIcon;
        itemicon.color = Color.white;

        slotIcon.sprite = UIManager.MyInstance.MySlotBG_Brown;
        slotIcon.color = Color.white;

        SetItemColor(item, slotIcon);

        item.MySlot = this;

        return true;
    }

    public bool PutOnItem(ObserverableStack<Item> newItem)
    {
        if (IsEmpty || newItem.Peek().GetType() == MyItem.GetType())
        {
            int count = newItem.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItem.Pop());
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// 아이템을 제거한다
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        // 슬롯에 아이템이 있다면
        if (IsEmpty == false)
        {
            // 아이템을 꺼내고 갯수가 있는 아이템이면 숫자를 줄이고
            // 아니면 해당슬롯에서 없앤다
            InventoryScript.MyInstance.OnItemCountChanged(items.Pop());
        }
    }

    /// <summary>
    /// 아이템을 사용
    /// </summary>
    public void UseItem()
    {
        // 이 종류라면 아이템을 사용
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
        //else if (MyItem is Equip)
        //{
        //    //(MyItem as Equip).EquipArmor();
        //}
    }

    /// <summary>
    /// 아이템을 쌓는다
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool StackItem(Item item)
    {
        // 슬롯이 비어있지 않고, 같은 종류의 아이템이고, 현재 아이템이 설정한 최대갯수보다 작다면
        if (IsEmpty == false && item.GetType() == MyItem.GetType() && items.Count < MyItem.MyItemStackSize)
        {
            items.Push(item);
            item.MySlot = this;

            return true;
        }

        return false;
    }

    public bool PutItemBack()
    {
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            InventoryScript.MyInstance.FromSlot.MySlotIcon.color = Color.white;

            return true;
        }

        return false;
    }

    private bool SwapItem(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyItemStackSize)
        {
            // a슬롯에 있는 모든 아이템을 복사
            ObserverableStack<Item> tmpFrom = new ObserverableStack<Item>(from.items);

            // a슬롯을 비운다
            from.items.Clear();

            // b슬롯에 a슬롯에 있던 아이템을 놓는다
            from.PutOnItem(items);

            // b슬롯을 비우고
            items.Clear();

            // b슬롯에 있던 아이템은 a슬롯으로 옮긴다
            PutOnItem(tmpFrom);

            return true;
        }

        return false;
    }

    private bool MergeItem(SlotScript from)
    {
        if (IsEmpty) return false;
        if (from.MyItem.GetType() == MyItem.GetType() && IsFull)
        {
            int free = MyItem.MyItemStackSize - MyCount;

            for (int i = 0; i < free; i++)
            {
                AddItem(from.items.Pop());
            }

            return true;
        }

        return false;
    }

    public void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public void SelectItem()
    {
        if (selectIcon.activeSelf == false)
        {
            globalItem = MyItem;
            selectIcon.SetActive(true);
            InventoryScript.MyInstance.ShowDescription(this);
        }
    }

    public void EquipItem()
    {
        CharacterPanel.MyInstance.EquipItem(globalItem);

        foreach (SlotScript slot in BagScript.MyInstance.MySlots)
        {
            if (slot.MySelectIcon.activeSelf == true)
            {
                slot.MySelectIcon.SetActive(false);
            }
        }
        globalItem = null;
    }

    public void DeSelectItem()
    {
        globalItem = null;
    }

    public void SetItemColor(Item item, Image slotIcon)
    {
        if (item is Equip)
        {
            if ((item as Equip).MyItemQuality == ItemQuality.Common)
            {
                slotIcon.sprite = UIManager.MyInstance.MySlotBG_Brown;
                slotIcon.color = Color.white;
            }

            else if ((item as Equip).MyItemQuality == ItemQuality.Rare)
            {
                slotIcon.sprite = UIManager.MyInstance.MySlotBG_Green;
                slotIcon.color = Color.green;
            }

            else if ((item as Equip).MyItemQuality == ItemQuality.Unique)
            {
                slotIcon.sprite = UIManager.MyInstance.MySlotBG_Blue;
                slotIcon.color = new Color(88 / 255f, 179 / 255f, 255 / 255f);
            }

            else if ((item as Equip).MyItemQuality == ItemQuality.Epic)
            {
                slotIcon.sprite = UIManager.MyInstance.MySlotBG_Purple;
                slotIcon.color = new Color(255 / 255f, 64 / 255f, 250 / 255f);
            }

            else if ((item as Equip).MyItemQuality == ItemQuality.Legendary)
            {
                slotIcon.sprite = UIManager.MyInstance.MySlotBG_Red;
                slotIcon.color = Color.red;
            }
        }
        else if (item is HealthPotion)
        {
            slotIcon.sprite = UIManager.MyInstance.MySlotBG_Brown;
            slotIcon.color = Color.white;
        }
    }

    void Call_ShiftUp()
    {
        int currentsibling = uiMenu.gameObject.transform.GetSiblingIndex();
        if (currentsibling > 0)
        {
            currentsibling -= 1;
            uiMenu.gameObject.transform.SetSiblingIndex(currentsibling);
        }
    }

    public void Call_ShiftDown()
    {
        int currentsibling = uiMenu.gameObject.transform.GetSiblingIndex();
        if (currentsibling > 0)
        {
            currentsibling += 1;
            uiMenu.gameObject.transform.SetSiblingIndex(currentsibling);
        }
    }

    IEnumerator Charge()
    {
        float timePassed = Time.deltaTime;

        while (isPressed == true)
        {
            timePassed += Time.deltaTime;

            saveChageTime = timePassed;

            if (saveChageTime > 1.0f)
            {
                if (InventoryScript.MyInstance.FromSlot == null && IsEmpty == false)
                {
                    if (MyItem is HealthPotion)
                    {
                        Call_ShiftUp();
                    }

                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveableIcon);
                    InventoryScript.MyInstance.FromSlot = this;
                   
                }
            }

            yield return null;
        }
    }

}
