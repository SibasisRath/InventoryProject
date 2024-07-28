using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCellController 
{
    private readonly ItemCellModel itemCellModel;
    private readonly BaseItemCellView baseCellView;
    private readonly PlayerInventoryItemView playerInventoryItemView;

    private EventService eventService;

    public ItemCellController(ItemSO itemSO, BaseItemCellView baseItemCellView) 
    {
        this.itemCellModel = new(itemSO);
        this.baseCellView = baseItemCellView;
        this.baseCellView.SetCellController(this);
    }

    public ItemCellController(ItemSO itemSO, PlayerInventoryItemView playerInventoryItemView)
    {
        this.itemCellModel = new(itemSO);
        this.playerInventoryItemView = playerInventoryItemView;
        this.playerInventoryItemView.SetCellController(this);
    }


    public void Init(EventService eventService)
    {
        this.eventService = eventService;
    }

    public BaseItemCellView GetView() => baseCellView;

    public PlayerInventoryItemView GetPlayerInventoryItemView() => playerInventoryItemView;

    public ItemCellModel GetItemModel() => itemCellModel;

    public void UpdateQuantity(ItemCellModel itemCell, int changedQuantity)
    {
        if (itemCell.TransactionType == TransactionType.Sell)
        {
            this.itemCellModel.Quantity -= changedQuantity;
        }
        else 
        {
            this.itemCellModel.Quantity += changedQuantity;
        }
        
        if (itemCellModel.Quantity == 0)
        {
            if (eventService != null)
            {
                eventService.OnEntirelySellingAnItem.InvokeEvent(this);
            }
            else
            {
                Debug.LogError("EventService is not initialized.");
            }
        }
        // Update the appropriate view
        if (playerInventoryItemView != null)
        {
            playerInventoryItemView.UpdatePlayerInventoryItemView(itemCellModel.Quantity);
        }
    }
}
