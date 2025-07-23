using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    protected Inventory_Item itemInSlot;
    protected Inventory_Player inventory;
    
    [Header("UiSlot Setup")] 
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    protected virtual void Start()
    {
        inventory = Inventory_Base.Instance as Inventory_Player;
        
        if (inventory == null)
            Debug.LogError("Could not find Inventory_Player instance");
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null)
            return;
        
        inventory.TryEquipItem(itemInSlot);
    }
    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if (itemInSlot == null)
        {
            itemStackSize.text = " ";
            itemIcon.color = Color.clear;
            return;
        }

        var color = Color.white; color.a = 0.9f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";

    }


    
}
