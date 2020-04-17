﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private float m_dropDistance = 2.0f;
    [SerializeField] private int m_maxInventory = 4;
    
    [SerializeField] private List<GameObject> m_inventoryList = new List<GameObject>();
    [SerializeField] private List<Image> m_inventoryImages;
    [SerializeField] private int m_itemSelection = 0;
    
    private Sprite m_defaultSprite;

    public List<GameObject> GetInventory()
    {
        return m_inventoryList; 
    }
    
    private void Start()
    {
        for (int i = 0; i < m_maxInventory; ++i)
        {
            m_inventoryList.Add(null);
        }

        var ps = gameObject.AddComponent<PickUpSystem>();
        ps.Initialize(this);
        m_defaultSprite = m_inventoryImages[0].sprite;
    }

    public bool AddNewItem(GameObject m_item)
    {
        for (int i = 0; i < m_maxInventory; ++i)
        {
            if (!m_inventoryList[i])
            {
                m_inventoryList[i] = m_item;
                if (m_item.GetComponent<Collectible>())
                    m_inventoryImages[i].sprite = m_item.GetComponent<Collectible>().icon;
                return true;
            }
        }

        Debug.Log("no place for item");
        return false;
    }

    private void Update()
    {
        SelectItem();
        
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton3))
            ThrowSelectedItem(m_itemSelection);
    }

    private void SelectItem()
    {
        int newSelection = m_itemSelection + (int)(Input.GetAxis("Mouse ScrollWheel") * 10.0f) + 4;
        if (Input.GetAxis("HorizontalPad") < 0)
            newSelection = 0;
        else if (Input.GetAxis("HorizontalPad") > 0)
            newSelection = 1;
        else if (Input.GetAxis("VerticalPad") < 0)
            newSelection = 2;
        else if (Input.GetAxis("VerticalPad") > 0)
            newSelection = 3;
        m_itemSelection = newSelection % m_maxInventory;
    }

    private void ThrowSelectedItem(int p_id)
    {
        if (!m_inventoryList[p_id])
            return;
        
        Vector3 dropingDistance = transform.forward * m_dropDistance;
        GameObject go = m_inventoryList[p_id];
        go.transform.position = (transform.position + dropingDistance);
        go.SetActive(true);
        
        m_inventoryList[p_id] = null;
        
        m_inventoryImages[p_id].sprite = m_defaultSprite;
    }

    public void DropUIItem(int p_id)
    {
        ThrowSelectedItem(p_id);
    }

    public void RemoveAt(int p_id)
    {
        var item = m_inventoryList[p_id];
        Destroy(item);
        m_inventoryList[p_id] = null;
        m_inventoryImages[p_id].sprite = m_defaultSprite;
    }
}
