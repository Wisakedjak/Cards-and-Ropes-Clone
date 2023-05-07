using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> slots = new List<Transform>();
    [SerializeField] private List<GameObject> cards = new List<GameObject>();
    [SerializeField] private List<GameObject> cardPrefabs = new List<GameObject>();
    [SerializeField]private GameObject level1CardGameObject;
    public int money;
    [SerializeField]private int moneyForCreateCard;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Button createBtn, throwBtn;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        moneyText.text = money.ToString();
       _checkCreateButtonIntractability();
    }

    public void CreateCard(int cardLevel)
    {
       _createCard(cardLevel); 
    }

    private void _createCard(int cardLevel)
    {
        if (money>=moneyForCreateCard)
        {
            foreach (var t in slots)
            {
                if (t.GetComponent<SlotHolder>().isFull) continue;
                GameObject createdCard = Instantiate(cardPrefabs[cardLevel-1], t);
                createdCard.transform.localPosition = new Vector3(0, .12f, 0);
                t.GetComponent<SlotHolder>().isFull = true;
                t.GetComponent<SlotHolder>().cardLevel = cardLevel;
                t.GetComponent<SlotHolder>().cardInSlot = createdCard;
                cards.Add(createdCard.gameObject);
                money -= moneyForCreateCard;
                moneyText.text = money.ToString();
                break;
            }
        }
        
        _checkCreateButtonIntractability();

        
        
    }

    public void MergeCard(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        _mergeCard(selectedObject,slotOver,cardLevel);
    }

    private void _mergeCard(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        cards.Remove(selectedObject);
        cards.Remove(slotOver.GetComponent<SlotHolder>().cardInSlot);
        var parent = selectedObject.transform.parent;
        parent.GetComponent<SlotHolder>().isFull = false;
        parent.GetComponent<SlotHolder>().cardLevel = 0;
        parent.GetComponent<SlotHolder>().cardInSlot = null;
        var t = slotOver.transform;
        Destroy(selectedObject);
        Destroy(slotOver.GetComponent<SlotHolder>().cardInSlot);
        GameObject createdCard = Instantiate(cardPrefabs[cardLevel], t);
        createdCard.transform.localPosition = new Vector3(0, .12f, 0);
        t.GetComponent<SlotHolder>().isFull = true;
        t.GetComponent<SlotHolder>().cardLevel = cardLevel+1;
        t.GetComponent<SlotHolder>().cardInSlot = createdCard;
       
        cards.Add(createdCard.gameObject);
    }

    void _checkCreateButtonIntractability()
    {
        if (money>=moneyForCreateCard)
        {
            createBtn.interactable = true;
        }
        else
        {
            createBtn.interactable = false;
        }
    }

    public void ThrowCard()
    {
        _throwCard();
        throwBtn.interactable = false;
        createBtn.interactable = false;
    }

    private void _throwCard()
    {
        if (cards.Count <= 0) return;
        foreach (var t in cards)
        {
            t.GetComponent<CardMove>().ThrowCard();
        }
    }

    public void CloseColliders()
    {
        _closeColliders();
    }

    public void OpenColliders()
    {
        _openColliders();
    }

    private void _closeColliders()
    {
        foreach (var t in cards)
        {
            t.GetComponent<BoxCollider>().enabled = false;
        }
    }  
    
    private void _openColliders()
    {
        foreach (var t in cards)
        {
            t.GetComponent<BoxCollider>().enabled = true;
        }
    }

    
    void Update()
    {
        
    }
}
