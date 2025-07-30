using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    public Inventory_Storage storage { get; private set; }
    private Inventory_Player playerInventory;
    
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    [SerializeField] private UI_ItemSlotParent storageParent;
    [SerializeField] private UI_ItemSlotParent materialStashParent;
    
    private void Awake()
    {
        // Validate required components
        if (inventoryParent == null || storageParent == null || materialStashParent == null)
        {
            Debug.LogError("UI_Storage: Missing required UI_ItemSlotParent references!");
        }
    }
    
    public void SetupStorage(Inventory_Storage storage)
    {
        if (storage == null)
        {
            Debug.LogError("UI_Storage: Attempted to setup with null storage!");
            return;
        }
        
        this.storage = storage;
        playerInventory = storage.playerInventory;
        
        if (playerInventory == null)
        {
            Debug.LogError("UI_Storage: Player inventory not set in storage!");
            return;
        }
        
        storage.OnInventoryChange += UpdateUI;
        UpdateUI();

        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>();

        foreach (var slot in storageSlots)
            slot.SetStorage(storage);
        
    }

    private void UpdateUI()
    {
        inventoryParent.updateSlots(playerInventory.itemList);
        storageParent.updateSlots(storage.itemList);
        materialStashParent.updateSlots(storage.materialStash);
    }
}
