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
    void Start()
    {
        moneyText.text = money.ToString();
       _checkCreateButtonIntractability();
    }

    public void CreateCard()
    {
       _createCard(); 
    }

    private void _createCard()
    {
        if (money>=moneyForCreateCard)
        {
            foreach (var t in slots)
            {
                if (t.GetComponent<SlotHolder>().isFull) continue;
                GameObject createdCard = Instantiate(level1CardGameObject, t);
                createdCard.transform.localPosition = new Vector3(0, .12f, 0);
                t.GetComponent<SlotHolder>().isFull = true;
                cards.Add(createdCard.gameObject);
                money -= moneyForCreateCard;
                moneyText.text = money.ToString();
                break;
            }
        }
        
        _checkCreateButtonIntractability();

        
        
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

    
    void Update()
    {
        
    }
}
