using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 해당 스킬 버튼의 스킬 이름 입력을 위한 변수
    /// </summary>
    [SerializeField]
    private string skillName;

    /// <summary>
    /// 스킬창에서 해당 스킬을 눌렀을때 처리
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 왼쪽 클릭 했다면
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //오브젝트에서 해당 스킬의 이름을 찾아서 스킬의 아이콘을 가져온다
            HandScript.MyInstance.TakeMoveable(SkillBook.MyInstance.GetSpell(skillName));
        }
    }
}
