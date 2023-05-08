using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZoneTrigger : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Card"))
      {
         GameManager.instance.DestroyCardAndCheckGameOver(other.gameObject);
      }
   }
}
