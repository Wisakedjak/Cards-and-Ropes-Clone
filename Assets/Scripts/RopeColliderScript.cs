using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeColliderScript : MonoBehaviour
{
    public int ropeLevel;
    [SerializeField] private GameObject rope;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CardMove>().Trigger(ropeLevel,rope,gameObject.GetComponent<BoxCollider>());
    }
}
