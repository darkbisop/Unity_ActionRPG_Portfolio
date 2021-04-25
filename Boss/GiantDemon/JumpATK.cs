using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpATK : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos, centerPos;
    public float maxHeight = 2.0f;
    public float totalTime = 3.0f;

    // >> 높이 관련
    float power = 100.0f;
    float currPower = 0.0f;
    float gravity = 0.0f;
    float elapsedTime_Y = 0.0f;
    // << 높이 관련

    // >> xz 이동 관련
    Vector3 moveDir;
    float moveSpeed;
    float elapsedTime_XZ = 0.0f;

    public bool isJump = false;
    // << xz 이동 관련

    // Use this for initialization
    void Start()
    {
        transform.position = startPos.position;

        // >> 높이 관련
        currPower = power;

        gravity = -power / totalTime;

        float height = startPos.position.y + (power * totalTime) + (0.5f * gravity * totalTime * totalTime);

        // 비율에 맞춘 초기 스피드 height: power = maxHeight : x;
        power = power * maxHeight / height;

        gravity = -power / totalTime;

        height = startPos.position.y + (power * totalTime) + (0.2f * gravity * totalTime * totalTime);

        elapsedTime_Y = 0.0f;
        // << 높이 관련

        // >> xz 이동 관련




        elapsedTime_XZ = 0.0f;
        // << xz 이동 관련
    }

    // Update is called once per frame
    void Update()
    {
        if (isJump == true)
        {
            if (elapsedTime_XZ <= totalTime * 1.4f)
            {
                if (GiantDemon.MyInstance.isDash == true)
                {
                    moveSpeed = (centerPos.position - startPos.position).magnitude / (totalTime * 0.7f);
                    transform.position += moveDir * moveSpeed * Time.deltaTime;

                    elapsedTime_XZ += Time.deltaTime;
                }
                else
                {
                    moveSpeed = (endPos.position - startPos.position).magnitude / (totalTime * 1.0f);
                    transform.position += moveDir * moveSpeed * Time.deltaTime;

                    elapsedTime_XZ += Time.deltaTime;
                }
            }

            StartCoroutine(zero());
        }
        else
        {
            if (GiantDemon.MyInstance.isDash == true) moveDir = (centerPos.position - startPos.position).normalized;
            else moveDir = (endPos.position - startPos.position).normalized;
        }
        // << xz 이동 관련
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            // >> 높이 관련
            if (elapsedTime_Y <= totalTime * 1.0f)
            {
                elapsedTime_Y += Time.fixedDeltaTime;

                // 1) v = v0 + at
                currPower = power + gravity * elapsedTime_Y;

                // 2) v = v0 + 1/2(at^2)
                //currPower = power + gravity * elapsedTime_Y * elapsedTime_Y * 0.5f;
                transform.position += Vector3.up * currPower * Time.fixedDeltaTime;

                // >> axis, angle
                Vector3 axis = Vector3.Cross(moveDir, Vector3.up);
                float angle = currPower / power;
                //transform.rotation = Quaternion.AngleAxis(angle, axis);
                // << axis, angle
            }
            // << 높이 관련
        }
    }

    IEnumerator zero()
    {
        yield return new WaitForSeconds(2.0f);
        elapsedTime_XZ = 0.0f;
        elapsedTime_Y = 0.0f;
        isJump = false;
    }
}
