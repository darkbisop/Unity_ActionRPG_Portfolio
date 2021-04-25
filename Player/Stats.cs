using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public event EventHandler OnStatChanged;
    public static float statMin = 0;
    public static float statMax = 20;

    public enum Type { ATTACK, DEFENCE, HEALTH, CRI, STAMINA}

    private singleStat attackStat;
    private singleStat defenceStat;
    private singleStat healthStat;
    private singleStat criStat;
    private singleStat staStat;

    public Stats(float attackStatAmount, float defenceStatAmount, float healthStatAmount, float criStatAmount, float staStatAmount)
    {
        attackStat = new singleStat(attackStatAmount);
        defenceStat = new singleStat(defenceStatAmount);
        healthStat = new singleStat(healthStatAmount);
        criStat = new singleStat(criStatAmount);
        staStat = new singleStat(staStatAmount);
    }

    private singleStat GetSingleStat(Type statType)
    {
        switch (statType)
        {
            default:
            case Type.ATTACK:
                return attackStat;
            case Type.DEFENCE:
                return defenceStat;
            case Type.HEALTH:
                return healthStat;
            case Type.CRI:
                return criStat;
            case Type.STAMINA:
                return staStat;
        }
    }

    public void SetStatAmount(Type statType, float StatAmount)
    {
        GetSingleStat(statType).SetStatAmount(StatAmount);

        if (OnStatChanged != null)
        {
            OnStatChanged(this, EventArgs.Empty);
        }
    }

    public void IncreaseStat(Type statType, float EquipStat)
    {
        SetStatAmount(statType, GetStatAmount(statType) + EquipStat * 0.2f);
    }

    public void DecreaseStat(Type statType, float EquipStat)
    {
        SetStatAmount(statType, GetStatAmount(statType) - EquipStat * 0.2f);
    }

    public float GetStatAmount(Type statType)
    {
        return GetSingleStat(statType).GetStatAmount();
    }

    public float GetStatAmountNormalize(Type statType)
    {
        return GetSingleStat(statType).GetStatAmountNormalize();
    }

    private class singleStat
    {
        private float Stat;

        public singleStat(float statAmount)
        {
            SetStatAmount(statAmount);
        }

        public void SetStatAmount(float StatAmount)
        {
            Stat = Mathf.Clamp(StatAmount, statMin, statMax);
        }

        public float GetStatAmount()
        {
            return Stat;
        }

        public float GetStatAmountNormalize()
        {
            return (float)Stat / statMax;
        }
    }
}
