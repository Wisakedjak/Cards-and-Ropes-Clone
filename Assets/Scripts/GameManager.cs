using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Transform> slots = new List<Transform>();
    [SerializeField] private List<GameObject> cards = new List<GameObject>();
    public List<GameObject> chests = new List<GameObject>();
    public List<GameObject> giftBoxes = new List<GameObject>();
    [SerializeField] private List<GameObject> cardPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> chestPrefabs = new List<GameObject>();
    private int _money;
    [SerializeField]private int moneyForCreateCard;
    [SerializeField] private TextMeshProUGUI moneyText,progressText,levelText;
    [SerializeField] private Button createBtn, throwBtn;
    private int _currentProgress, _maxProgress,_currentLevel;
    [SerializeField] private List<int> levelProgressCounts = new List<int>();
    private bool _isProgressCompleted;
    
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
       _giveLevelValues();
        _createOldCardsAndChests();
    }

    private void _giveLevelValues()
    {
        _currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        levelText.text = "Level " + _currentLevel;
        _currentProgress = 0;
        _maxProgress = levelProgressCounts[_currentLevel-1];
        progressText.text = _currentProgress + "/" + _maxProgress;
        _money = PlayerPrefs.GetInt("CurrentGold");
        moneyText.text = _money.ToString();
        _isProgressCompleted = false;
        _checkCreateButtonIntractability();
    }

    private void _openNextLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel",PlayerPrefs.GetInt("CurrentLevel")+1);
        _giveLevelValues();
        SceneController.instance.OpenNextLevel();
    }

    public void CreateCard(int cardLevel)
    {
       _createCard(cardLevel); 
    }

    private void _createCard(int cardLevel)
    {
        if (_money>=moneyForCreateCard)
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
                int index = slots.FindIndex(a => a.Equals(t));
                PlayerPrefs.SetInt("isSlot"+(index+1)+"Full",101);
                _spendMoney();
                
                break;
            }
        }
        
        _checkCreateButtonIntractability();

        
        
    }
    private void _createStartCard(int cardLevel,Transform t)
    {
       
            GameObject createdCard = Instantiate(cardPrefabs[cardLevel-1], t);
            createdCard.transform.localPosition = new Vector3(0, .12f, 0);
            t.GetComponent<SlotHolder>().isFull = true;
            t.GetComponent<SlotHolder>().cardLevel = cardLevel;
            t.GetComponent<SlotHolder>().cardInSlot = createdCard;
            cards.Add(createdCard.gameObject);
            
        
    }

    public void DestroyCardAndCheckGameOver(GameObject card)
    {
        _destroyCardAndCheckGameOver(card);
    }

    private void _destroyCardAndCheckGameOver(GameObject card)
    {
        print("_destroyCardAndCheckGameOver before foreach");
        card.SetActive(false);
        foreach (var t in cards)
        {
            if (t.activeSelf)
            {
                return;
            }
        }
        print("_destroyCardAndCheckGameOver after foreach");
        _gameOver();
    }

    private void _gameOver()
    {
        if (_isProgressCompleted)
        {
            print("next Level");
            _openNextLevel();
        }
        else
        {
           SceneController.instance.RestartLevel();
        }
        
    }

    public void GainMoney(int money)
    {
        StartCoroutine(GainMoneyCoroutine(money));
    }

    IEnumerator GainMoneyCoroutine(int money)
    {
        yield return new WaitForSeconds(.3f);
        _money += money;
        moneyText.text = _money.ToString();
        PlayerPrefs.SetInt("CurrentGold",_money);
    }

    private void _spendMoney()
    {
        _money -= moneyForCreateCard;
        moneyText.text = _money.ToString();
        PlayerPrefs.SetInt("CurrentGold",_money);
    }

    public void MergeCard(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        _mergeCard(selectedObject,slotOver,cardLevel);
    }

    private void _mergeCard(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        if (cardLevel<7)
        {
            cards.Remove(selectedObject);
            cards.Remove(slotOver.GetComponent<SlotHolder>().cardInSlot);
            var parent = selectedObject.transform.parent;
            parent.GetComponent<SlotHolder>().isFull = false;
            parent.GetComponent<SlotHolder>().cardLevel = 0;
            parent.GetComponent<SlotHolder>().cardInSlot = null;
            int index = slots.FindIndex(a => a.Equals(parent));
            PlayerPrefs.SetInt("isSlot"+(index+1)+"Full",0);
            var t = slotOver.transform;
            Destroy(selectedObject);
            Destroy(slotOver.GetComponent<SlotHolder>().cardInSlot);
            GameObject createdCard = Instantiate(cardPrefabs[cardLevel], t);
            createdCard.transform.localPosition = new Vector3(0, .12f, 0);
            t.GetComponent<SlotHolder>().isFull = true;
            t.GetComponent<SlotHolder>().cardLevel = cardLevel+1;
            t.GetComponent<SlotHolder>().cardInSlot = createdCard;
            int indexSlotOver = slots.FindIndex(a => a.Equals(t));
            PlayerPrefs.SetInt("isSlot"+(indexSlotOver+1)+"Full",100+cardLevel+1);
            cards.Add(createdCard.gameObject);
        }
        
    }

    public void MergeChest(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        _mergeChest(selectedObject,slotOver,cardLevel);
    }

    private void _mergeChest(GameObject selectedObject,GameObject slotOver,int cardLevel)
    {
        if (cardLevel<8)
        {
            chests.Remove(selectedObject);
            chests.Remove(slotOver.GetComponent<SlotHolder>().cardInSlot);
            var parent = selectedObject.transform.parent;
            parent.GetComponent<SlotHolder>().isFull = false;
            parent.GetComponent<SlotHolder>().cardLevel = 0;
            parent.GetComponent<SlotHolder>().cardInSlot = null;
            int index = slots.FindIndex(a => a.Equals(parent));
            PlayerPrefs.SetInt("isSlot"+(index+1)+"Full",0);
            var t = slotOver.transform;
            Destroy(selectedObject);
            Destroy(slotOver.GetComponent<SlotHolder>().cardInSlot);
            GameObject createdChest = Instantiate(chestPrefabs[cardLevel], t);
            createdChest.transform.localPosition = new Vector3(0, .12f, 0);
            t.GetComponent<SlotHolder>().isFull = true;
            t.GetComponent<SlotHolder>().cardLevel = cardLevel+1;
            t.GetComponent<SlotHolder>().cardInSlot = createdChest;
            int indexSlotOver = slots.FindIndex(a => a.Equals(t));
            PlayerPrefs.SetInt("isSlot"+(indexSlotOver+1)+"Full",200+cardLevel+1);
            chests.Add(createdChest.gameObject);
            createdChest.tag = "ChestCollected";
        }
    }
    

    void _checkCreateButtonIntractability()
    {
        if (_money>=moneyForCreateCard)
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
        int indexSlotOver = slots.FindIndex(a => a.Equals(slotOver.transform));
        PlayerPrefs.SetInt("isSlot"+(indexSlotOver+1)+"Full",100+cardLevel);
        cards.Add(createdCard);
    }

    public void IncreaseProgress()
    {
        _increaseProgress();
    }

    private void _increaseProgress()
    {
        _currentProgress++;
        progressText.text = _currentProgress + "/" + _maxProgress;
        if (_currentProgress>=_maxProgress)
        {
            _isProgressCompleted = true;
        }
    }

    public void ReturnCardsAndOpenButtons()
    {
        _giveLevelValues();
        _returnCards();
        _checkCreateButtonIntractability();
        _resetGiftBox();
        throwBtn.interactable = true;
       
    }

    private void _resetGiftBox()
    {
        for (int i = 0; i < giftBoxes.Count; i++)
        {
            giftBoxes[i].GetComponent<BoxCollider>().enabled = true;
            giftBoxes[i].GetComponent<Animator>().SetBool("isCollect",false);
            
        }
    }

    private void _returnCards()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.localPosition = new Vector3(0, .12f, 0);
            cards[i].transform.localRotation = Quaternion.Euler(Vector3.zero);
            cards[i].GetComponent<CardMove>().tearTry = 5;
            cards[i].SetActive(true);
        }
        
    }

    private void _createOldCardsAndChests()
    {
        for (int i = 1; i < 10; i++)
        {
            if (PlayerPrefs.GetInt("isSlot"+i+"Full")/100==1)
            {
                _createStartCard(PlayerPrefs.GetInt("isSlot"+i+"Full")%100,slots[i-1]);
            }
        }
    }
    

    
    void Update()
    {
        
    }
}
