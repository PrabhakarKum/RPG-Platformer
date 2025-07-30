using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Craft : MonoBehaviour
{
   [SerializeField] private UI_ItemSlotParent inventoryParent;
   private Inventory_Player playerInventory;
   private UI_CraftPreview craftPreviewUI;
   private UI_CraftSlot[] craftSlots;
   private UI_CraftListButton[] craftListButtons;
   

   public void ResetCraftUI()
   {
      if (craftPreviewUI != null)
      {
         craftPreviewUI.ClearPreview(); // Add this method to UI_CraftPreview
      }
        
      if (craftSlots != null)
      {
         foreach (var slot in craftSlots)
         {
            slot.gameObject.SetActive(false);
         }
      }
   }
   public void SetupCraftUI(Inventory_Storage storage)
   {
      playerInventory = storage.playerInventory;
      playerInventory.OnInventoryChange += UpdateUI;
      UpdateUI();
      
      craftPreviewUI = GetComponentInChildren<UI_CraftPreview>();
      craftPreviewUI.SetupCraftPreview(storage);
      SetupCraftListButtons();
   }
   private void SetupCraftListButtons()
   {
      craftSlots = GetComponentsInChildren<UI_CraftSlot>(true);

      foreach (var slot in craftSlots)
         slot.gameObject.SetActive(false);

      craftListButtons = GetComponentsInChildren<UI_CraftListButton>();
      
      foreach (var button in craftListButtons)
        button.SetCraftSlots(craftSlots);
   }

   private void UpdateUI() => inventoryParent.updateSlots(playerInventory.itemList);
}
