using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField] private ActionButton[] actionButtons;

    [SerializeField] private Sprite slotBG_Green;
    [SerializeField] private Sprite slotBG_Blue;
    [SerializeField] private Sprite slotBG_Purple;
    [SerializeField] private Sprite slotBG_Red;
    [SerializeField] private Sprite slotBG_Brown;
    [SerializeField] private GameObject toolTip;
    [SerializeField] private RectTransform tooltipRect;

    public Sprite MySlotBG_Green { get => slotBG_Green; }
    public Sprite MySlotBG_Blue { get => slotBG_Blue; }
    public Sprite MySlotBG_Purple { get => slotBG_Purple; }
    public Sprite MySlotBG_Red { get => slotBG_Red; }
    public Sprite MySlotBG_Brown { get => slotBG_Brown; }

    private Text toolipText;

    private void Start()
    {
        ObjectPoolingManager.MyInstance.SetObject("TestItem", 3);
        toolipText = toolTip.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameObject go = ObjectPoolingManager.MyInstance.GetObject("TestItem");
            go.transform.position = new Vector3(PlayerMovement.MyInstance.transform.position.x + 2.0f, PlayerMovement.MyInstance.transform.position.y + 2.0f, PlayerMovement.MyInstance.transform.position.z + 2.0f);
            go.SetActive(true);
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.color = new Color(0, 0, 0, 0);
        clickable.MyIcon.color = Color.white;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }
            
        
        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MySlotIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void ShowTooltip(Vector2 pivot, Vector3 position, Item questItem)
    {
        tooltipRect.pivot = pivot;
        toolTip.SetActive(true);
        toolTip.transform.position = position;
        toolipText.text = questItem.GetDescription();
    }

    public void HideTooltip()
    {
        toolTip.SetActive(false);
    }
}
