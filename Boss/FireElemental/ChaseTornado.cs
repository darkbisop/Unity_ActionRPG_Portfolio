using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTornado : MonoBehaviour
{
    private GameObject target;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 4.7f * Time.deltaTime);
    }
}
