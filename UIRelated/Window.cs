using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject button;

    private NPC npc;

    public virtual void Open(NPC npc)
    {
        if (canvasGroup.alpha == 0)
        {
            this.npc = npc;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            button.SetActive(false);
        }
    }

    public virtual void Close(NPC npc)
    {
        if (canvasGroup.alpha == 1) {
            npc.IsInteracting = false;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            npc = null;
            button.SetActive(true);
        }
    }
}
