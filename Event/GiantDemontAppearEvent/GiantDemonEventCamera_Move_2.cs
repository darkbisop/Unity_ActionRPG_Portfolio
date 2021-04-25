using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantDemonEventCamera_Move_2 : MonoBehaviour
{
    private float elpasedTime;
    private float maxTime = 10.0f;
    private GameObject eventCamera;
    public AudioClip roarSound;
    public AudioSource audioSource;

    void Start()
    {
        eventCamera = GameObject.Find("CameraObject").transform.Find("TargetCamera").gameObject;

        StartCoroutine(ControlScene());
        StartCoroutine(roar());
    }

    void Update()
    {
        Quaternion rot = Quaternion.identity;

        if (eventCamera.transform.position != new Vector3(2.099266f, 3.627826f, -1.596834f))
        {
            if (elpasedTime < maxTime)
            {
                elpasedTime += Time.deltaTime;
            }

            eventCamera.transform.position = Vector3.Lerp(eventCamera.transform.position, new Vector3(2.099266f, 3.627826f, -1.596834f), elpasedTime / maxTime);
            rot.eulerAngles = new Vector3(0.035f, -16.398f, 0);
            eventCamera.transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3.5f);
        }

        //if (eventCamera.transform.position != new Vector3(1.593361f, 2.517796f, -0.6308196f))
        //{
        //    if (elpasedTime < maxTime)
        //    {
        //        elpasedTime += Time.deltaTime;
        //    }

        //    eventCamera.transform.position = Vector3.Lerp(eventCamera.transform.position, new Vector3(1.593361f, 2.517796f, -0.6308196f), elpasedTime / maxTime);
        //    rot.eulerAngles = new Vector3(-4.949f, -12.616f, 0);
        //    eventCamera.transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3.5f);
        //}
    }

    IEnumerator ControlScene()
    {
        yield return new WaitForSeconds(4.5f);
        eventCamera.GetComponent<GiantDemonEventCamera_Move_2>().enabled = false;
        eventCamera.GetComponent<LastMoveEventCamera>().enabled = true;
    }

    IEnumerator roar()
    {
        yield return new WaitForSeconds(2.3f);
        audioSource.PlayOneShot(roarSound);
    }
}
