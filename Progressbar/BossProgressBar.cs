using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossProgressBar : MonoBehaviour
{
    public const float DAMAGED_SHRINK_TIMER_MAX = 0.2f;
    private Image healthBarImage, damagedBarImage, nextHpBarImage;
    public float damagedShrinkTimer;
    public Health health;
    public Text hpText;
    private int maxHp = 400;
    private int currentHp = 400;
    private int hpSingleBar = 50;
    public List<Color> color;
    private bool hpHit = false;

    void Awake()
    {
        healthBarImage = GameObject.Find("BossHpBar").transform.Find("HealthBar").GetComponent<Image>();
        damagedBarImage = GameObject.Find("BossHpBar").transform.Find("DamageBar").GetComponent<Image>();
        nextHpBarImage = GameObject.Find("BossHpBar").transform.Find("NextHpBar").GetComponent<Image>();
    }

    void Update()
    {
        damagedShrinkTimer -= Time.deltaTime;

        //if (damagedShrinkTimer < 0)
        //{
        //    if (healthBarImage.fillAmount < damagedBarImage.fillAmount)
        //    {
        //        float shrinkedSpeed = 1f;
        //        damagedBarImage.fillAmount -= shrinkedSpeed * Time.deltaTime;
        //    }
        //}

        //healthBarImage.fillAmount = health.GetHealthNormalized();


        if (damagedShrinkTimer < 0)
        {
            if (healthBarImage.rectTransform.sizeDelta.x != damagedBarImage.rectTransform.sizeDelta.x)
            {
                float shrinkedSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkedSpeed * Time.deltaTime;
                Invoke("ReFreshHPBar",0.3f);
                //damagedBarImage.rectTransform.sizeDelta = new Vector2(healthBarImage.rectTransform.sizeDelta.x * GetHpRationSingleBar(currentHp), healthBarImage.rectTransform.sizeDelta.y);
            }

            //if (healthBarImage.rectTransform.sizeDelta.x == damagedBarImage.rectTransform.sizeDelta.x)
            //{
            //    damagedBarImage.rectTransform.sizeDelta = new Vector2(nextHpBarImage.rectTransform.sizeDelta.x * GetHpRationSingleBar(currentHp), nextHpBarImage.rectTransform.sizeDelta.y);
            //    damagedBarImage.fillAmount = 1;
            //}
        }

        Refresh();

    }

    void ReFreshHPBar()
    {
        damagedBarImage.rectTransform.sizeDelta = new Vector2(nextHpBarImage.rectTransform.sizeDelta.x * GetHpRationSingleBar(currentHp), nextHpBarImage.rectTransform.sizeDelta.y);
        damagedBarImage.fillAmount = 1;
    }

    void Refresh()
    {
        healthBarImage.rectTransform.sizeDelta = new Vector2(nextHpBarImage.rectTransform.sizeDelta.x * GetHpRationSingleBar(currentHp), nextHpBarImage.rectTransform.sizeDelta.y);
        float divided = currentHp / hpSingleBar;
        int index = (int)divided;
        hpText.text = string.Format("x" + index.ToString());
        healthBarImage.color = GetColorByLayer(currentHp);
        nextHpBarImage.color = GetColorByLayer(currentHp - hpSingleBar);
    }

    public float GetHpRationSingleBar(int targetHp)
    {
        float resultRatio = 0;
        float divided = (float)targetHp / hpSingleBar;

        if (targetHp > 0)
        {
            if (divided == (int)divided)
            {
                resultRatio = 1;
            }
            else
            {
                float moduled = targetHp % hpSingleBar;
                resultRatio = moduled / hpSingleBar;
            }
        }
        else
        {
            resultRatio = 0;
        }

        return resultRatio;
    }

    public Color GetColorByLayer(int targetHp)
    {
        Color result = Color.black;

        if (targetHp > 0)
        {
            float divided = (float)targetHp / hpSingleBar;
            int index = (int)divided;

            if (divided == (int)divided)
            {
                index--;
            }

            result = color[index % color.Count];
        }

        return result;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Invoke("damageHP", 0.3f);
    }

    private void damageHP()
    {
        hpHit = true;
    }
}

public class Health
{
    private int healthAmount;
    private int healthAmountMax;
    public int hpSingleBar = 20;

    public int HealthAmount { get => healthAmount; set => healthAmount = value; }

    public Health(int healthAmount)
    {
        healthAmountMax = healthAmount;
        this.healthAmount = healthAmount;
    }

    public void Damaged(int amount)
    {
        healthAmount -= amount;
        if (healthAmount < 0) healthAmount = 0;
        //if (OnDamged != null) OnDamged(this, EventArgs.Empty);
    }

    public void Heal(int amount)
    {
        healthAmount += amount;
        if (healthAmount > healthAmountMax)
        {
            healthAmount = healthAmountMax;
        }
        //if (OnHealed != null) OnHealed(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }
}
