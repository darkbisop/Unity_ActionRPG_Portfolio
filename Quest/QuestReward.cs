using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestReward : EventTrigger, IPointerClickHandler
{
    [SerializeField] private Image Icon;
    [SerializeField] private Image slotIcon;
    [SerializeField] private Text text;

    Item item;

    private float saveChageTime;
    private bool isPressed = false;

    public void QuestAddItem(Item item)
    {
        this.item = item;
        Icon.enabled = true;
        text.enabled = true;
        slotIcon.enabled = true;
        Icon.sprite = item.MyIcon;
        slotIcon.sprite = UIManager.MyInstance.MySlotBG_Red;
    }

    public void RemoveQuestReward(Item item)
    {
        this.item = null;
        Icon.enabled = false;
        text.enabled = false;
        slotIcon.enabled = false;
        Icon.sprite = null;
        slotIcon.sprite = null;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        StartCoroutine(Charge()); 
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        saveChageTime = 0;
        UIManager.MyInstance.HideTooltip();
    }

    IEnumerator Charge()
    {
        float timePassed = Time.deltaTime;

        while (isPressed == true)
        {
            timePassed += Time.deltaTime;

            saveChageTime = timePassed;

            if (saveChageTime > 0.5f)
            {
                UIManager.MyInstance.ShowTooltip(new Vector2(-0.4f, 1.0f), transform.position, item);
            }

            yield return null;
        }
    }

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    UIManager.MyInstance.HideTooltip();
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    UIManager.MyInstance.ShowTooltip(new Vector2(0, 1), transform.position, item);
    //}
}
