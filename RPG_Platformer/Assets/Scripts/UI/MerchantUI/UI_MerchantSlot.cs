using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Shop inventoryShop;
    
    public enum MerchantSlotType
    {
        MerchantSlot, 
        PlayerSlot
    }
    
    public MerchantSlotType slotType;

    public void SetupMerchantUI(Inventory_Shop inventoryShop) => this.inventoryShop = inventoryShop;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(itemInSlot == null)
            return;
        
        if(slotType == MerchantSlotType.MerchantSlot)
            uiManager.itemTooltip.ShowToolTip(true, rectTransform, itemInSlot,true, true);
        else if(slotType == MerchantSlotType.PlayerSlot)
            uiManager.itemTooltip.ShowToolTip(true, rectTransform, itemInSlot,false, true);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null)
            return;
        
        var rightButton = eventData.button == PointerEventData.InputButton.Right;
        var leftButton = eventData.button == PointerEventData.InputButton.Left;

        if (slotType == MerchantSlotType.PlayerSlot)
        {
            if (rightButton)
            {
                var sellFullStack = Input.GetKey(KeyCode.LeftControl);
                inventoryShop.TrySellItem(itemInSlot, sellFullStack);
            }
            else if (leftButton)
            {
                base.OnPointerDown(eventData);
            }
        }
        else if(slotType == MerchantSlotType.MerchantSlot)
        {
            if(leftButton)
                return; // left click does nothing
            
            var buyFullStack = Input.GetKey(KeyCode.LeftControl);
            inventoryShop.TryBuyItem(itemInSlot, buyFullStack);
        }
        
        uiManager.itemTooltip.ShowToolTip(false, null);
        
        
    }

    
    
}
