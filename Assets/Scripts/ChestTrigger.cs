using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    [SerializeField] private int chestLevel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            
            for (int i = 0; i < GameManager.instance.slots.Count; i++)
            {
                if (!GameManager.instance.slots[i].GetComponent<SlotHolder>().isFull)
                {
                    gameObject.transform.tag="ChestCollected";
                    gameObject.transform.parent = GameManager.instance.slots[i];
                    gameObject.transform.DOLocalJump(new Vector3(0,.12f,0), 3, 1, .3f);
                    GameManager.instance.slots[i].GetComponent<SlotHolder>().isFull = true;
                    GameManager.instance.slots[i].GetComponent<SlotHolder>().cardInSlot = gameObject;
                    GameManager.instance.slots[i].GetComponent<SlotHolder>().cardLevel = chestLevel;
                    GameManager.instance.chests.Add(gameObject);
                    break;
                }
            }
           
        }
    }
}
