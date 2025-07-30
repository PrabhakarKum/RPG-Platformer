using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Shop inventoryShop;
    private Inventory_Player inventoryPlayer;

    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_EquipSlotParent uiEquipSlots;
    
    public void SetupMerchantUI(Inventory_Shop inventoryShop, Inventory_Player inventoryPlayer)
    {
        this.inventoryShop = inventoryShop;
        this.inventoryPlayer = inventoryPlayer;

        this.inventoryPlayer.OnInventoryChange += UpdateSlotUI;
        this.inventoryShop.OnInventoryChange += UpdateSlotUI;
        
        UpdateSlotUI();

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();

        foreach (var slot in merchantSlots)
            slot.SetupMerchantUI(inventoryShop);
    }

    private void UpdateSlotUI()
    {
        if(inventoryPlayer == null)
            return;
        
        inventorySlots.updateSlots(inventoryPlayer.itemList);
        merchantSlots.updateSlots(inventoryShop.itemList);
        uiEquipSlots.UpdateEquipmentSlots(inventoryPlayer.equipList);
    }
}
