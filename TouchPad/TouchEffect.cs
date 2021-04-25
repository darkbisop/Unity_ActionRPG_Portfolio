using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    [SerializeField] public Camera camera;
    //public float defaultTime = 0.05f;
    //float spawnsTime;

    private Touch touch;
    private Vector3 touchPos;

    void Start()
    {
        ObjectPoolingManager.MyInstance.SetObject("Fx_Touch", 30);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    StartCreate2();
        //}
        TouchInput();

        //spawnsTime += Time.deltaTime;
    }

    private void StartCreate2()
    {
        Vector3 touchPos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("Fx_Touch", touchPos);
        go.SetActive(true);
        StartCoroutine(OffEffect());
    }

    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                touch = Input.GetTouch(i);

                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    //touchPos = camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -Camera.main.transform.position.z));
                    //GameObject go = ObjectPoolingManager.MyInstance.GetObject("Fx_Touch", touchPos);
                    //go.SetActive(true);
                    StartCoroutine(OffEffect());
                }
            }
        }
    }

    IEnumerator OffEffect()
    {
        touchPos = camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -Camera.main.transform.position.z));
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("Fx_Touch", touchPos);
        go.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        go.SetActive(false);
        //ObjectPoolingManager.MyInstance.OffObject("Fx_Touch");
    }
}
