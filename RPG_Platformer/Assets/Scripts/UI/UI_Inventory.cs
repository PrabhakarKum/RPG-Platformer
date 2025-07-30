using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private static Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventorySlotsParent;
    [SerializeField] private UI_EquipSlotParent uiEquipSlotParent;
    
    private void Start()
    {
        inventory = Inventory_Player.Instance;
        if (inventory != null)
            inventory.OnInventoryChange += UpdateUI;
        
        UpdateUI();
    }
    
    private void UpdateUI()
    {
       inventorySlotsParent.updateSlots(inventory.itemList);
       uiEquipSlotParent.UpdateEquipmentSlots(inventory.equipList);
    }

    
}
