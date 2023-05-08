using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GiftBoxTrigger : MonoBehaviour
{
    private GameObject _hit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Card"))
        {
            _hit = other.gameObject;
            other.gameObject.SetActive(false);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<Animator>().SetBool("isCollect",true);
           
            GameManager.instance.IncreaseProgress();
            
            
            
        }
    }

    public void ShrinkBox()
    {
        _hit.GetComponent<CardMove>()._destroyObject();
        //gameObject.transform.DOScale(Vector3.zero, .5f).OnComplete(() => gameObject.SetActive(false));
    }
}
