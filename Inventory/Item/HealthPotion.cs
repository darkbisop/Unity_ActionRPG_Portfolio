using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Item/Potion", order = 1)]
public class HealthPotion : Item, IUseable
{
    [SerializeField]
    private int Health;

    public void Use()
    {
        if (PlayerMovement.MyInstance.MyHealth.CurrentValue < PlayerMovement.MyInstance.MyHealth.MaxValue)
        {
            Remove();
            PlayerMovement.MyInstance.MyHealth.CurrentValue += Health;
        }
    }

    //public override string GetDescription()
    //{
    //    return base.GetDescription() + string.Format("\n<color=#00ff00ff> 긴급할때 이마저도 없으면 아쉬운 체력포션. \n 사용시 {0} 만큼 회복시켜 준다.</color>", Health);
    //}
}
