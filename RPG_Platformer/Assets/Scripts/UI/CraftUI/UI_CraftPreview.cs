using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
   private Inventory_Item itemToCraft;
   private Inventory_Storage storage;
   private UI_CraftPreviewSlot[] craftPreviewSlots;
   
   [Header("Item Preview Setup")]
   [SerializeField] private Image itemIcon;
   [SerializeField] private Sprite blackIconImage;
   [SerializeField] private TextMeshProUGUI itemName;
   [SerializeField] private TextMeshProUGUI itemInfo;
   public TextMeshProUGUI buttonText;

   public void ConfirmCraft()
   {
      if (itemToCraft == null)
      {
         buttonText.text = "Pick an item";
         return;
      }

      if (storage.HasEnoughMaterial(itemToCraft) && storage.playerInventory.CanAddItem(itemToCraft))
      {
        
         storage.ConsumeMaterial(itemToCraft);
         storage.playerInventory.AddItem(itemToCraft);
      }
      
      UpdateCraftPreviewSlot();
   }
   
   public void ClearPreview()
   {
      itemToCraft = null;
      itemIcon.sprite = blackIconImage;
      itemName.text = string.Empty;
      itemInfo.text = string.Empty;
      buttonText.text = "Pick an item";
        
      foreach (var slot in craftPreviewSlots)
      {
         slot.gameObject.SetActive(false);
      }
   }
   public void SetupCraftPreview(Inventory_Storage storage)
   {
      this.storage = storage;
      craftPreviewSlots = GetComponentsInChildren<UI_CraftPreviewSlot>(true);
      foreach (var slot in craftPreviewSlots)
         slot.gameObject.SetActive(false);
   }

   public void UpdateCraftPreview(ItemDataSO itemData)
   {
      itemToCraft = new Inventory_Item(itemData);

      itemIcon.sprite = itemData.itemIcon;
      itemName.text = itemData.itemName;
      buttonText.text = "Craft";
      itemInfo.text = itemToCraft.GetItemInfo();
      UpdateCraftPreviewSlot();
   }

   private void UpdateCraftPreviewSlot()
   {
      foreach (var slot in craftPreviewSlots)
         slot.gameObject.SetActive(false);
      
      for (int i = 0; i < itemToCraft.itemData.craftRecipe.Length; i++)
      {
         Inventory_Item requiredItem = itemToCraft.itemData.craftRecipe[i];
         int availableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
         int requiredAmount = requiredItem.stackSize;
         craftPreviewSlots[i].gameObject.SetActive(true);
         craftPreviewSlots[i].SetupPreviewSlot(requiredItem.itemData,availableAmount, requiredAmount);
      }
   }
}
