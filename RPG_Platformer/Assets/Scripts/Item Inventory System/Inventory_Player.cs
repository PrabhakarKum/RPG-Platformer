using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Entity_Stats playerStats;
    public List<Inventory_EquipmentSlot> equipList;

    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<Entity_Stats>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        Debug.Log($"Trying to equip item of type: {item.itemData.itemType}");
        var inventoryItem = FindItem(item.itemData);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);
        
        // trying to find empty slot and equip item
        foreach (var slot in matchingSlots)
        {
            if(slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }
        
        // No empty slot found, replacing the first matching slot
        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equippedItem;
        
        EquipItem(inventoryItem,slotToReplace);
        UnEquipItem(itemToUnequip);
    }
    
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        slot.equippedItem = itemToEquip;
        slot.equippedItem.AddModifiers(playerStats);
        
        RemoveItem(itemToEquip);
    }
    
    public void UnEquipItem(Inventory_Item itemToUnEquip)
    {
        if (CanAddItem() == false)
        {
            Debug.Log($"Can't unequip item {itemToUnEquip.itemData.itemType}");
            return;
        }

        foreach (var slot in equipList)
        {
            if (slot.equippedItem == itemToUnEquip)
            {
                slot.equippedItem = null;
                break;
            }
        }
        
        itemToUnEquip.RemoveModifiers(playerStats);
        AddItem(itemToUnEquip);
    }
}
