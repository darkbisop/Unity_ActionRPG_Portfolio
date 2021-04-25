using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Skill : IUseable, IMoveableIcon
{
    [SerializeField] private string name;
    [SerializeField] private int damage;
    [SerializeField] private float castTime;
    [SerializeField] private float coolTime;
    [SerializeField] private Sprite icon;
    [SerializeField] private Image image;

    public string MySkillName { get => name; }
    //public int MySkillDamage { get => damage; }
    public float MySkillChargeTime { get => castTime; }
    public float MySkillCoolTime { get => coolTime; }
    public Sprite MyIcon { get => icon; set => icon = value; }

    public void Use()
    {
        PlayerMovement.MyInstance.UseSkill(MySkillName);
    }
}