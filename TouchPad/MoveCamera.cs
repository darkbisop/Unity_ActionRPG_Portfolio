using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform lookAt;
    public TouchField TouchField;
    private Vector3 dir;

    private const float Y_ANGLE_MIN = 0;
    private const float Y_ANGLE_MAX = 50f;

    private float distance = 16f;
    private float currentX = 0;
    private float currentY = 0;
    private float rotSpeed = 3.5f;

    void Update()
    {
        currentX += TouchField.TouchDist.x * rotSpeed * Time.deltaTime;
        currentY += TouchField.TouchDist.y * rotSpeed * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        dir = new Vector3(0, 3.5f, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        transform.position = lookAt.position + rotation * dir;
        transform.LookAt(lookAt.transform.position + (Vector3.up * 5.0f));
    }

    public IEnumerator ShakeIt()
    {
        float duration = 0.5f;
        float passTime = 0.0f;
        float oldCurrentX = currentX;
        float oldCurrentY = currentY;

        while (passTime < duration)
        {
            currentX = Random.Range(-0.5f, 0.5f) * 5.0f;
            currentY = Random.Range(-0.5f, 0.5f) * 5.0f;

            transform.position = transform.position + new Vector3(currentX, currentY, 0);

            passTime += Time.deltaTime;
            yield return null;
        }

        currentX = oldCurrentX;
        currentY = oldCurrentY;
    }
}
