using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private UI_ItemSlot[] uiItemSlots;
    private static Inventory_Player inventory;
    private UI_EquipSlot[] equipSlots;

    [SerializeField] private Transform uiItemSlotParent;
    [SerializeField] private Transform uiEquipSlotParent;
    private void Awake()
    {
        inventory = Inventory_Base.Instance as Inventory_Player;
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();
    }
    
    private void Start()
    {
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }
    
    private void UpdateUI()
    {
       UpdateInventorySlots();
       UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> playerEquipList = inventory.equipList;

        for (int i = 0; i < equipSlots.Length; i++)
        {
            var playerEquipSlot = playerEquipList[i];
            
            if (playerEquipSlot.HasItem() == false)
                equipSlots[i].UpdateSlot(null);
            else
                equipSlots[i].UpdateSlot(playerEquipSlot.equippedItem);
        }
    }
    private void UpdateInventorySlots()
    {
        var itemList = inventory.itemList;

        for (var i = 0; i < uiItemSlots.Length; i++) // 10 ui slots
        {
            uiItemSlots[i].UpdateSlot(i < itemList.Count ? itemList[i] : null);
        }
    }
}
