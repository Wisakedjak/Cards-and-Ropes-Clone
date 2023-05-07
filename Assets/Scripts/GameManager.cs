using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Transform> slots = new List<Transform>();
    [SerializeField] private List<GameObject> cards = new List<GameObject>();
    public List<GameObject> chests = new List<GameObject>();
    [SerializeField] private List<GameObject> cardPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> chestPrefabs = new List<GameObject>();
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

    public void DestroyCardAndCheckGameOver(GameObject card)
    {
        _destroyCardAndCheckGameOver(card);
    }

    private void _destroyCardAndCheckGameOver(GameObject card)
    {
        cards.Remove(card);
        Destroy(card);
        if (cards.Count<=0)
        {
            _gameOver();
        }
    }

    private void _gameOver()
    {
        print("gameOver");
    }

    public void GainMoney(int money)
    {
        StartCoroutine(GainMoneyCoroutine(money));
    }

    IEnumerator GainMoneyCoroutine(int money)
    {
        yield return new WaitForSeconds(.3f);
        this.money += money;
        moneyText.text = this.money.ToString();

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

    public void MergeChest(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        _mergeChest(selectedObject,slotOver,cardLevel);
    }

    private void _mergeChest(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        chests.Remove(selectedObject);
        chests.Remove(slotOver.GetComponent<SlotHolder>().cardInSlot);
        var parent = selectedObject.transform.parent;
        parent.GetComponent<SlotHolder>().isFull = false;
        parent.GetComponent<SlotHolder>().cardLevel = 0;
        parent.GetComponent<SlotHolder>().cardInSlot = null;
        var t = slotOver.transform;
        Destroy(selectedObject);
        Destroy(slotOver.GetComponent<SlotHolder>().cardInSlot);
        GameObject createdChest = Instantiate(chestPrefabs[cardLevel], t);
        createdChest.transform.localPosition = new Vector3(0, .12f, 0);
        t.GetComponent<SlotHolder>().isFull = true;
        t.GetComponent<SlotHolder>().cardLevel = cardLevel+1;
        t.GetComponent<SlotHolder>().cardInSlot = createdChest;
        chests.Add(createdChest.gameObject);
        createdChest.tag = "ChestCollected";
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
        if (cards.Count>0)
        {
            foreach (var t in cards)
            {
                t.GetComponent<BoxCollider>().enabled = false;
            }
        }

        if (chests.Count>0)
        {
            foreach (var t in chests)
            {
                t.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }  
    
    private void _openColliders()
    {
        if (cards.Count>0)
        {
            foreach (var t in cards)
            {
                t.GetComponent<BoxCollider>().enabled = true;
            }
        }

        if (chests.Count>0)
        {
            foreach (var t in chests)
            {
                t.GetComponent<BoxCollider>().enabled = true;
            }
        }
        
    }

    public void CreateCardFromChest(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        _createCardFromChest(selectedObject,slotOver,cardLevel);
    }

    private void _createCardFromChest(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        chests.Remove(selectedObject);
        Destroy(selectedObject);
        GameObject createdCard = Instantiate(cardPrefabs[cardLevel-1], slotOver.transform);
        createdCard.transform.localPosition = new Vector3(0, .12f, 0);
        slotOver.GetComponent<SlotHolder>().cardLevel = cardLevel;
        slotOver.GetComponent<SlotHolder>().cardInSlot = createdCard;
        cards.Add(createdCard);
    }

    
    void Update()
    {
        
    }
}
