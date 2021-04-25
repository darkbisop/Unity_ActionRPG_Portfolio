using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastMoveEventCamera : MonoBehaviour
{
    private GameObject eventCamera, canvas_Overlay, canvas_Camera;
    private float elpasedTime;
    private float maxTime = 10.0f;

    void Start()
    {
        eventCamera = GameObject.Find("CameraObject").transform.Find("TargetCamera").gameObject;
        canvas_Overlay = GameObject.Find("Canvas").transform.Find("Canvas_Overlay").gameObject;
        canvas_Camera = GameObject.Find("Canvas").transform.Find("Canvas_Camera").gameObject;
    }

    void Update()
    {
        StartCoroutine(ControlScene());
    }

    IEnumerator ControlScene()
    {
        if (eventCamera.transform.position != new Vector3(1.08f, 3.59f, -30.90001f))
        {
            if (elpasedTime < maxTime)
            {
                elpasedTime += Time.deltaTime;
            }

            eventCamera.transform.position = Vector3.Lerp(eventCamera.transform.position, new Vector3(1.08f, 3.59f, -30.90001f), elpasedTime / maxTime);

            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = new Vector3(-5.356f, 0, 0);
            eventCamera.transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 4.5f);
        }

        yield return new WaitForSeconds(1.0f);
        Event_UpImage.MyInstance.isEvent = false;
        Event_DownImage.MyInstance.isEvent = false;
        yield return new WaitForSeconds(0.5f);
        canvas_Overlay.GetComponent<CanvasGroup>().alpha = 1;
        canvas_Camera.GetComponent<CanvasGroup>().alpha = 1;
        canvas_Camera.GetComponent<CanvasGroup>().blocksRaycasts = true;
        eventCamera.SetActive(false);
    }
}
