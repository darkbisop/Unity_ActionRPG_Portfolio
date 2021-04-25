using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    /// <summary>
    /// 상점에서 파는 아이템의 아이콘
    /// </summary>
    [SerializeField]
    private Image icon;

    /// <summary>
    /// 아이템 이름
    /// </summary>
    [SerializeField]
    private Text title;

    /// <summary>
    /// 가격
    /// </summary>
    [SerializeField]
    private Text price;

    /// <summary>
    /// 갯수
    /// </summary>
    [SerializeField]
    private Text stackSize;

    [SerializeField]
    private Image slotIcon;

    [SerializeField] private GameObject selectIcon;

    private ShopItem shopItem;
    private static ShopItem saveShopItem;
    private Equip equipment;

    public Image MySlotIcon { get => slotIcon; set => slotIcon = value; }
    public Equip MyEquip { get => equipment; }
    public Text MyTitle { get => title; set => title = value; }
    public Text StackSize { get => stackSize; set => stackSize = value; }
    public static ShopItem SaveShopItem { get => saveShopItem; set => saveShopItem = value; }
    public GameObject MySelectIcon { get => selectIcon; set => selectIcon = value; }

    /// <summary>
    /// 상점에 아이템을 추가 시킨다
    /// </summary>
    /// <param name="shopItem"></param>
    public void ShopAddItem(ShopItem shopItem)
    {
        this.shopItem = shopItem;

        // 아이템 갯수가 1개 이상
        if (shopItem.MyStackSize > 0) {
            // 아이콘 담고
            icon.sprite = shopItem.MyItem.MyIcon;

            // 이건 내가 추가 한거지만 아이템의 종류가 장비라면
            if (shopItem.MyItem is Equip) {
                // 장비 등급에 맞춰서 색깔을 집어넣고
                SlotScript.MyInstance.SetItemColor(shopItem.MyItem, slotIcon);
                title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColor[(shopItem.MyItem as Equip).MyItemQuality], shopItem.MyItem.MyName);
            }
            else {
                SlotScript.MyInstance.SetItemColor(shopItem.MyItem, slotIcon);
                title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColor[shopItem.MyItem.MyQuality], shopItem.MyItem.MyName);
            }

            stackSize.text = shopItem.MyStackSize.ToString();


            if (shopItem.MyStackSize != 0)
            {
                stackSize.text = shopItem.MyStackSize.ToString();
            }

            // 아이템 가격을 나타낸다
            if (shopItem.MyItem.MyPrice > 0) {
                price.text = "가격 : " + shopItem.MyItem.MyPrice.ToString();
            }
            else {
                // 아니면 빈칸으로
                price.text = string.Empty;
            }

            // 그리고 활성화 해준다
            gameObject.SetActive(true);
        }

    }

    public void SelectItem()
    {
        if (selectIcon.activeSelf == false)
        {
            saveShopItem = this.shopItem;
            selectIcon.SetActive(true);
            ShopWindow.MyInstance.SelectShopItem(this, this.shopItem);
        } 
    }

    /// <summary>
    /// 아이템을 산다
    /// </summary>
    public void BuyItem()
    {
        if (saveShopItem != null)
        {
            // 돈을 빼주고
            if (PlayerMovement.MyInstance.MyGold >= shopItem.MyItem.MyPrice && shopItem.MyStackSize > 0)
            {

                PlayerMovement.MyInstance.MyGold -= shopItem.MyItem.MyPrice;

                shopItem.MyStackSize--;

                // 갯수를 표시해준다
                stackSize.text = shopItem.MyStackSize.ToString();

                // 해당 아이템의 갯수가 없으면
                if (shopItem.MyStackSize == 0)
                {
                    // 품목을 삭제하고 툴팁도 안보이게한다
                    gameObject.SetActive(false);
                }

                if (shopItem.MyItem as Equip)
                {
                    InventoryScript.MyInstance.AddItem(saveShopItem.MyItem);
                    //saveShopItem = null;
                }
                else
                {
                    InventoryScript.MyInstance.AddItem(Instantiate(saveShopItem.MyItem));
                }
            }

            else if (PlayerMovement.MyInstance.MyGold < shopItem.MyItem.MyPrice)
            {
                MessageFeedManager.MyInstance.WriteMessage(string.Format("골드가 부족합니다!"));
            }
        }
    }
}
