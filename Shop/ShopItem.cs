using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private int stackSize;

    public Item MyItem { get => item; }
    public int MyStackSize { get => stackSize; set => stackSize = value; }
}
