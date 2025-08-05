
using System.Collections.Generic;
using UnityEngine;


public class PlayerDropManager : Entity_DropManager
{
   [Header("Player Drop Details")] 
   [Range(0,100)]
   [SerializeField] private float chanceLoseItem = 90f;
   private Inventory_Player inventoryPlayer;

   private void Awake()
   {
      inventoryPlayer = GetComponent<Inventory_Player>();
   }

   public override void DropItems()
   {
      
      List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventoryPlayer.itemList);
      List<Inventory_EquipmentSlot> equipCopy = new List<Inventory_EquipmentSlot>(inventoryPlayer.equipList);

      foreach (var item in inventoryCopy)
      {
         if (Random.Range(0, 100) < chanceLoseItem)
         {
            CreateItemDrop(item.itemData);
            inventoryPlayer.RemoveFullStack(item);
         }
      }

      foreach (var equip in equipCopy)
      {
         if (Random.Range(0, 100) < chanceLoseItem && equip.HasItem())
         {
            var item = equip.GetEquippedItem();
            CreateItemDrop(item.itemData);
            inventoryPlayer.UnequipItem(item);
            inventoryPlayer.RemoveFullStack(item);
         }
      }

   }
}
