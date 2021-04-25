using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : NPC
{
    [SerializeField]
    private ShopItem[] shopItem;
    public ShopItem[] MyShopItem { get => shopItem; }
}
