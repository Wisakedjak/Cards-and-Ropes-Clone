using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotHolder : MonoBehaviour
{
    public bool isFull;
    private void OnMouseOver()
    {
        CardDrag.Instance.isOverSlot = true;
        CardDrag.Instance.slotOver = gameObject;
    }

    private void OnMouseExit()
    {
        CardDrag.Instance.isOverSlot = false;
        CardDrag.Instance.slotOver = null;
    }
}
