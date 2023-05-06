using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeColliderScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CardMove>().Trigger();
    }
}
