using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGradeBg : MonoBehaviour
{
    [SerializeField] private Sprite slotBG_Green;
    [SerializeField] private Sprite slotBG_Blue;
    [SerializeField] private Sprite slotBG_Purple;
    [SerializeField] private Sprite slotBG_Red;
    [SerializeField] private Sprite slotBG_Brown;

    [SerializeField] private Sprite ItemBG_Green;
    [SerializeField] private Sprite ItemBG_Blue;
    [SerializeField] private Sprite ItemBG_Purple;
    [SerializeField] private Sprite ItemBG_Red;

    public Sprite MySlotBG_Green { get => slotBG_Green; set => slotBG_Green = value; }
    public Sprite MySlotBG_Blue { get => slotBG_Blue; set => slotBG_Blue = value; }
    public Sprite MySlotBG_Purple { get => slotBG_Purple; set => slotBG_Purple = value; }
    public Sprite MySlotBG_Red { get => slotBG_Red; set => slotBG_Red = value; }
    public Sprite MySlotBG_Brown { get => slotBG_Brown; set => slotBG_Brown = value; }
    public Sprite MyItemBG_Green { get => ItemBG_Green; set => ItemBG_Green = value; }
    public Sprite MyItemBG_Blue { get => ItemBG_Blue; set => ItemBG_Blue = value; }
    public Sprite MyItemBG_Purple { get => ItemBG_Purple; set => ItemBG_Purple = value; }
    public Sprite MyItemBG_Red { get => ItemBG_Red; set => ItemBG_Red = value; }
   
}
