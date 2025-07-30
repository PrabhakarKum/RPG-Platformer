using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_BlackSmith : Object_NPC, IInteractable
{
    private static readonly int IsBlackSmith = Animator.StringToHash("isBlackSmith");
    private Animator animator;
    private Inventory_Player inventoryPlayer;
    private Inventory_Storage inventoryStorage;
    public void Interact()
    {
        uiManager.craftUI.ResetCraftUI();
        //uiManager.storageUI.SetupStorage(inventoryStorage);
        uiManager.craftUI.SetupCraftUI(inventoryStorage);
        
        
        //uiManager.storageUI.gameObject.SetActive(true);
        uiManager.craftUI.gameObject.SetActive(true);
    }
   
    protected override void Awake()
    {
        base.Awake();
        inventoryStorage = GetComponent<Inventory_Storage>();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool(IsBlackSmith, true);
        
        uiManager.SetDefaultStorage(inventoryStorage);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventoryPlayer = player.GetComponent<Inventory_Player>();
        inventoryStorage.SetInventory(inventoryPlayer);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        uiManager.SwitchOffAllTooltips();
        uiManager.craftUI.ResetCraftUI();
        //uiManager.storageUI.gameObject.SetActive(false);
        uiManager.craftUI.gameObject.SetActive(false);
        
    }
}
