using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantDemon : MonoBehaviour
{
    private static GiantDemon instance;

    public static GiantDemon MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GiantDemon>();
            }

            return instance;
        }
    }

    [SerializeField] private Transform hitPos;
    private GameObject target;
    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private BossProgressBar ProgressBar;
    private Collider col, bangleCol;
    private JumpATK jumpATK;
    private CanvasGroup canvasGroup;

    private bool isChangePhase = false;
    public bool isDash = false;
    public float dist;
    public bool goingDash = false;
    public bool dashTrigger = false;
    public List<string> SelectRandPattern = new List<string>() { "패턴1", "패턴2", "패턴3", "패턴4" };
    public List<string> SelectdistPattern = new List<string>() { "패턴1", "패턴2" };

    public Collider dashCol;
    public AudioClip sound_1, sound_2, sound_3;
    public AudioSource audio;
    public AudioClip[] hitSound;
    public string type;

    private GameObject windSound, sand, bgm;

    Vector3 Dir;

    public Animator MyAnimator { get => animator; set => animator = value; }

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        jumpATK = GetComponent<JumpATK>();
        col = GameObject.FindGameObjectWithTag("Sword").GetComponent<BoxCollider>();
        bangleCol = GameObject.FindWithTag("Player").GetComponent<SphereCollider>();
        dashCol = GameObject.FindWithTag("DashCol").GetComponent<SphereCollider>();
        bangleCol.enabled = false;
        dashCol.enabled = false;

        animator = GetComponent<Animator>();
        ProgressBar = GetComponent<BossProgressBar>();
        //ProgressBar.health = new Health(20);

        canvasGroup = GameObject.Find("BossHpBar").GetComponent<CanvasGroup>();
        ObjectPoolingManager.MyInstance.SetObject("DashEffect", 1, GiantDemon.MyInstance.transform);

        windSound = GameObject.Find("BGM").transform.Find("Wind Loop").gameObject;
        sand = GameObject.Find("BGM").transform.Find("sand_fx").gameObject;
        bgm = GameObject.Find("BGM").transform.Find("battle_03_loop").gameObject;

        StartCoroutine(StartPattern());
    }

    void Update()
    {
        ChaseTarget();
        

        if (dashTrigger)
        {
            goingDash = true;
            dashCol.enabled = true;
            Dir = target.transform.position - this.transform.position;
            Vector3 look = Vector3.Slerp(this.transform.forward, Dir.normalized, Time.deltaTime * 9.0f);
            this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);
            Dir.Normalize();
        }

        if (goingDash)
        {
            transform.position += Dir * Time.deltaTime * 9.0f;
        }

        dist = Vector3.Distance(target.transform.position, this.transform.position);

        //if (ProgressBar.health.HealthAmount <= currentHp * 0.9)
        //{
        //    isChangePhase = true;
        //    animator.SetTrigger("ChangePhase");
        //    StopAllCoroutines();
        //}

        //if (ProgressBar.health.HealthAmount <= 0)
        //{
        //    StopAllCoroutines();
        //    ProgressBar.health.HealthAmount = 0;
        //    GameManager.MyInstance.OnKillConfirm(type);
        //}

    }

    IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(6.0f);
        canvasGroup.alpha = 1;
        StartCoroutine(SelectPattern());
        bgm.SetActive(true);
        windSound.SetActive(false);
        sand.SetActive(false);
    }

    public IEnumerator SelectPattern()
    {
        yield return new WaitForSeconds(2.0f);

        if (SelectRandPattern.Count == 0)
        {
            SelectRandPattern = new List<string>() { "패턴1", "패턴2", "패턴3", "패턴4" };
        }

        if (SelectdistPattern.Count == 0)
        {
            SelectdistPattern = new List<string>() { "패턴1", "패턴2" };
        }

        int rand = Random.Range(0, SelectRandPattern.Count);
        int distRand = Random.Range(0, SelectdistPattern.Count);

        if (dist >= 8.5f)
        {
            switch (SelectdistPattern[distRand])
            {
                case "패턴1":
                    StartCoroutine(JumpAttack());
                    break;
                case "패턴2":
                    StartCoroutine(ChaseFireTornado());
                    break;
            }

            SelectdistPattern.RemoveAt(distRand);
        }
        else
        {
            switch (SelectRandPattern[rand])
            {
                case "패턴1":
                    StartCoroutine(Attack1());
                    break;
                case "패턴2":
                    StartCoroutine(Attack2());
                    break;
                case "패턴3":
                    StartCoroutine(JumpAttack());
                    break;
                case "패턴4":
                    isDash = true;
                    animator.SetTrigger("DashPre");
                    break;
            }

            SelectRandPattern.RemoveAt(rand);
        }
    }

    IEnumerator Attack1()
    {
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(SelectPattern());
    }

    IEnumerator Attack2()
    {
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(SelectPattern());
    }

    IEnumerator JumpAttack()
    {
        animator.SetTrigger("JumpAttack");
        animator.SetTrigger("AfterAttack");
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(SelectPattern());
    }

    public void JumpTrigger()
    {
        jumpATK.isJump = true;
    }

    private void ChaseTarget()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("idle"))
        {
            Turn();
        }
    }

    IEnumerator ChaseFireTornado()
    {
        animator.SetTrigger("ChaseTornado");
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("fire-tornado", new Vector3(target.transform.position.x + Random.Range(-10f, 10f), 0, target.transform.position.z + Random.Range(-10f, 10f)));
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(SelectPattern());
        yield return new WaitForSeconds(6.0f);
        ObjectPoolingManager.MyInstance.OffObject("fire-tornado");
    }


    private void Turn()
    {
        Vector3 Dir = target.transform.position - this.transform.position;
        Vector3 look = Vector3.Slerp(this.transform.forward, Dir.normalized, Time.deltaTime * 9.0f);
        this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);
    }

    IEnumerator BangleAttack()
    {
        while (bangleCol.enabled == true)
        {
            ProgressBar.health.Damaged(1);
            ProgressBar.damagedShrinkTimer = 0.2f;
            StartCoroutine(hitEffect());
            RandSound();
            yield return new WaitForSeconds(0.4f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            col.enabled = false;
            ProgressBar.TakeDamage(5);
            //ProgressBar.health.Damaged(1);
            ProgressBar.damagedShrinkTimer = 0.2f;
            StartCoroutine(hitEffect());
            RandSound();
        }

        if (other.gameObject.tag == "Player")
        {
            if (bangleCol.enabled == true)
            {
                StartCoroutine(BangleAttack());
            }
        }

        if (GiantDemon.MyInstance.goingDash == true)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(PlayerMovement.MyInstance.DamageEffect());
                PlayerMovement.MyInstance.MyHealth.CurrentValue -= 3;
                audio.PlayOneShot(PlayerMovement.MyInstance.damageSound[0]);

                dashCol.enabled = false;
                goingDash = false;
                dashTrigger = false;
                isDash = false;
                animator.SetTrigger("DashFalse");
                ObjectPoolingManager.MyInstance.OffObject("DashEffect");
                StartCoroutine(SelectPattern());
            }
        }
    }

    IEnumerator hitEffect()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("TargetHitExplosion", hitPos);
        go.transform.position = hitPos.transform.position + new Vector3(Random.Range(0.1f, 0.3f), Random.Range(0.1f, 0.3f), Random.Range(0.1f, 0.3f));
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        go.SetActive(false);
    }

    IEnumerator DashEffect()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("DashEffect", hitPos);
        go.transform.position = hitPos.transform.position + new Vector3(0, 1.0f, 0);
        go.SetActive(true);
        yield return null;
        //go.SetActive(false);
    }

    private void RandSound()
    {
        int rand = Random.Range(1, 5);

        switch (rand)
        {
            case 1:
                audio.PlayOneShot(hitSound[0]);
                break;
            case 2:
                audio.PlayOneShot(hitSound[1]);
                break;
            case 3:
                audio.PlayOneShot(hitSound[2]);
                break;
            case 4:
                audio.PlayOneShot(hitSound[3]);
                break;
        }
    }

    public void PlaySound()
    {
        audio.PlayOneShot(sound_1, 1.0f);
    }

    public void PlaySound_2()
    {
        audio.PlayOneShot(sound_2, 1.0f);
    }

    public void PlaySound_3()
    {
        audio.PlayOneShot(sound_3, 1.0f);
    }

    public void DashRun()
    {
        dashTrigger = true; 
    }
}
