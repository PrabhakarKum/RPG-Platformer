using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected Inventory_Item itemInSlot;
    protected Inventory_Player inventory;
    protected UI_Manager uiManager;
    protected RectTransform rectTransform;
    
    [Header("UiSlot Setup")] 
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    protected virtual void Awake()
    {
        uiManager = GetComponentInParent<UI_Manager>();
        rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void Start()
    {
        inventory = Inventory_Player.Instance;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null || itemInSlot.itemData.itemType == ItemType.Material)
            return;

        bool alternativeInput = Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Mouse0);
        
        if(alternativeInput)
        {
            inventory.RemoveOneItem(itemInSlot);
        }
        else
        {
            if (itemInSlot.itemData.itemType == ItemType.Consumable)
            {
                if(itemInSlot.itemEffect.CanBeUsed() == false)
                    return;
            
                inventory.TryUseItem(itemInSlot);
            }
            else
                inventory.TryEquipItem(itemInSlot);
        }
        
        if(itemInSlot == null)
            uiManager.itemTooltip.ShowToolTip(false, null);
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
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(itemInSlot == null)
            return;
        
        uiManager.itemTooltip.ShowToolTip(true, rectTransform, itemInSlot);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiManager.itemTooltip.ShowToolTip(false, null);
    }
}
