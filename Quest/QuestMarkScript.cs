using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkScript : MonoBehaviour
{
    private Camera mainCamera;
    private Canvas canvas;
    private RectTransform rectPrent;
    private RectTransform rectMy;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    void Start()
    {
        canvas = GameObject.Find("Canvas_Camera").GetComponentInParent<Canvas>();
        mainCamera = canvas.worldCamera;
        rectPrent = canvas.GetComponent<RectTransform>();
        rectMy = this.gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectPrent, screenPos, mainCamera, out localPos);

        rectMy.localPosition = localPos;
    }
}
