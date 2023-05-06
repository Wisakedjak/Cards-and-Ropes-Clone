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
                    if (!hit.collider.CompareTag("Card"))
                    {
                        return;
                    }

                    _selectedObject = hit.collider.gameObject;
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
                        _selectedObject.transform.parent.GetComponent<SlotHolder>().isFull = false;
                        _selectedObject.transform.parent = slotOver.transform;
                        _selectedObject.transform.localPosition = new Vector3(0, .2f, 0);
                        _selectedObject.GetComponent<BoxCollider>().enabled = true;
                        slotOver.GetComponent<SlotHolder>().isFull = true;
                        _selectedObject = null;
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    _selectedObject.transform.localPosition = new Vector3(0, .2f, 0);
                    _selectedObject.GetComponent<BoxCollider>().enabled = true;
                    _selectedObject = null;
                }
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
