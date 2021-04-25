using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private Window window;

    public bool IsInteracting { get; set; }

    public void Interact()
    {
        if (!IsInteracting)
        {
            IsInteracting = true;
            window.Open(this);
        }
    }

    public void DeInteract()
    {
        if (IsInteracting) {
            IsInteracting = false;
            window.Close(this);
        }
    }
}
