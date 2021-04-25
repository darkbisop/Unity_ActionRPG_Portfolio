using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcOpenEvent : MonoBehaviour
{
    [SerializeField] private GameObject button;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            button.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            button.SetActive(false);
        }
    }
}
