using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButton : EventTrigger, IPointerClickHandler, IClickable
{
    public IUseable MyUseable { get; set; }
    private Stack<IUseable> useables = new Stack<IUseable>();

    public Button MyButton { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private Text coolTime;
    [SerializeField] private Image skillBackGRD;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text stackSize;
    [SerializeField] private Image nothingImage;

    bool isCheck = false;
    bool test = false;
    private int count;

    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public Text MyCoolTime { get => coolTime; set => coolTime = value; }
    public Image MySkillBackGRD { get => skillBackGRD; set => skillBackGRD = value; }
    public CanvasGroup MyCanvasGroup { get => canvasGroup; set => canvasGroup = value; }
    public Image MyBgIcon { get => nothingImage; set => nothingImage = value; }
    public Image MySlotIcon { get => nothingImage; set => nothingImage = value; }

    public int MyCount
    {
        get { return count; }
    }

    public Text MyStackText
    {
        get { return stackSize; }
    }

    public Stack<IUseable> MyUseables
    {
        get
        {
            return useables;
        }

        set
        {
            if (value.Count > 0)
            {
                MyUseable = value.Peek();
            }
            else
            {
                MyUseable = null;
            }

            useables = value;
        }
    }

    void Start()
    {
        MyButton = GetComponent<Button>();

        InventoryScript.MyInstance.itemCountChangeEvent += new ItemCountChanged(UpdateItemCount);
    }

    /// <summary>
    /// 액션 버튼을 눌렀을때
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        // 핸드아이콘이 비어있고
        if (HandScript.MyInstance.MyMoveableIcon == null)
        {
            // 아이콘이 셋팅 되었으면 사용한다
            if (MyUseable != null)
            {
                MyUseable.Use();
                CheckSkill();
            }
            else if (MyUseables != null && MyUseables.Count > 0)
            {
                MyUseables.Peek().Use();
            }
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        PlayerMovement.MyInstance.BangleSkillUp("BangleBangle");
    }

    /// <summary>
    /// 액션바에 세팅한다
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 핸드이미지에 아이콘이 있다면 
            if (HandScript.MyInstance.MyMoveableIcon != null && HandScript.MyInstance.MyMoveableIcon is IUseable)
            {
                if (HandScript.MyInstance.MyMoveableIcon is HealthPotion && SlotScript.MyInstance.IsPressed == false)
                {
                    SlotScript.MyInstance.Call_ShiftDown();
                }

                // 셋팅해준다
                SetUseable(HandScript.MyInstance.MyMoveableIcon as IUseable);
            }
        }
    }

    /// <summary>
    /// 아이콘 세팅하기
    /// </summary>
    /// <param name="useable"></param>
    public void SetUseable(IUseable useable)
    {
        if (useable is Item)
        {
            MyUseables = InventoryScript.MyInstance.GetUseables(useable);

            if (InventoryScript.MyInstance.FromSlot != null)
            {
                InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
                InventoryScript.MyInstance.FromSlot = null;
            }
        }
        else
        {
            MyUseables.Clear();
            this.MyUseable = useable;
        }

        count = MyUseables.Count;
        UpdateVisual(useable as IMoveableIcon);
    }

    /// <summary>
    /// 아이콘 이미지 업뎃
    /// </summary>
    /// <param name="moveable"></param>
    public void UpdateVisual(IMoveableIcon moveable)
    {
        if (HandScript.MyInstance.MyMoveableIcon != null)
        {
            HandScript.MyInstance.Drop();
        }

        MyIcon.sprite = moveable.MyIcon;
        MyIcon.color = Color.white;

        if (count > 1)
        {
            UIManager.MyInstance.UpdateStackSize(this);
        }

        else if (MyUseable is Skill)
        {
            // 스택카운트를 없앤다
            UIManager.MyInstance.ClearStackCount(this); 
        }
    }

    public void UpdateItemCount(Item item)
    {
        if (item is IUseable && MyUseables.Count > 0)
        {
            if (MyUseables.Peek().GetType() == item.GetType())
            {
                MyUseables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                count = MyUseables.Count;

                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    /// <summary>
    /// 스킬을 사용 했는지 체크
    /// </summary>
    private void CheckSkill()
    {
        if (isCheck == false)
        {
            if (SkillBook.MyInstance.GetSpell("BangleBangle") == MyUseable)
            {
                if (PlayerMovement.MyInstance.MyIsUseCharge == false)
                    StartCoroutine(CoolTime("BangleBangle"));
            }
            if (SkillBook.MyInstance.GetSpell("Destroy") == MyUseable)
            {
                if (PlayerMovement.MyInstance.MyIsUseDestroy == false)
                    StartCoroutine(CoolTime("Destroy"));
            }
            if (SkillBook.MyInstance.GetSpell("Rolling") == MyUseable)
            {
                StartCoroutine(CoolTime("Rolling"));
            }
        }
    }

    /// <summary>
    /// 스킬 쿨타임 계산
    /// </summary>
    /// <param name="skillName"></param>
    /// <returns></returns>
    public IEnumerator CoolTime(string skillName)
    {
        Skill skill = Array.Find(SkillBook.MyInstance.MySkills, x => x.MySkillName == skillName);

        float progress = 1.0f;
        float rate = 1.0f / skill.MySkillCoolTime;
        float timePass = Time.deltaTime;

        if ((isCheck == false && PlayerMovement.MyInstance.state == PlayerMovement.State.ATTACK ||
            isCheck == false && PlayerMovement.MyInstance.state == PlayerMovement.State.ANYACTION ||
            isCheck == false && PlayerMovement.MyInstance.state == PlayerMovement.State.ROLL))
        {
            while (skill.MySkillCoolTime > 0.5f)
            {
                isCheck = true;
                MyCanvasGroup.alpha = 1;

                MySkillBackGRD.fillAmount = Mathf.Lerp(0, 1, progress);

                progress -= rate * Time.deltaTime;

                timePass += Time.deltaTime;

                if (timePass > 0)
                {
                    MyCoolTime.text = (skill.MySkillCoolTime - timePass).ToString("F0");
                }

                if (skill.MySkillCoolTime - timePass <= 0)
                {
                    isCheck = false;
                    MyCanvasGroup.alpha = 0;
                    MySkillBackGRD.fillAmount = 1.0f;
                    MyCoolTime.text = "0";
                    PlayerMovement.MyInstance.MyIsUseCharge = false;
                    PlayerMovement.MyInstance.MyIsUseDestroy = false;
                    break;
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void TestTest(string skillName)
    {
        Skill skill = Array.Find(SkillBook.MyInstance.MySkills, x => x.MySkillName == skillName);
        float timePass = Time.deltaTime;

        while (skill.MySkillCoolTime > 0.5f)
        {
            timePass += Time.deltaTime;

            if (skill.MySkillCoolTime - timePass >= 0)
            {
                StartCoroutine(CoolTime("Destroy"));
            }
        }
    }
}
