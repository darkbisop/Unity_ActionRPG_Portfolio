using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private Item itemPrefabs;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            InventoryScript.MyInstance.AddItem(itemPrefabs);
            //GameObject go = ObjectPoolingManager.MyInstance.GetObject("TestItem");
            this.gameObject.SetActive(false);
        }
    }
}
