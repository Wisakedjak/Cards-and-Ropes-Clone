using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefInitializer : MonoBehaviour
{
    
    void Awake()
    {
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", 1);
        }
        if (!PlayerPrefs.HasKey("CurrentGold"))
        {
            PlayerPrefs.SetInt("CurrentGold", 50);
        }
        if (!PlayerPrefs.HasKey("isSlot1Full"))
        {
            PlayerPrefs.SetInt("isSlot1Full", 0);
        }
        if (!PlayerPrefs.HasKey("isSlot2Full"))
        {
            PlayerPrefs.SetInt("isSlot2Full", 0);
        }
        if (!PlayerPrefs.HasKey("isSlot3Full"))
        {
            PlayerPrefs.SetInt("isSlot3Full", 0);
        }
        if (!PlayerPrefs.HasKey("isSlot4Full"))
        {
            PlayerPrefs.SetInt("isSlot4Full", 0);
        }
        if (!PlayerPrefs.HasKey("isSlot5Full"))
        {
            PlayerPrefs.SetInt("isSlot5Full", 0);
        }
        if (!PlayerPrefs.HasKey("isSlot6Full"))
        {
            PlayerPrefs.SetInt("isSlot6Full", 0);
        }
        if (!PlayerPrefs.HasKey("isSlot7Full"))
        {
            PlayerPrefs.SetInt("isSlot7Full", 0);
        }
        if (!PlayerPrefs.HasKey("isSlot8Full"))
        {
            PlayerPrefs.SetInt("isSlot8Full", 0);
        }
        if (!PlayerPrefs.HasKey("isSlot9Full"))
        {
            PlayerPrefs.SetInt("isSlot9Full", 0);
        }
        if (!PlayerPrefs.HasKey("isSlot10Full"))
        {
            PlayerPrefs.SetInt("isSlot10Full", 0);
        }
    }

    
    void Update()
    {
        
    }
}
