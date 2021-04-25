using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string type;

    public string MyType { get => type; }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        GameManager.MyInstance.OnKillConfirm("GiantDemon");
    //    }
    //}

    //public void TakeDamage()
    //{
    //    GameManager.MyInstance.OnKillConfirm(this);
    //}
}
