using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private Transform pos, rightPos, leftPos, introPos;
    [SerializeField] private Camera camera;
    private Collider col;

    public AudioClip groundSound, impactSound, rocketSound, exploSound;
    public AudioSource audio;

    void Start()
    {
        col = GameObject.FindGameObjectWithTag("BossAtkCol").GetComponent<SphereCollider>();
        col.enabled = false;
    }

    IEnumerator Effect_DustDirtyPoofSoft_Big()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("DustDirtyPoofSoft_Big_Brown", leftPos);
        go.SetActive(true);
        audio.PlayOneShot(rocketSound, 0.6f);
        StartCoroutine(Shake.MyInstance.ShakeIt(0.4f));
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("DustDirtyPoofSoft_Big_Brown");
    }

    IEnumerator Effect_LeftPunchDust()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("DustDirtyPoofSoft_Brown", leftPos);
        go.SetActive(true);
        audio.PlayOneShot(rocketSound, 0.6f);
        StartCoroutine(Shake.MyInstance.ShakeIt(0.2f));
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("DustDirtyPoofSoft_Brown");
    }

    IEnumerator Effect_RightPunchDust()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("DustDirtyPoofSoft_Brown", rightPos);
        go.SetActive(true);
        audio.PlayOneShot(rocketSound, 0.6f);
        StartCoroutine(Shake.MyInstance.ShakeIt(0.2f));
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("DustDirtyPoofSoft_Brown");
    }

    IEnumerator Effect_Dust()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("DustDirtyPoofSoft_Brown", rightPos);
        go.SetActive(true);
        audio.PlayOneShot(groundSound);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("Dust");
    }

    IEnumerator Effect_Rock()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("ROCK", rightPos);
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("ROCK");
    }

    IEnumerator Effect_DustLeft()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("Dust", leftPos);
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("Dust");
    }

    IEnumerator Effect_RockLeft()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("ROCK", leftPos);
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("ROCK");
    }

    IEnumerator Effect_ImpactGround()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("vfx_MagicAbility_Impact_Ground", transform.position + new Vector3(0, 1.0f, 0));
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("vfx_MagicAbility_Impact_Ground");
    }

    IEnumerator Effect_GrenadeExplosionFire()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("DustDirtyPoofSoft_Big_Brown", pos);
        go.SetActive(true);
        audio.PlayOneShot(impactSound, 0.6f);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("DustDirtyPoofSoft_Big_Brown");
    }

    IEnumerator Effect_NovaFireRed()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("NovaFireRed", pos);
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("NovaFireRed");
    }

    IEnumerator Effect_NovaFireRed_Small()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("NovaFireRed_Small", pos);
        go.SetActive(true);
        audio.PlayOneShot(exploSound, 0.6f);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("NovaFireRed_Small");
    }

    IEnumerator Effect_CartoonyBodySlam()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("CartoonyBodySlam", pos);
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("CartoonyBodySlam");
    }

    IEnumerator Effect_CartoonyBodySla_Small()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("CartoonyBodySlam_Small", pos);
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("CartoonyBodySlam_Small");
    }

    IEnumerator Effect_ImpactDust()
    {
        Vector3 pos = camera.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, -Camera.main.transform.position.z + 20f));
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("IntroDust_Brown", pos);
        go.SetActive(true);
        StartCoroutine(Effect_DustDirtyPoofSoft_Big());
        audio.PlayOneShot(impactSound);
        StartCoroutine(Shake.MyInstance.ShakeIt(0.6f));
        yield return new WaitForSeconds(2.0f);
        ObjectPoolingManager.MyInstance.OffObject("IntroDust_Brown");
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
}
