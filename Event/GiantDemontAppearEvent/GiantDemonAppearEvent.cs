using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantDemonAppearEvent : MonoBehaviour
{
    public GameObject test;

    GameObject canvas_Overlay, canvas_Camera;

    private GameObject eventCamera;

    private float elpasedTime;
    private float maxTime = 10.0f;
    private bool isTest = false;
    private Coroutine test1Test;
    Quaternion rot = Quaternion.identity;

    public AudioClip roarSound;
    public AudioSource audioSource;

    private void Awake()
    {
        StartCoroutine(FadeOut.MyInstance.Fade(0.0f));

        canvas_Overlay = GameObject.Find("Canvas").transform.Find("Canvas_Overlay").gameObject;
        canvas_Overlay.GetComponent<CanvasGroup>().alpha = 0;

        canvas_Camera = GameObject.Find("Canvas").transform.Find("Canvas_Camera").gameObject;
        canvas_Camera.GetComponent<CanvasGroup>().alpha = 0;
        canvas_Camera.GetComponent<CanvasGroup>().blocksRaycasts = false;

        Event_UpImage.MyInstance.isEvent = true;
        Event_DownImage.MyInstance.isEvent = true;

        //eventCamera = GetComponent<Camera>();
        eventCamera = GameObject.Find("EventCamera");

        StartCoroutine(roar());
    }

    void Update()
    {
        StartCoroutine(test1());
    }

    IEnumerator test1()
    {
        rot.eulerAngles = new Vector3(6.395f, -72.777f, 0);
        eventCamera.transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 0.3f);
        yield return new WaitForSeconds(4.5f);
        eventCamera.GetComponent<GiantDemonAppearEvent>().enabled = false;
        eventCamera.GetComponent<Event_MoveCamera>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        test.SetActive(true);
        //eventCamera.GetComponent<Event_MoveCamera>().enabled = false;
    }

    IEnumerator roar()
    {
        yield return new WaitForSeconds(3.2f);
        audioSource.PlayOneShot(roarSound);
    }
}
