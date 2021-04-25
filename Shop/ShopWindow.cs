using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : Window
{
    private static ShopWindow instance;

    public static ShopWindow MyInstance
    {
        get
        {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<ShopWindow>();
            }

            return instance;
        }
    }

    [SerializeField]
    private ShopItemButton[] shopItemButton;

    [SerializeField]
    private Text pageNumber;

    [SerializeField] private Text itemName;
    [SerializeField] private Text itemGrade;
    [SerializeField] private Text itemDescription;
    [SerializeField] private Text MyGold;

    private List<List<ShopItem>> Pages = new List<List<ShopItem>>();
    private ShopItemButton prevShopItem;

    private int pageIndex;

    public ShopItemButton PrevShopItem { get => prevShopItem; }

    private void LateUpdate()
    {
        MyGold.text = PlayerMovement.MyInstance.MyGold.ToString();
    }

    /// <summary>
    /// 이거는 아이템 페이지를 만든다
    /// </summary>
    /// <param name="items"></param>
    public void CreatePage(ShopItem[] items)
    {
        Pages.Clear();
        // 상점아이템을 리스트로 만들어서 담고 
        List<ShopItem> page = new List<ShopItem>();

        // 아이템갯수만큼 돌려서
        for (int i = 0; i < items.Length; i++) {
            // 한 페이지에 아이템을 추가한다
            page.Add(items[i]);
            Instantiate(items[i].MyItem);

            // 만약 페이지에 아이템 갯수가 12개라면
            if (page.Count == 9 || i == items.Length - 1) {
                // 페이지를 새로 만든다
                Pages.Add(page);
                page = new List<ShopItem>();
            }
        }

        // 아이템 추가
        AddItems();
    }

    /// <summary>
    /// 아이템을 추가한다
    /// </summary>
    public void AddItems()
    {
        // 상점 페이지의 숫자를 표시해주고
        pageNumber.text = pageIndex + 1 + "/" + Pages.Count;

        // 페이지가 1이상이라는건 아이템이 있다는 뜻
        if (Pages.Count > 0) {
            // 페이지만큼 돌린다
            for (int i = 0; i < Pages[pageIndex].Count; i++) {
                // 페이지가 없지 않는 이상
                if (Pages[pageIndex][i] != null) {
                    // 아이템을 추가시켜준다
                    shopItemButton[i].ShopAddItem(Pages[pageIndex][i]);
                }
            }
        }
    }

    ///// <summary>
    ///// 이름 그대로임
    ///// </summary>
    //public void NextPage()
    //{
    //    if (pageIndex < Pages.Count - 1) {
    //        ClearButton();
    //        pageIndex++;
    //        AddItems();
    //    }
    //}

    ///// <summary>
    ///// 마찬가지로 이름 그대로
    ///// </summary>
    //public void PreviousPage()
    //{
    //    if (pageIndex > 0) {
    //        ClearButton();
    //        pageIndex--;
    //        AddItems();
    //    }
    //}

    public void ClearButton()
    {
        foreach (ShopItemButton btn in shopItemButton) {
            btn.gameObject.SetActive(false);
        }
    }

    public override void Open(NPC npc)
    {
        CreatePage((npc as Shop).MyShopItem);
        base.Open(npc);
    }

    public override void Close(NPC npc)
    {
        CreatePage((npc as Shop).MyShopItem);
        base.Close(npc);
    }

    public void SelectShopItem(ShopItemButton shopItemButton, ShopItem shopitem)
    {
        if (prevShopItem != null && shopItemButton != prevShopItem)
        {
            prevShopItem.MySelectIcon.SetActive(false);
            prevShopItem.MySlotIcon.color = Color.white;
        }

        if (shopItemButton != null) 
        {
            if (shopitem.MyItem is Equip) 
            {
                itemName.text = shopitem.MyItem.MyName;
                itemGrade.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColor[(shopitem.MyItem as Equip).MyItemQuality], (shopitem.MyItem as Equip).MyItemQuality);
                itemDescription.text = shopitem.MyItem.GetDescription();
            }
            else {
                itemName.text = shopitem.MyItem.MyName;
                //itemGrade.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColor[MyItemQuality], MyItemQuality);
                itemDescription.text = shopitem.MyItem.GetDescription();
            }
        }
        else 
        {
            itemDescription.text = "";
        }

        prevShopItem = shopItemButton;
    }

    public void BuyShopItem()
    {
        prevShopItem.BuyItem();
        itemName.text = "";
        itemGrade.text = "";
        itemDescription.text = "";
    }
}
