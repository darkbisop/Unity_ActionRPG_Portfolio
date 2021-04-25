using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_MoveCamera : MonoBehaviour
{
    private GameObject eventCamera, targetCamera;
    private float elpasedTime;
    private float maxTime = 10.0f;

    void Start()
    {
        eventCamera = GameObject.Find("EventCamera");
        targetCamera = GameObject.Find("CameraObject").transform.Find("TargetCamera").gameObject;
    }

    void Update()
    {
        StartCoroutine(test());
    }

    IEnumerator test()
    {
        if (eventCamera.transform.position != new Vector3(2.085597f, 5.063839f, 2.115792f))
        {
            if (elpasedTime < maxTime)
            {
                elpasedTime += Time.deltaTime;
            }

            eventCamera.transform.position = Vector3.Lerp(eventCamera.transform.position, new Vector3(2.085597f, 5.063839f, 2.115792f), elpasedTime / maxTime);

            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = new Vector3(-50.255f, -29.575f, 0.164f);
            eventCamera.transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 4.5f);
        }

        yield return new WaitForSeconds(0.1f);
        eventCamera.SetActive(false);
        targetCamera.SetActive(true);


    }
}
