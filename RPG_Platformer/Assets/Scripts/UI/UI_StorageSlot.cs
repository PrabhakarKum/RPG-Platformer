using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Storage storage;
    public enum storageSlotType { storageSlot, playerinventorySlot }
    public storageSlotType SlotType;
    
    public void SetStorage(Inventory_Storage storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null)
            return;

        bool transferFullStack = Input.GetKey(KeyCode.LeftControl);
        
        if(SlotType == storageSlotType.storageSlot)
            storage.FromStorageToPlayer(itemInSlot, transferFullStack);
        
        if(SlotType == storageSlotType.playerinventorySlot)
            storage.FromPlayerToStorage(itemInSlot, transferFullStack);
        
        uiManager.itemTooltip.ShowToolTip(false, null);
        
    }
}
