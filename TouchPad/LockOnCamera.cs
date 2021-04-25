using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCamera : MonoBehaviour
{
    public GameObject cameraTarget;
    public float rotateSpeed;
    float rotate;
    public float offsetDistance;
    public float offsetHeight;
    public float smoothing;
    Vector3 offset;
    bool following = true;
    Vector3 lastPosition;

    public GameObject target;

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public float DelayTime;

    void Start()
    {
        lastPosition = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + offsetHeight, cameraTarget.transform.position.z - offsetDistance);
        offset = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + offsetHeight, cameraTarget.transform.position.z - offsetDistance);
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Q))
        {
            rotate = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotate = 1;
        }
        else
        {
            rotate = 0;
        }

        Vector3 FixedPos =
            new Vector3(
            target.transform.position.x + offsetX,
            target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ);
        transform.position = Vector3.Lerp(transform.position, FixedPos, DelayTime);

        //offset = Quaternion.AngleAxis(rotate * rotateSpeed, Vector3.up) * offset;
        //transform.position = cameraTarget.transform.position + offset;
        //transform.position = new Vector3(Mathf.Lerp(lastPosition.x, cameraTarget.transform.position.x + offset.x, smoothing * Time.deltaTime),
        //    Mathf.Lerp(lastPosition.y, cameraTarget.transform.position.y + offset.y, smoothing * Time.deltaTime),
        //    Mathf.Lerp(lastPosition.z, cameraTarget.transform.position.z + offset.z, smoothing * Time.deltaTime));

        //Vector3 dir = cameraTarget.transform.position - PlayerMovement.MyInstance.transform.position;
        //Quaternion rot = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5f);
        transform.LookAt(cameraTarget.transform.position);
        //transform.RotateAround(cameraTarget.transform.position, Vector3.up, PlayerMovement.MyInstance.transform.position.x * Time.deltaTime);
    }
}

