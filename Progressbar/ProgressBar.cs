using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    /// <summary>
    /// 프로그레스바의 숫자 표시용
    /// </summary>
    [SerializeField] private Text textValue;

    /// <summary>
    /// 프로그래스바의 이미지
    /// </summary>
    private Image content;

    /// <summary>
    /// 현재 프로그레스바의 양
    /// </summary>
    private float currentFill;

    /// <summary>
    /// 현재의 수치
    /// </summary>
    private float currentValue;

    /// <summary>
    /// 최대 수치
    /// </summary>
    public float MaxValue { get; set; }

    /// <summary>
    /// currentValue의 프로퍼티. 모든 계산은 이걸 통한다
    /// </summary>
    public float CurrentValue
    {
        get => currentValue;
        set
        {
            // 현재값이 최대값보다 크다면 현재값을 최대값으로
            if (value > MaxValue) currentValue = MaxValue;

            // 현재수치가 0보다 작다면 현재수치를 0으로
            else if (value < 0) currentValue = 0;

            // 그것도 아니면 현재 값을 적용시킨다
            else currentValue = value;

            // 비율 계산
            currentFill = currentValue / MaxValue;

            // 텍스트 포맷.
            textValue.text = string.Format("{0:N0} / {1}", currentValue, MaxValue);
        }
    }

    void Start()
    {
        content = GetComponent<Image>();
    }

    void Update()
    {
        // 이만큼 차있지 않다면
        if (currentFill != content.fillAmount)
        {
            // 프로그레스바를 이만큼 채운다. 
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * 10.0f);
        }
    }

    // 초기수치용이지만 딱히 의미는 없다. 
    public void InitProgress(float currVal, float maxVal)
    {
        MaxValue = maxVal;
        CurrentValue = currVal;
    }
}
