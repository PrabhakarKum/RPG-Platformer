using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemSlotParent : MonoBehaviour
{
   private UI_ItemSlot[] slots;

   public void updateSlots(List<Inventory_Item> itemList)
   {
      if (slots == null)
         slots = GetComponentsInChildren<UI_ItemSlot>();
      
      for (var i = 0; i < slots.Length; i++) // 30 ui slots
      {
         slots[i].UpdateSlot(i < itemList.Count ? itemList[i] : null);
      }
   }
}
