using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GiftBoxTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            gameObject.GetComponent<Animator>().SetBool("isCollect",true);
            GameManager.instance.IncreaseProgress();
        }
    }

    public void ShrinkBox()
    {
        gameObject.transform.DOScale(Vector3.zero, .5f).OnComplete(() => gameObject.SetActive(false));
    }
}
