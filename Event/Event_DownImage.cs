using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_DownImage : MonoBehaviour
{
    private static Event_DownImage instance;
    public bool isEvent = false;

    public static Event_DownImage MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Event_DownImage>();
            }

            return instance;
        }
    }

    void Update()
    {
        if (isEvent)
        {
            if (transform.localPosition.y < -550)
            {
                transform.localPosition += new Vector3(0, 5, 0);
            }
        }

        if (isEvent == false)
        {
            if (transform.localPosition.y > -720)
            {
                transform.localPosition -= new Vector3(0, 5, 0);
            }
        }
    }
}
