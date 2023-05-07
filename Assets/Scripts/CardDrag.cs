using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardDrag : MonoBehaviour
{
    private GameObject _selectedObject;
    public GameObject slotOver;
    public static CardDrag Instance;
    public bool isOverSlot;
    [SerializeField] private LayerMask ignoreRaycastLayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_selectedObject==null)
            {
                RaycastHit hit = _castRay();

                if (hit.collider!=null)
                {
                    if (hit.collider.CompareTag("Card")||hit.collider.CompareTag("ChestCollected"))
                    {
                        _selectedObject = hit.collider.gameObject;
                        GameManager.instance.CloseColliders();
                    }
                    else
                    {
                        return;
                    }

                    
                }
            }
        }

        if (_selectedObject!=null)
        {
            _selectedObject.GetComponent<BoxCollider>().enabled = false;
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            _selectedObject.transform.position = new Vector3(worldPosition.x, 1f, worldPosition.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_selectedObject!=null)
            {
                if (isOverSlot)
                {
                    if (!slotOver.GetComponent<SlotHolder>().isFull)
                    {
                        var parent = _selectedObject.transform.parent;
                        parent.GetComponent<SlotHolder>().isFull = false;
                        slotOver.GetComponent<SlotHolder>().cardLevel = parent.GetComponent<SlotHolder>().cardLevel;
                        parent.GetComponent<SlotHolder>().cardLevel = 0;
                        parent.GetComponent<SlotHolder>().cardInSlot = null;
                        parent = slotOver.transform;
                        parent.GetComponent<SlotHolder>().cardInSlot = _selectedObject;
                        _selectedObject.transform.parent = parent;
                        _selectedObject.transform.localPosition = new Vector3(0, .12f, 0);
                        slotOver.GetComponent<SlotHolder>().isFull = true;
                        _selectedObject = null;
                    }
                    else
                    {
                        if (_selectedObject!=slotOver.GetComponent<SlotHolder>().cardInSlot)
                        {
                            if (slotOver.GetComponent<SlotHolder>().cardLevel==_selectedObject.transform.parent.GetComponent<SlotHolder>().cardLevel)
                            {
                                if (_selectedObject.CompareTag("Card"))
                                {
                                    GameManager.instance.MergeCard(_selectedObject,slotOver,slotOver.GetComponent<SlotHolder>().cardLevel);
                                }
                                else
                                {
                                    GameManager.instance.MergeChest(_selectedObject,slotOver,slotOver.GetComponent<SlotHolder>().cardLevel);
                                }
                                print("cardMerge");
                            
                            
                            }
                        }
                        else
                        {
                            if (_selectedObject.CompareTag("ChestCollected"))
                            {
                                GameManager.instance.CreateCardFromChest(_selectedObject,slotOver,slotOver.GetComponent<SlotHolder>().cardLevel);
                            }
                        }
                        
                    }
                }
                else
                {
                    _selectedObject.transform.localPosition = new Vector3(0, .12f, 0);
                    _selectedObject.GetComponent<BoxCollider>().enabled = true;
                    _selectedObject = null;
                }
                
                GameManager.instance.OpenColliders();
            }
        }
    }

    RaycastHit _castRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear =
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit,Mathf.Infinity,~ignoreRaycastLayer);
        return hit;
    }
}
