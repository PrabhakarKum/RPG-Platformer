using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Object_ItemPickup : MonoBehaviour
{
    [SerializeField] private Vector2 dropForce = new Vector2(5, 10);
    private ItemDataSO itemData;
    
    [Space]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider2D;

    private void OnValidate()
    {
        if(itemData == null)
            return;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetupVisuals();
    }

    public void SetupItems(ItemDataSO itemData)
    {
        this.itemData = itemData;
        SetupVisuals();

        float xDropForce = Random.Range(-dropForce.x, dropForce.x);
        rb.velocity = new Vector2(xDropForce, dropForce.y);
        collider2D.isTrigger = false;
    }

    private void SetupVisuals()
    {
        spriteRenderer.sprite = itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + itemData.itemName;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && collider2D.isTrigger == false)
        {
            Debug.Log("touched the ground: "+ collision.gameObject.layer);
            collider2D.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory_Player inventoryPlayer = collision.GetComponent<Inventory_Player>();

        if (inventoryPlayer == null)
            return;
        
        Inventory_Item itemToAdd = new Inventory_Item(itemData);
        Inventory_Storage storage = inventoryPlayer.storage;

        if (itemData.itemType == ItemType.Material)
        {
            storage.AddMaterialToStash(itemToAdd);
            Destroy(gameObject);
            return;
        }

        if (inventoryPlayer.CanAddItem(itemToAdd)) 
        {
             inventoryPlayer.AddItem(itemToAdd);
             Destroy(gameObject); 
        }
         

    }
}
