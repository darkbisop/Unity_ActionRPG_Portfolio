using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    private static Shake Instance;

    private GameObject eventOnlyCamera, shakeCamera;
    public float shakeAmount;
    public AudioClip crashSound;
    public AudioSource audioSource;

    public static Shake MyInstance
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<Shake>();
            }

            return Instance;
        }
    }

    void Awake()
    {
        eventOnlyCamera = GameObject.Find("CameraObject").transform.Find("TargetCamera").gameObject;
        shakeCamera = GameObject.Find("CameraObject").transform.Find("Main Camera").gameObject;
    }

    public IEnumerator ShakeIt()
    {
        audioSource.PlayOneShot(crashSound, 0.6f);
        float duration = 0.5f;

        // 지나간 시간을 누적할 변수
        float passTime = 0.0f;

        // 진동시간 동안 루프를 순회함
        while (passTime < duration)
        {
            // 카메라의 위치를 변경
            if (eventOnlyCamera.activeSelf != false)
                eventOnlyCamera.transform.localPosition = eventOnlyCamera.transform.localPosition + Random.insideUnitSphere * shakeAmount;
            else
                shakeCamera.transform.localPosition = shakeCamera.transform.localPosition + Random.insideUnitSphere * shakeAmount;
                
            // 진동시간을 누적
            passTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator ShakeIt(float shakeAmount)
    {
        float duration = 0.5f;

        // 지나간 시간을 누적할 변수
        float passTime = 0.0f;

        // 진동시간 동안 루프를 순회함
        while (passTime < duration)
        {
            // 카메라의 위치를 변경
            if (eventOnlyCamera.activeSelf != false)
                eventOnlyCamera.transform.localPosition = eventOnlyCamera.transform.localPosition + Random.insideUnitSphere * shakeAmount;
            else
                shakeCamera.transform.localPosition = shakeCamera.transform.localPosition + Random.insideUnitSphere * shakeAmount;

            // 진동시간을 누적
            passTime += Time.deltaTime;
            yield return null;
        }
    }
}
