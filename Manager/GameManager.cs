using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void KillConfirm(string type);

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    public event KillConfirm killConfirmEvent;

    public void OnKillConfirm(string type)
    {
        if (killConfirmEvent != null)
        {
            killConfirmEvent(type);
        }
    }
}
