using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    public Transform effectPos;
    private Collider col, bangleCol;

    [SerializeField] private AudioClip attackSound_1, attackSound_2, attackSound_3, attackSound_4;
    [SerializeField] private AudioClip swingSound_1, swingSound_2, swingSound_3, swingSound_4, swingSound_5, swingSound_6;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        col = GameObject.FindGameObjectWithTag("Sword").GetComponent<BoxCollider>();
        col.enabled = false;
       
        ObjectPoolingManager.MyInstance.SetObject("Slash_1", 1, effectPos);
        ObjectPoolingManager.MyInstance.SetObject("Slash_2", 1, effectPos);
        ObjectPoolingManager.MyInstance.SetObject("Slash_3", 1, effectPos);
        ObjectPoolingManager.MyInstance.SetObject("SwordSpinBlue", 1, transform);
    }

    public void AttackEffect()
    {
        if (PlayerMovement.MyInstance.state == PlayerMovement.State.ATTACK) StartCoroutine(OnEffect());
        else StopCoroutine(OnEffect());
    }

    IEnumerator OnEffect()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("Slash_1");
        go.transform.position = effectPos.transform.position + new Vector3(0, 0.8f, 0);
        go.SetActive(true);
        audioSource.PlayOneShot(attackSound_1);
        PlaySwingSound();
        yield return new WaitForSeconds(0.5f);
        ObjectPoolingManager.MyInstance.OffObject("Slash_1");
    }

    public void AttackEffect_2()
    {
        StartCoroutine(OnEffect_2());
    }

    IEnumerator OnEffect_2()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("Slash_2");
        go.SetActive(true);
        audioSource.PlayOneShot(attackSound_2);
        PlaySwingSound();
        yield return new WaitForSeconds(0.5f);
        ObjectPoolingManager.MyInstance.OffObject("Slash_2");
    }

    public void AttackEffect_3()
    {
        StartCoroutine(OnEffect_3());
    }

    IEnumerator OnEffect_3()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("Slash_3");
        go.transform.position = effectPos.transform.position + new Vector3(0, 0.27f, 0);
        go.SetActive(true);
        audioSource.PlayOneShot(attackSound_3);
        PlaySwingSound();
        yield return new WaitForSeconds(0.5f);
        ObjectPoolingManager.MyInstance.OffObject("Slash_3");
    }

    public void AttackEffect_4()
    {
        StartCoroutine(OnEffect_4());
    }

    IEnumerator OnEffect_4()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("SwordSpinBlue");
        go.transform.position = PlayerMovement.MyInstance.transform.position + new Vector3(0, 1.0f, 0);
        go.SetActive(true);
        audioSource.PlayOneShot(attackSound_4);
        audioSource.PlayOneShot(swingSound_6, 0.7f);
        yield return new WaitForSeconds(0.5f);
        ObjectPoolingManager.MyInstance.OffObject("SwordSpinBlue");
    }

    public void DestroySkillEffect()
    {
        if (PlayerMovement.MyInstance.state == PlayerMovement.State.DESTROYSKILL)
            StartCoroutine(OnDestroySkillEffect());
    }

    IEnumerator OnDestroySkillEffect()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("SKill_Destroy");
        go.transform.position = effectPos.transform.position;
        go.transform.rotation = PlayerMovement.MyInstance.transform.rotation;
        go.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        ObjectPoolingManager.MyInstance.OffObject("SKill_Destroy");
    }

    public void EnableCollider()
    {
        StartCoroutine(ColliderSetEnable());
    }

    IEnumerator ColliderSetEnable()
    {
        col.enabled = true;
        yield return new WaitForSeconds(0.04f);
        col.enabled = false;
    }

    private void PlaySwingSound()
    {
        int rand = Random.Range(1, 5);
        switch (rand)
        {
            case 1:
                audioSource.PlayOneShot(swingSound_1, 0.5f);
                break;
            case 2:
                audioSource.PlayOneShot(swingSound_2, 0.5f);
                break;
            case 3:
                audioSource.PlayOneShot(swingSound_3, 0.5f);
                break;
            case 4:
                audioSource.PlayOneShot(swingSound_4, 0.5f);
                break;
            case 5:
                audioSource.PlayOneShot(swingSound_5, 0.5f);
                break;
            default:
                break;
        }
    }
}
