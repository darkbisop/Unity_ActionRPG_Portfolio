using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMark : MonoBehaviour
{
    public GameObject markPrefabs;

    public Vector3 offset = new Vector3(0, 2.2f, 0);

    private Canvas uiCanvas;
    private SpriteRenderer image;

    void Start()
    {
        SetImage();

    }

    void SetImage()
    {
        uiCanvas = GameObject.Find("Canvas_Camera").GetComponent<Canvas>();
        GameObject bar = Instantiate<GameObject>(markPrefabs, uiCanvas.transform);
        var sprite = bar.GetComponent<SpriteRenderer>();
        var image = bar.GetComponent<QuestMarkScript>();
        image.targetTr = this.gameObject.transform;
        image.offset = offset;
    }
}
