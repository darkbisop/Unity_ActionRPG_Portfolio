using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;

    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }

    public IMoveableIcon MyMoveableIcon { get; set; }
    public Image MyIcon { get => icon; set => icon = value; }

    private Image icon;

    void Start()
    {
        icon = GetComponent<Image>();
    }

    void Update()
    {
        icon.transform.position = Input.mousePosition;

        if (Input.touchCount > 0)
        {   
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(i) == false && MyInstance.MyMoveableIcon != null)
                {
                    Touch tempTouchs = Input.GetTouch(i);

                    if (tempTouchs.phase == TouchPhase.Moved || tempTouchs.phase == TouchPhase.Began ||
                        tempTouchs.phase == TouchPhase.Stationary)
                    {
                        DeleteIcon();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 누르면 해당 아이콘의 정보를 가져온다
    /// </summary>
    /// <param name="moveable"></param>
    public void TakeMoveable(IMoveableIcon moveable)
    {
        this.MyMoveableIcon = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }

    public IMoveableIcon PutOn()
    {

        IMoveableIcon tmp = MyMoveableIcon;

        MyMoveableIcon = null;
        
        icon.color = new Color(0, 0, 0, 0);
        
        return tmp;
    }

    public void Drop()
    {
        MyMoveableIcon = null;

        icon.color = new Color(0, 0, 0, 0);

        InventoryScript.MyInstance.FromSlot = null;
    }

    public void DeleteIcon()
    {
        if (MyMoveableIcon is Item && InventoryScript.MyInstance.FromSlot != null)
        {
            (MyMoveableIcon as Item).MySlot.Clear();
        }

        // 이걸 부르고
        Drop();

        // 이전슬롯을 비운다
        InventoryScript.MyInstance.FromSlot = null;
    }
}
