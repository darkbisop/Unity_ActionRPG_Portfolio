using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    private GameObject StageManager;

    private void Start()
    {
        SaveManager.MyInstance.Load();
    }
}
