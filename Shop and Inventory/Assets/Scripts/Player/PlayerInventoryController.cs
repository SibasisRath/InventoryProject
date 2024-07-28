using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryController
{
    private readonly PlayerInventoryView playerInventoryView;
    private readonly PlayerInventoryModel playerInventoryModel;

    private List<ItemCategoryController> categoryControllers;

    private ShopService shopService;
    private EventService eventService;
    private UIService uiService;

    private ItemCategoryController selectedCategoryCell = null;

    public PlayerInventoryController(PlayerInventoryView playerInventoryView, PlayerInventorySO playerInventorySO)
    {
        this.playerInventoryView = playerInventoryView;
        this.playerInventoryView.SetPlayerInventoryController(this);
        playerInventoryModel = new(playerInventorySO);
    }

    public PlayerInventoryView GetPlayerInventoryView()
    {
        return playerInventoryView;
    }

    public PlayerInventoryModel GetPlayerInventoryModel()
    {
        return playerInventoryModel;
    }

    public void Init(ShopService shopService, EventService eventService, UIService uiService)
    {
        this.shopService = shopService;
        this.eventService = eventService;
        this.uiService = uiService;

        categoryControllers = new ();
        //itemTypes = new ();

        foreach (var categoryController in categoryControllers)
        {
            categoryController.Init(eventService, uiService);
        }
    }

    public void InvokeGatherService()
    {
        eventService.OnGatherButtonClicked.InvokeEvent();
    }

    public void LoadPlayerInventoryItems()
    {
        foreach (var itemType in shopService.ItemTypes)
        {
            // Create the item category model
            ItemCategoryModel itemCategoryModel = new (itemType);

            // Create the item category model
            GameObject categoryCellObject = Object.Instantiate(playerInventoryView.CellView.gameObject);
            categoryCellObject.transform.SetParent(playerInventoryView.ItemCategoryDisplayArea, false);
            categoryCellObject.GetComponent<Image>().sprite = itemType.TypeImage;
            BaseItemCellView categoryCellView = categoryCellObject.GetComponent<BaseItemCellView>();

            // Create the item category controller
            ItemCategoryController itemCategoryController = new(categoryCellView, itemCategoryModel)
            {
                TransactionType = playerInventoryView.TransactionType
            };

            if (this.eventService != null && this.uiService != null)
            {
                itemCategoryController.Init(this.eventService, this.uiService);
            }
            categoryCellObject.GetComponent<Button>().onClick.AddListener(() => OnItemCategoryButtonClicked(itemCategoryController));

            categoryControllers.Add(itemCategoryController);
        }
    }

    public void EnableInitialItemsToDisplay()
    {
        if (categoryControllers.Count > 0)
        {
            var firstCategoryController = categoryControllers[0];
            firstCategoryController.SetShopItemsActive(true);
            selectedCategoryCell = firstCategoryController;
        }
    }

    private void OnItemCategoryButtonClicked(ItemCategoryController clickedCategoryController)
    {
        eventService.OnButtonSelected.InvokeEvent(playerInventoryView.SuccessfulClick);

        if (selectedCategoryCell != clickedCategoryController)
        {
            selectedCategoryCell.SetPlayerItemsActive(false);
        }

        clickedCategoryController.SetPlayerItemsActive(true);
        selectedCategoryCell = clickedCategoryController;
    }

    public void UpdatePlayerInventory(ItemCellModel itemCell, int changedQuantity)
    {
        foreach (var controller in categoryControllers) 
        {
            if (itemCell.Type == controller.GetItemCategoryModel().ItemCategory) 
            {
                bool UpdateSuccessful = controller.UpdateItems(itemCell, changedQuantity);
                if (UpdateSuccessful == false)
                {
                    controller.GenerateItems(playerInventoryView.ItemListDisplayArea, itemCell, changedQuantity, playerInventoryView.PlayerInventoryItemView);
                }
                
                OnItemCategoryButtonClicked(controller);
                
            }
        }
        UpdatePlayerGold(itemCell, changedQuantity);
        UpdatePlayerInventoryWeight(itemCell, changedQuantity);
    }

    public bool CheckBuyingPossibility (int totalRequiredGold)
    {
        if (totalRequiredGold < playerInventoryModel.GoldAmount) {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdatePlayerInventoryWeight(ItemCellModel itemCell, int changedQuantity)
    {
        if (itemCell.TransactionType == TransactionType.Sell)
        {
            playerInventoryModel.Weight += (itemCell.Weight * changedQuantity);
        }
        else if (itemCell.TransactionType == TransactionType.Buy || itemCell.TransactionType == TransactionType.Gather)
        {
            playerInventoryModel.Weight -= (itemCell.Weight * changedQuantity);
        }
        
        playerInventoryView.UpdateAvailableSpace();
    }

    private void UpdatePlayerGold(ItemCellModel itemCell, int changedQuantity)
    {
        if (itemCell.TransactionType == TransactionType.Sell)
        {
            playerInventoryModel.GoldAmount += (itemCell.SellingPrice * changedQuantity);
        }
        else if(itemCell.TransactionType == TransactionType.Buy)
        {
            playerInventoryModel.GoldAmount -= (itemCell.BuyingPrice * changedQuantity);
        }
        
        playerInventoryView.UpdateGoldAmmount();
    }

    public int GetPossibleQuantityAccordingToWeight(int itemWeight)
    {
        int possibleAmount = playerInventoryModel.Weight / itemWeight;
        return possibleAmount;
    }
}
