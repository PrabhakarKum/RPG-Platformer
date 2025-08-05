using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private ItemListDataSO dropData;

    [Header("Drop Restrictions")] 
    [SerializeField] private int maximumRarityAmount = 1200;
    [SerializeField] private int maxItemsToDrop = 3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            DropItems();
        }
    }

    public virtual void DropItems()
    {
        if (dropData == null)
        {
            Debug.Log("you need to assign drop data on entity: "+ gameObject.name);
            return;
        }
        
        List<ItemDataSO> itemsToDrop = RollDrops();
        int amountToDrop = Mathf.Min(itemsToDrop.Count, maxItemsToDrop);

        for (int i = 0; i < amountToDrop; i++)
        {
            CreateItemDrop(itemsToDrop[i]);   
        }

    }

    protected void CreateItemDrop(ItemDataSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab, transform.position, quaternion.identity);
        newItem.GetComponent<Object_ItemPickup>().SetupItems(itemToDrop);
    }
        
        
    public List<ItemDataSO> RollDrops()
    {
        List<ItemDataSO> possibleDrops = new List<ItemDataSO>();
        List<ItemDataSO> finalDrops = new List<ItemDataSO>();
        float maximumRarityAmount = this.maximumRarityAmount;
        
        foreach (var item in dropData.itemList)
        {
            float dropChance = item.GetDropChance();

            if (Random.Range(0, 100) <= dropChance)
                possibleDrops.Add(item);
        }
        
        // step 2: sort by rarity (highest to lowest)
        possibleDrops = possibleDrops.OrderByDescending(item => item.itemRarity).ToList();
        
        // step 3: Add items to final drop list until rarity limit on entity is reached

        foreach (var item in possibleDrops)
        {
            if (maximumRarityAmount > item.itemRarity)
            {
                finalDrops.Add(item);
                maximumRarityAmount -= item.itemRarity;
            }
        }

        return finalDrops;
    }

}
