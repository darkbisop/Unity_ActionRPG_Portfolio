using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    private static BagScript instance;

    public static BagScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BagScript>();
            }

            return instance;
        }
    }

    [SerializeField] private GameObject slotPrefabs;

    private List<SlotScript> slots = new List<SlotScript>();

    public List<SlotScript> MySlots { get => slots; }

    public void AddSlot(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefabs, transform).GetComponent<SlotScript>();
            slot.MyIndex = i;
            MySlots.Add(slot);
        }
    }

    /// <summary>
    /// 가방안에 있는 슬롯에 아이템을 집어 넣는다
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        // 가방안에 슬롯을 검사해서 슬롯이 비어 있으면
        foreach (SlotScript slot in MySlots)
        {
            if (slot.IsEmpty)
            {
                // 하나씩 집어넣어주고 트루를 반환한다
                slot.AddItem(item);

                return true;
            }
        }

        // 아니라면 슬롯 비어있지 않다는 얘기
        return false;
    }
}
