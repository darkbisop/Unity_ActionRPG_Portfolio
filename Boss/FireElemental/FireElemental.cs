using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElemental : MonoBehaviour
{
    private GameObject target;
    private Animator animator;
    private BossProgressBar ProgressBar;
    private Collider col;
    public List<string> SelectRandPattern = new List<string>() { "패턴1", "패턴2", "패턴3" };

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        col = GameObject.FindGameObjectWithTag("Sword").GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        ProgressBar = GetComponent<BossProgressBar>();
        ObjectPoolingManager.MyInstance.SetObject("fire-tornado", 4, this.transform);

        StartCoroutine(StartPattern());
    }


    IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(SelectPattern());
    }
    IEnumerator SelectPattern()
    {
        yield return new WaitForSeconds(2.0f);

        if (SelectRandPattern.Count == 0)
        {
            SelectRandPattern = new List<string>() { "패턴1", "패턴2", "패턴3" };
        }

        int rand = Random.Range(0, SelectRandPattern.Count);
        
        if (SelectRandPattern[rand] == "패턴1")
        {
            StartCoroutine(LeftPunch());
        }
        else if (SelectRandPattern[rand] == "패턴2")
        {
            StartCoroutine(RightPunch());
        }
        else if (SelectRandPattern[rand] == "패턴3")
        {
            StartCoroutine(ChaseFireTornado());
        }

        SelectRandPattern.RemoveAt(rand);
    }

    IEnumerator LeftPunch()
    {
        animator.SetTrigger("LeftAttack");
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(SelectPattern());
    }

    IEnumerator RightPunch()
    {
        animator.SetTrigger("RightAttack");
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(SelectPattern());
    }

    IEnumerator ChaseFireTornado()
    {
        animator.SetTrigger("ChannelTornado");
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("fire-tornado", new Vector3(target.transform.position.x + Random.Range(-10f, 10f), 0, target.transform.position.z + Random.Range(-10f, 10f)));
        go.SetActive(true);
        yield return new WaitForSeconds(6.0f);
        animator.SetTrigger("ChannelTornadoOver");
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(SelectPattern());
        ObjectPoolingManager.MyInstance.OffObject("fire-tornado");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            col.enabled = false;
            ProgressBar.health.Damaged(1);
            ProgressBar.damagedShrinkTimer = 0.2f;
        }
    }
}
