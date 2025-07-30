using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Object_Merchant : Object_NPC, IInteractable
{
    private Inventory_Player inventoryPlayer;
    private Inventory_Shop inventoryShop;
    public void Interact()
    {
        uiManager.merchantUI.SetupMerchantUI(inventoryShop, inventoryPlayer);
        uiManager.merchantUI.gameObject.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Z))
            inventoryShop.FillShopList();
    }

    protected override void Awake()
    {
        base.Awake();
        inventoryShop = GetComponent<Inventory_Shop>();
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventoryPlayer = player.GetComponent<Inventory_Player>();
        inventoryShop.SetInventory(inventoryPlayer);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        uiManager.SwitchOffAllTooltips();
        uiManager.merchantUI.gameObject.SetActive(false);
    }
}
