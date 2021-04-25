using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Item/Bag", order = 1)]
public class BagItem : Item, IUseable
{
    private int slots;

    [SerializeField] private GameObject bagPrefab;

    public BagScript MyBagScript { get; set; }
    public int MyBagSlot { get => slots; }

    public void Init(int slot)
    {
        this.slots = slot;
    }

    public void Use()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlot(slots);
    }
}
