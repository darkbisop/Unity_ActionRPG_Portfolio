using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuButton : MonoBehaviour
{
    private GameObject touchField, overlay_JoyStick, camera_JoyStick;
    private GameObject shopButton, questButton;

    private void Start()
    {
        touchField = GameObject.Find("TouchField");
        overlay_JoyStick = GameObject.Find("OverlayTouch");
        camera_JoyStick = GameObject.Find("TouchPad").transform.Find("LeftTouch").gameObject;
        shopButton = GameObject.Find("UiMenuButton").transform.Find("ShopButton").gameObject;
        questButton = GameObject.Find("UiMenuButton").transform.Find("QuestGiverButton").gameObject;
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
        touchField.GetComponent<Image>().raycastTarget = touchField.GetComponent<Image>().raycastTarget == true ? false : true;
        InventoryScript.MyInstance.CheckOpenInventory();
        OnOffUI(canvasGroup);
        RemoveMoveableIcon(canvasGroup);
    }

    private void OnOffUI(CanvasGroup canvasGroup)
    {
        if (canvasGroup.alpha == 0)
        {
            overlay_JoyStick.SetActive(true);
            camera_JoyStick.SetActive(false);
        }
        else
        {
            overlay_JoyStick.SetActive(false);
            camera_JoyStick.SetActive(true);
        }
    }

    private void RemoveMoveableIcon(CanvasGroup canvasGroup)
    {
        if (canvasGroup.alpha == 0 || canvasGroup.alpha == 1)
        {
            if (HandScript.MyInstance.MyMoveableIcon != null)
            {
                if (HandScript.MyInstance.MyMoveableIcon is HealthPotion)
                {
                    SlotScript.MyInstance.Call_ShiftDown();
                }

                if (HandScript.MyInstance.MyMoveableIcon is Item)
                {
                    InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
                    InventoryScript.MyInstance.FromSlot.MySlotIcon.color = Color.white;
                    InventoryScript.MyInstance.FromSlot = null;

                    SlotScript.MyInstance.IsPressed = false;
                }

                HandScript.MyInstance.MyMoveableIcon = null;
                HandScript.MyInstance.MyIcon.sprite = null;
                HandScript.MyInstance.MyIcon.color = new Color(0, 0, 0, 0);
            }
        }
    }
}
