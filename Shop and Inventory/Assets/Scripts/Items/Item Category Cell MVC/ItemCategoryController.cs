using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemCategoryController 
{
    private readonly ItemCategoryModel categoryModel;
    private readonly BaseItemCellView cellView;
    private readonly List<ItemCellController> itemControllers;
    private TransactionType transactionType;

    private EventService eventService;
    private UIService uiService;

    public TransactionType TransactionType { get => transactionType; set => transactionType = value; }

    public ItemCategoryController (BaseItemCellView baseItemCellView, ItemCategoryModel itemCategoryModel) 
    {
        cellView = baseItemCellView;
        categoryModel = itemCategoryModel;
        itemControllers = new ();
    }

    public void Init(EventService eventService, UIService uIService)
    {
        this.eventService = eventService;
        this.uiService = uIService;
        SubscribeToEvents();

        foreach (var categoryController in itemControllers)
        {
            categoryController.Init(eventService);
        }
    }

    private void SubscribeToEvents()
    {
        if (eventService != null)
        {
            eventService.OnEntirelySellingAnItem.AddListener(RemoveItem);
        }
        else
        {
            Debug.LogError("EventService is not initialized.");
        }
    }

    private void UnSubscribeToEvent()
    {
        eventService.OnEntirelySellingAnItem.RemoveListener(RemoveItem);
    }

    public void GenerateItems(Transform itemCatagoryArea)
    {
        foreach (var item in categoryModel.Items)
        {
            GameObject itemCellObject = GameObject.Instantiate(cellView.gameObject);
            itemCellObject.transform.SetParent(itemCatagoryArea, false);
            itemCellObject.GetComponent<Image>().sprite = item.Icon;
            itemCellObject.SetActive(false);

            BaseItemCellView itemCellView = itemCellObject.GetComponent<BaseItemCellView>();
            ItemCellController itemCellController = new (item, itemCellView);
            itemCellController.GetItemModel().TransactionType = transactionType;
            itemCellObject.GetComponent<Button>().onClick.AddListener(() => OnItemSelected(itemCellController));
            itemControllers.Add(itemCellController);
        }
    }

    private void RemoveItem(ItemCellController itemCellController)
    {
        List<ItemCellController> itemsToRemove = new ();

        foreach (ItemCellController cellController in itemControllers)
        {
            if (cellController == itemCellController)
            {
                itemsToRemove.Add(cellController);
            }
        }

        foreach (ItemCellController cellController in itemsToRemove)
        {
            itemControllers.Remove(cellController);
            UnityEngine.Object.Destroy(cellController.GetPlayerInventoryItemView().gameObject);
        }
    }

    public void GenerateItems(Transform itemListArea, ItemCellModel itemCell, int changedQuantity, PlayerInventoryItemView playerInventoryItemView)
    {
        foreach (ItemSO itemSO in categoryModel.Items)
        {
            if (itemSO.Item == itemCell.Item)
            {
                GameObject itemCellObject = GameObject.Instantiate(playerInventoryItemView.gameObject);
                itemCellObject.transform.SetParent(itemListArea, false);
                itemCellObject.GetComponent<Image>().sprite = itemSO.Icon;

                PlayerInventoryItemView playerItemView = itemCellObject.GetComponent<PlayerInventoryItemView>();
                ItemCellController itemCellController = new(itemSO, playerItemView);
                if (this.eventService != null)
                {
                    itemCellController.Init(this.eventService);
                }
                itemCellController.GetItemModel().TransactionType = TransactionType;
                itemCellObject.GetComponent<Button>().onClick.AddListener(() => OnItemSelected(itemCellController));
                itemControllers.Add(itemCellController);
                itemCellController.UpdateQuantity(itemCell, changedQuantity);
                //UpdateItems(itemCellController.GetItemModel());
            }
        }
    }

    private void OnItemSelected(ItemCellController item)
    {
        this.eventService.OnButtonSelected.InvokeEvent(SoundTypes.ClickSuccessfull);
        this.uiService.EnablePlayerDetails(item, item.GetItemModel().TransactionType);
    }

    public void SetShopItemsActive(bool isActive)
    {
        foreach (var itemController in itemControllers)
        {
            itemController.GetView().gameObject.SetActive(isActive);
        }
    }

    public void SetPlayerItemsActive(bool isActive)
    {
        foreach (var itemController in itemControllers)
        {
            itemController.GetPlayerInventoryItemView().gameObject.SetActive(isActive);
        }
    }

    public ItemCategoryModel GetItemCategoryModel() => categoryModel;

    public bool UpdateItems(ItemCellModel itemCell, int changedQuantity) 
    {
 
        foreach (ItemCellController itemCellController in itemControllers)
        {
            if (itemCellController.GetItemModel().Item == itemCell.Item)
            {
                itemCellController.UpdateQuantity(itemCell, changedQuantity);
                return true;
            }
        }
        return false;
    }

    ~ItemCategoryController() 
    {
        UnSubscribeToEvent();
    }
}
