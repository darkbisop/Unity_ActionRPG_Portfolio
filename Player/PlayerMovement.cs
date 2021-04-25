using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public enum State { ANYACTION, ROLL, ATTACK, CHARGESKILL, CHARGEMOVE, TIRED, DESTROYSKILL }

    private static PlayerMovement instance;

    public static PlayerMovement MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerMovement>();
            }

            return instance;
        }
    }

    [SerializeField] private ProgressBar health;
    [SerializeField] private ProgressBar mana;
    [SerializeField] private ProgressBar stamina;

    public State state;
    private Animator animator;
    private AnimatorStateInfo stateInfo;
    private Transform cam;
    private Vector3 camFoward, camDir;
    private Rigidbody rigidbody;
    private Collider bangleCol;

    public AudioClip bangleSound, bangleSwing, chargeSound;
    public AudioClip[] damageSound;
    public AudioSource audio;

    private float moveSpeed = 7.0f;

    private float nextRolling = 0.0f;
    private readonly float rollingRate = 1.0f;

    private float nextCharge = 0.0f;
    private readonly float ChargegRate = 8.0f;

    private float nextDestroy = 0.0f;
    private readonly float DestroyRate = 10.0f;

    public bool isMove = false;
    private bool isPressed = false;
    private bool isConfused = false;
    private bool isUseDestroy = false;
    private bool isUseCharge = false;
    private float saveChageTime;
    private int Gold;

    float h, v;

    private float attack = 5;
    private float defence = 5;
    private float cri = 5;

    private float atkPoint = 5;
    private float defPoint = 5;
    private float hpPoint = 5;
    private float criPoint = 5;
    private float staPoint = 5;

    private float maxHp = 100;
    private float maxSta = 100;

    public float MyAttack { get => attack; set => attack = value; }
    public float MyDefence { get => defence; set => defence = value; }
    public float MyCri { get => cri; set => cri = value; }

    public float AtkPoint { get => atkPoint; set => atkPoint = value; }
    public float DefPoint { get => defPoint; set => defPoint = value; }
    public float HpPoint { get => hpPoint; set => hpPoint = value; }
    public float CriPoint { get => criPoint; set => criPoint = value; }
    public float StaPoint { get => staPoint; set => staPoint = value; }

    public int MyGold { get => Gold; set => Gold = value; }
    public float NextDestroy { get => nextDestroy; set => nextDestroy = value; }

    public float DestroyRate1 => DestroyRate;

    public bool MyIsUseDestroy { get => isUseDestroy; set => isUseDestroy = value; }
    public bool MyIsUseCharge { get => isUseCharge; set => isUseCharge = value; }

    public ProgressBar MyHealth { get => health; set => health = value; }
    public ProgressBar MyStamina { get => stamina; set => stamina = value; }

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float MaxSta { get => maxSta; set => maxSta = value; }

    //public Transform lookAt;
    //public TouchField TouchField;
    //private Vector3 dir;

    //private const float Y_ANGLE_MIN = 0;
    //private const float Y_ANGLE_MAX = 50f;

    //private float distance = 16f;
    //private float currentX = 0;
    //private float currentY = 0;
    //private float rotSpeed = 3.5f;

    void Awake()
    {
        state = State.ANYACTION;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        Gold = 1000;
        health.InitProgress(100, 100);
        mana.InitProgress(0, 100);
        stamina.InitProgress(0, 100);

        bangleCol = GameObject.FindWithTag("Player").GetComponent<SphereCollider>();
        bangleCol.enabled = false;

        ObjectPoolingManager.MyInstance.SetObject("StunnedCirclingStars", 1, transform);
        ObjectPoolingManager.MyInstance.SetObject("SwordWhirlwindBlue", 1, transform);
        ObjectPoolingManager.MyInstance.SetObject("vfx_MagicAbility_Stripes_Orange_Mobile", 1, transform);
    }
   
    void Update()
    {
        Movement();
    }

    /// <summary>
    /// 플레이어의 기본 움직임
    /// </summary>
    public void Movement()
    {
        CheckState();

        stamina.CurrentValue += 3.0f * Time.deltaTime;

        if (isMove == true)
        {
            animator.SetFloat("Speed", (h * h + v * v));
            rigidbody.velocity = camDir * moveSpeed;

            if (h != 0f && v != 0f)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(camDir.x, 0, camDir.z));

                //currentY += v * 0.3f;
                //currentX += h * 2.0f;
                //dir = new Vector3(0, 0, -currentY);
                //Quaternion rotation = Quaternion.Euler(0, -currentX, 0);

                //transform.position = lookAt.position + rotation * dir;
                //transform.LookAt(lookAt.transform.position);
            }
        }
    }

    /// <summary>
    /// 조이스틱을 움직이면 조이스틱의 위치를 받아와서 캐릭을 이동
    /// </summary>
    /// <param name="stickPos"></param>
    public void OnstickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;

        camFoward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        camDir = v * camFoward + h * cam.right;
    }

    /// <summary>
    /// 공격버튼
    /// </summary>
    public void OnAttackDown()
    {
        if (state == State.ANYACTION /*|| state == State.ROLL*/)
        {
            state = State.ATTACK;
        }

        if (state == State.ATTACK || state == State.ROLL)
        {
            animator.SetTrigger("AttackStart");
        }
    }

    /// <summary>
    /// 구르기 
    /// </summary>
    public void OnDashDown()
    {
        if (state == State.TIRED)
        {
            animator.ResetTrigger("AttackStart");
            animator.ResetTrigger("Tired");
            animator.ResetTrigger("Dash");
            animator.ResetTrigger("ChargeSkill");
            animator.ResetTrigger("Destroy");
        }

        else if (stamina.CurrentValue < 12)
        {
            if (Time.time >= nextRolling)
            {
                state = State.TIRED;
                animator.SetTrigger("Tired");
                nextRolling = Time.time + rollingRate;
            }
        }

        else if (state == State.ANYACTION || state == State.ATTACK)
        {
            if (state != State.ROLL)
            {
                if (Time.time >= nextRolling)
                {
                    StartCoroutine(Rolling());
                    nextRolling = Time.time + rollingRate;
                }
            }
        }
    }

    /// <summary>
    /// 스킬을 사용해라
    /// </summary>
    /// <param name="skillname"></param>
    public void UseSkill(string skillname)
    {
        //SkillBook.MyInstance.castSkill(skillname);

        DestroyAttackEffect();

        if (skillname == "BangleBangle")
        {
            ChargeSkillDown(skillname);
        }

        else if (skillname == "Destroy")
        {
            DestroySkill();
        }

        else if (skillname == "Rolling")
        {
            OnDashDown();
        }
    }

    /// <summary>
    /// 파개 스킬
    /// </summary>
    public void DestroySkill()
    {
        if (state == State.ANYACTION || state == State.ATTACK || state == State.ROLL)
        {
            if (Time.time >= nextDestroy)
            {
                animator.ResetTrigger("AttackStart");
                animator.SetTrigger("Destroy");
                nextDestroy = Time.time + DestroyRate;
            }
        }
    }

    /// <summary>
    /// 뱅글뱅글 차징
    /// </summary>
    /// <param name="skillname"></param>
    public void ChargeSkillDown(string skillname)
    {
        if (state != State.TIRED)
        {
            if (Time.time >= nextCharge)
            {
                audio.PlayOneShot(chargeSound);
                if (saveChageTime < 0.8f)
                {
                    SkillBook.MyInstance.StopCasting();
                    isPressed = false;
                    isMove = true;
                    animator.ResetTrigger("ChargeSkill");
                    animator.ResetTrigger("Spin");
                    animator.ResetTrigger("AttackStart");
                    animator.ResetTrigger("Destroy");
                    animator.SetTrigger("FailCharge");
                }

                if (state == State.ANYACTION || state == State.ATTACK || state == State.ROLL)
                {
                    nextCharge = Time.time + ChargegRate;
                    isPressed = true;
                    isMove = false;
                    SkillBook.MyInstance.ChargeSkill(skillname);
                    StartCoroutine(Charge(skillname));
                    StartCoroutine(OnBangleChargeEffect());
                    animator.ResetTrigger("FailCharge");
                    animator.ResetTrigger("AttackStart");
                    animator.ResetTrigger("Destroy");
                    animator.SetTrigger("ChargeSkill");
                }
            }
            else
            {
                animator.ResetTrigger("Spin");
                animator.ResetTrigger("FailCharge");
                animator.ResetTrigger("ChargeSkill");
                animator.ResetTrigger("AttackStart");
                animator.ResetTrigger("Destroy");
            }
        }
    }

    /// <summary>
    /// 뱅글뱅글 버튼 떼었을때
    /// </summary>
    /// <param name="skillname"></param>
    public void BangleSkillUp(string skillname)
    {
        if (state != State.TIRED)
        {
            ObjectPoolingManager.MyInstance.OffObject("vfx_MagicAbility_Stripes_Orange_Mobile");

            if (state == State.CHARGESKILL)
            {
                if (saveChageTime >= 0.5f)
                {
                    StartCoroutine(OnBangleSkillEffect());
                    audio.PlayOneShot(bangleSound); 
                    isPressed = false;
                    isMove = true;
                    SkillBook.MyInstance.UseSkill(skillname);
                    animator.SetTrigger("Spin");
                    StartCoroutine(ChargeSkill(0));
                }
            }
            if (state == State.ANYACTION || state == State.CHARGESKILL || state == State.ATTACK ||
                state == State.ROLL)
            {
                if (saveChageTime < 0.5f)
                {
                    SkillBook.MyInstance.StopCasting();
                    isPressed = false;
                    isMove = true;
                    animator.ResetTrigger("ChargeSkill");
                    animator.ResetTrigger("AttackStart");
                    animator.SetTrigger("FailCharge");
                }
            }
        }
    }

    /// <summary>
    /// 플레이어의 상태 체크
    /// </summary>
    private void CheckState()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("attackA1") || stateInfo.IsName("attackA2") || stateInfo.IsName("attackA3") ||
            stateInfo.IsName("attackA4") || stateInfo.IsName("attackA5") || stateInfo.IsName("attackA5ToStand"))
        {
            state = State.ATTACK;
        }

        else if (stateInfo.IsName("down_back") || stateInfo.IsName("down_back_ToStand") || stateInfo.IsName("stand_tired@loop")
            || stateInfo.IsName("emotion_defeat") || stateInfo.IsName("ChargeSkillA_SkilllToStand"))
        {
            state = State.TIRED;
        }

        else if (stateInfo.IsName("ChargeSkillA_ChargeStart") || stateInfo.IsName("ChargeSkillA_Charge@loop"))
        {
            state = State.CHARGESKILL;
            isUseCharge = true;
        }

        else if (stateInfo.IsName("ChargeSkillA_ChargeToSkill") || stateInfo.IsName("ChargeSkillA_Skill"))
        {
            state = State.CHARGEMOVE;
        }

        else if (stateInfo.IsName("SkillA_unlock"))
        {
            state = State.DESTROYSKILL;
            isUseDestroy = true;
        }

        else if (stateInfo.IsName("roll_front"))
            state = State.ROLL;
        else
            state = State.ANYACTION;

        if (state == State.TIRED)
        {
            animator.ResetTrigger("AttackStart");
        }

        if (state == State.ATTACK || state == State.TIRED || state == State.CHARGESKILL || state == State.DESTROYSKILL)
        {
            isMove = false;
            rigidbody.velocity = camDir * 0f;
            animator.SetFloat("Speed", 0);
        }
        else
            isMove = true;

    }

    private void DestroyAttackEffect()
    {
        ObjectPoolingManager.MyInstance.OffObject("Slash_1");
        ObjectPoolingManager.MyInstance.OffObject("Slash_2");
        ObjectPoolingManager.MyInstance.OffObject("Slash_3");
        ObjectPoolingManager.MyInstance.OffObject("Slash_Ex");
    }

    IEnumerator Rolling()
    {
        stamina.CurrentValue -= 14;
        float dashTime = 3.0f;
        float timePass = Time.deltaTime;
        animator.SetTrigger("Dash");
        animator.ResetTrigger("AttackStart");

        while (dashTime >= 0)
        {
            rigidbody.velocity = new Vector3(camDir.x * 11.0f, 0, camDir.z * 11.0f);
            timePass += Time.deltaTime;
            dashTime -= timePass;
            
            yield return null;
        }
    }

    IEnumerator Charge(string skillname)
    {
        float timePassed = Time.deltaTime;

        while (isPressed == true)
        {
            timePassed += Time.deltaTime;

            if (timePassed <= 2.5f)
            {
                saveChageTime = timePassed;
            }

            yield return null;
        }
    }

    IEnumerator ChargeSkill(int index)
    {
        float timePassed = saveChageTime;

        while (isPressed == false)
        {
            timePassed -= Time.deltaTime;
 
            if (saveChageTime >= 2.0f)
            {
                isConfused = true;
            }

            if (timePassed <= 0.1f && isConfused)
            {
                animator.SetTrigger("Confuse");
                StartCoroutine(StunEffect());
                bangleCol.enabled = false;
                isConfused = false;
                break; 
            }

            else if (timePassed <= 0.1f)
            {
                animator.SetTrigger("StopSkill");
            }

            if (timePassed <= 0)
            {
                isConfused = false;
                bangleCol.enabled = false;
                animator.ResetTrigger("StopSkill");
                animator.ResetTrigger("Confuse");
            }

            yield return null;
        }
    }

    IEnumerator OnBangleChargeEffect()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("vfx_MagicAbility_Stripes_Orange_Mobile");
        go.transform.position = this.transform.position;
        go.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        ObjectPoolingManager.MyInstance.OffObject("vfx_MagicAbility_Stripes_Orange_Mobile");
    }

    IEnumerator OnBangleSkillEffect()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("SwordWhirlwindBlue");
        go.transform.position = this.transform.position + new Vector3(0, 1.0f, 0);

        ParticleSystem[] ps = go.GetComponentsInChildren<ParticleSystem>();

        for (int i=0; i < ps.Length; i++)
        {
            var main = ps[i].main;
            main.startLifetime = saveChageTime + 0.5f;
        }
        
        go.SetActive(true);
        audio.PlayOneShot(bangleSwing);
        bangleCol.enabled = true;
        yield return new WaitForSeconds(3.0f);
        ObjectPoolingManager.MyInstance.OffObject("SwordWhirlwindBlue");
    }

    IEnumerator StunEffect()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("StunnedCirclingStars");
        go.transform.position = this.transform.position + new Vector3(0, 2.3f, 0);
        go.SetActive(true);
        yield return new WaitForSeconds(4.3f);
        ObjectPoolingManager.MyInstance.OffObject("StunnedCirclingStars");
    }

    public IEnumerator DamageEffect()
    {
        GameObject go = ObjectPoolingManager.MyInstance.GetObject("ExplosionNovaFire");
        go.transform.position = this.transform.position;
        go.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        go.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BossAtkCol"))
        {
            StartCoroutine(DamageEffect());
            health.CurrentValue -= 3;

            int rand = Random.Range(1, 4);

            switch (rand)
            {
                case 1:
                    audio.PlayOneShot(damageSound[0]);
                    break;
                case 2:
                    audio.PlayOneShot(damageSound[1]);
                    break;
                case 3:
                    audio.PlayOneShot(damageSound[2]);
                    break;
            }
        }
    }

    public void BangleSound()
    {
        audio.PlayOneShot(bangleSwing);
    }
}
