using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveableIcon
{
    [SerializeField] private Sprite icon;
    [SerializeField] private int stackSize;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int Price;
    [SerializeField] private string type;

    private SlotScript slot;
    private ItemQuality itemQuality;

    public Sprite MyIcon { get => icon; }
    public int MyItemStackSize { get => stackSize; }
    public SlotScript MySlot { get => slot; set => slot = value; }
    public string MyName { get => name; set => name = value; }
    public string MyDescription { get => description; set => description = value; }
    public ItemQuality MyQuality { get => itemQuality; set => itemQuality = value; }
    public int MyPrice { get => Price; }
    public string MyType { get => type; }

    public virtual string GetDescription()
    {
        return string.Format("<color={0}>{1}\n\n{2}</color>", QualityColor.MyColor[MyQuality], MyName, MyDescription);
    }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}
