﻿using UnityEngine;


/// <summary>
/// To be placed on the root of the inventory UI. Handles spawning all the
/// inventory slot prefabs
/// </summary>

public class InventoryUI : MonoBehaviour
{
    // CONFIG DATA
    [SerializeField] InventorySlotUI InventoryItemPrefab = null;

    // CACHE
    InventoryController playerInventory;

    // LIFECYCLE METHODS

    private void Awake() 
    {
        playerInventory = InventoryController.GetPlayerInventory();
        playerInventory.inventoryUpdated += Redraw;
    }

    private void Start()
    {
        Redraw();
    }

    // PRIVATE

    private void Redraw()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }


        //Only Gets size of inventory, Needs to get Size of equipment as well
        for (int i = 0; i < playerInventory.GetSize3D(); i++)
        {
            var itemUI = Instantiate(InventoryItemPrefab, transform);
            itemUI.Setup(playerInventory, i);
        }
    }
}