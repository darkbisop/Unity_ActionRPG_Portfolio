using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_UpImage : MonoBehaviour
{
    private static Event_UpImage instance;
    public bool isEvent = false;

    public static Event_UpImage MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Event_UpImage>();
            }

            return instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEvent)
        {
            if (transform.localPosition.y > 450)
            {
                transform.localPosition += new Vector3(0, -5, 0);
            }
        }

        if (isEvent == false)
        {
            if (transform.localPosition.y < 620)
            {
                transform.localPosition += new Vector3(0, 5, 0);
            }
        }
    }
}
