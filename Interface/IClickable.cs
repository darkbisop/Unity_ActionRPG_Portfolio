using UnityEngine.UI;

public interface IClickable
{
    Image MyIcon
    {
        get;
        set;
    }

    Image MySlotIcon
    {
        get;
        set;
    }

    int MyCount
    {
        get;
    }

    Text MyStackText
    {
        get;
    }
}
