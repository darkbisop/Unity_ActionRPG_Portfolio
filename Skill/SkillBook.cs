using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBook : MonoBehaviour
{
    private static SkillBook instance;

    public static SkillBook MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillBook>();
            }

            return instance;
        }
    }

    public Skill[] MySkills { get => skills; set => skills = value; }

    [SerializeField] private Skill[] skills;
    [SerializeField] private Text skillName;
    [SerializeField] private Text castTime;
    [SerializeField] private Image castingBar;
    [SerializeField] private CanvasGroup canvasGroup;

    private Coroutine skillRoutine;
    private Coroutine fadeRoutine;
    private Coroutine coolRoutine;

    private ActionButton actionButton;

    private float saveProgress;
    private float saveChageTime;
    private bool isPressed = false;

    private void Start()
    {
        actionButton = GetComponent<ActionButton>();
    }

    public Skill castSkill(string spellName)
    {
        Skill skill = Array.Find(MySkills, x => x.MySkillName == spellName);
        castingBar.fillAmount = 0;
        skillName.text = skill.MySkillName;
        castTime.text = skill.MySkillChargeTime.ToString();
        skillRoutine = StartCoroutine(Progress(skill));
        
        
        fadeRoutine = StartCoroutine(FadeBar());
        return skill;
    }

    public Skill ChargeSkill(string SkillName)
    {
        isPressed = true;
        Skill skill = Array.Find(MySkills, x => x.MySkillName == SkillName);

        castingBar.fillAmount = 0;

        if (SkillName == "BangleBangle")
        {
            skillName.text = "뺑글뺑글~!";
        }
        
        skillRoutine = StartCoroutine(Progress(skill));
        fadeRoutine = StartCoroutine(FadeBar());

        return skill;
    }

    public Skill UseSkill(string SkillName)
    {
        Skill skill = Array.Find(MySkills, x => x.MySkillName == SkillName);
        isPressed = false;

        if (SkillName == "BangleBangle")
        {
            skillName.text = "뺑글뺑글~!";
        }

        skillRoutine = StartCoroutine(decreaseProgress(skill));

        return skill;
    }

    private IEnumerator FadeBar()
    {
        float rate = 1.0f / 0.5f;

        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator Progress(Skill skill)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / skill.MySkillChargeTime;

        float progress = 0.0f;

        while (isPressed == true)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            if (timePassed <= skill.MySkillChargeTime)
            {
                castTime.text = timePassed.ToString("F1");
                saveProgress = progress;
                saveChageTime = timePassed;
            }

            yield return null;
        }
    }

    private IEnumerator decreaseProgress(Skill skill)
    {
        float progress = saveProgress;
        float rate = 1.0f / skill.MySkillChargeTime;
        float timePassed = saveChageTime;

        while (isPressed == false)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress -= rate * Time.deltaTime;

            timePassed -= Time.deltaTime;

            if (timePassed > 0)
            {
                castTime.text = timePassed.ToString("F1");
            }
            else
                StopCasting();

            yield return null;
        }
    }

    IEnumerator CoolTime(Skill skill)
    {
        float progress = 1.0f;
        float rate = 1.0f / skill.MySkillCoolTime;
        float timePassed = Time.deltaTime;

        while (skill.MySkillCoolTime > 0.1f)
        {
            //CountCoolTime.MyInstance.MyCanvasGroup.alpha = 1;

            //skill.MySkillBackGround.fillAmount = Mathf.Lerp(0, 1, progress);

            progress -= rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            if (timePassed > 0)
            {
                //CountCoolTime.MyInstance.MyText.text = (skill.MySkillCoolTime - timePassed).ToString("F0");
            }

            if (skill.MySkillCoolTime - timePassed <= 0)
            {
                StopCoroutine(coolRoutine);
                //CountCoolTime.MyInstance.MyCanvasGroup.alpha = 0;
                //skill.MySkillBackGround.fillAmount = 1.0f;
                //CountCoolTime.MyInstance.MyText.text = "0";
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void StopCasting()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }

        if (skillRoutine != null)
        {
            StopCoroutine(skillRoutine);
            skillRoutine = null;
        }
    }

    public Skill GetSpell(string skillName)
    {
        Skill skill = Array.Find(MySkills, x => x.MySkillName == skillName);

        return skill;
    }
}
