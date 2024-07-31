using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIService : MonoBehaviour
{
    [Header("Entire Details and Transaction Section")]
    [SerializeField] private GameObject entireSection;

    [Header("Player details panel")]
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private TextMeshProUGUI category;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI buyOrSellPriceHeading;
    [SerializeField] private TextMeshProUGUI buyOrSellPrice; // during player it will show sell price, during shop it will show buy price.
    [SerializeField] private TextMeshProUGUI weight;
    [SerializeField] private TextMeshProUGUI rarity;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button backButtonFromItemDetails;

    [SerializeField] private SoundTypes buttonClickedSound;
    [SerializeField] private SoundTypes clickDeclinedSound;
    [SerializeField] private SoundTypes transactionSuccessfulSound;

    [Header("Transaction Section")]
    [SerializeField] private GameObject transactionSection;
    [SerializeField] private TextMeshProUGUI currentAmountText;
    [Space]
    [SerializeField] private string totalPossibleBuyHeadingText;
    [SerializeField] private string totalGoldDebitHeadingText;
    [Space]
    [SerializeField] private string totalPossibleSellHeadingText;
    [SerializeField] private string totalGoldCreditHeadingText;
    [Space]
    [SerializeField] private TextMeshProUGUI totalPossibleAmountText; // according to weight
    [SerializeField] private TextMeshProUGUI totalGoldRequiredText;
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button proceedButton;
    [SerializeField] private Button backFromTransactionSectionButton;

    [Header("Transaction Confirmation Section")]
    [SerializeField] private GameObject transactionConfirmationSection;
    [SerializeField] private TextMeshProUGUI transactionDetails;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button declineButton;

    [Header("Message Pop Up Section")]
    [SerializeField] private GameObject messageSection;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button backFromPopUpButton;

    private PlayerInventoryController playerInventoryController;
    private EventService eventService;

    private int totalPossibleQuantity;
    private int totalGoldForTransaction;
    private int currentQuantity;
    private const int leastQuantity = 0;
    private const string notEnoughMoneyMessage = "You don't have enough gold for this tracsaction.";
    private ItemCellController itemCell;
    private TransactionType currentTransactionType;
    public int CurrentAmount 
    {
        get => currentQuantity;
        set
        {
            currentQuantity = value;
            UpdateTotalGoldRequirement();
        }
    }

    private void OnDisable()
    {
        eventService.OnSpaceUnavailable.RemoveListener(UnableToGatherMessage);
    }

    private void UnableToGatherMessage(string message)
    {
        messageSection.SetActive(true);
        messageText.text = message;
    }

    public TransactionType CurrentTransactionType 
    {
        get => currentTransactionType;
        set
        {
            currentTransactionType = value;
            Debug.Log(currentAmountText);
        }  
    }

    private void UpdateTotalGoldRequirement()
    {
        if (CurrentTransactionType == TransactionType.Sell)
        {
            totalGoldForTransaction = currentQuantity * itemCell.GetItemModel().SellingPrice;
        }
        else 
        {
            totalGoldForTransaction = currentQuantity * itemCell.GetItemModel().BuyingPrice;
        }
    }

    private void UpdateTotalPossibleQuantity()
    {
        if (CurrentTransactionType == TransactionType.Sell)
        {
           totalPossibleQuantity = itemCell.GetItemModel().Quantity;
        }
        else
        {
           totalPossibleQuantity = playerInventoryController.GetPossibleQuantityAccordingToWeight(itemCell.GetItemModel().Weight);
        }
    }

    private void ResetTransaction()
    {
        CurrentAmount = leastQuantity;

        UpdateTotalPossibleQuantity();
        UpdateTotalGoldRequirement();
        UpdateTransactionSection();
    }

   

    private void Start()
    {
        entireSection.SetActive(false);
        transactionConfirmationSection.SetActive(false);
        transactionSection.SetActive(false);
        messageSection.SetActive(false);

        backButtonFromItemDetails.onClick.AddListener(OnBackButtonFromItemDetailsClicked);
        increaseButton.onClick.AddListener(OnIncreaseButtonClicked);
        decreaseButton.onClick.AddListener(OnDecreaseButtonClicked);
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        declineButton.onClick.AddListener(OnDeclineButtonClicked);

        proceedButton.onClick.AddListener(OnProceedButtonClicked);
        backFromTransactionSectionButton.onClick.AddListener(OnBackFromTransactionButtonClicked);

        backFromPopUpButton.onClick.AddListener(OnBackFromPopUpButtonClicked);
    }

    public void Init(PlayerInventoryController playerInventoryController, EventService eventService)
    {
        this.playerInventoryController = playerInventoryController;
        this.eventService = eventService;

        eventService.OnSpaceUnavailable.AddListener(UnableToGatherMessage);
    }

    private void OnBackButtonFromItemDetailsClicked()
    {
        eventService.OnButtonSelected.InvokeEvent(buttonClickedSound);
        entireSection.SetActive(false);
    }

    private void OnConfirmButtonClicked() 
    {
        if (CurrentAmount != 0)
        {
            eventService.OnButtonSelected.InvokeEvent(transactionSuccessfulSound);
            playerInventoryController.UpdatePlayerInventory(itemCell.GetItemModel(), currentQuantity);
        }
        else
        {
            eventService.OnButtonSelected.InvokeEvent(clickDeclinedSound);
        }
        transactionConfirmationSection.SetActive(false);
        ResetTransaction();
    }

    private void OnDeclineButtonClicked() 
    {
        eventService.OnButtonSelected.InvokeEvent(buttonClickedSound);
        transactionConfirmationSection.SetActive(false);
        //ResetTransactionDetailsSection();
        ResetTransaction();
    }

    private void OnDecreaseButtonClicked()
    {
        
        if (CurrentAmount > leastQuantity)
        {
            CurrentAmount--;
            eventService.OnButtonSelected.InvokeEvent(buttonClickedSound);
        }
        else
        {
            eventService.OnButtonSelected.InvokeEvent(clickDeclinedSound);
        }
        UpdateTransactionSection();
    }

    private void OnIncreaseButtonClicked()
    {
        
        if (CurrentAmount < totalPossibleQuantity)
        {
            eventService.OnButtonSelected.InvokeEvent(buttonClickedSound);
            CurrentAmount++;
        }
        else
        {
            eventService.OnButtonSelected.InvokeEvent(clickDeclinedSound);
        }
        UpdateTransactionSection();
    }

    private void OnBuyButtonClicked()
    {
        eventService.OnButtonSelected.InvokeEvent(buttonClickedSound);
        transactionSection.SetActive(true);        
    }

    private void OnSellButtonClicked() 
    {
        eventService.OnButtonSelected.InvokeEvent(buttonClickedSound);
        transactionSection.SetActive(true);
    }

    private void OnBackFromPopUpButtonClicked() 
    {
        messageSection.SetActive(false);
    }

    private void OnProceedButtonClicked()
    {
        //itemCell.GetItemModel().Quantity = CurrentAmount;

        if (currentTransactionType == TransactionType.Buy)
        {
            bool buyingPossiblity = playerInventoryController.CheckBuyingPossibility(totalGoldForTransaction);
            if (buyingPossiblity)
            {
                eventService.OnButtonSelected.InvokeEvent(buttonClickedSound);
                transactionConfirmationSection.SetActive(true);
                
            }
            else
            {
                eventService.OnButtonSelected.InvokeEvent(clickDeclinedSound);
                messageSection.SetActive(true);
                messageText.text = notEnoughMoneyMessage;
            }
        }
        else
        {
            eventService.OnButtonSelected.InvokeEvent(buttonClickedSound);
            transactionConfirmationSection.SetActive(true);
        }

        UpdateTransactionConfirmationSection();
    }

    private void OnBackFromTransactionButtonClicked()
    {
        eventService.OnButtonSelected.InvokeEvent(buttonClickedSound);
        transactionSection.SetActive(false);
    }

    public void EnablePlayerDetails(ItemCellController item, TransactionType transactionType)
    {
        itemCell = item;
        entireSection.SetActive(true);
        currentTransactionType = itemCell.GetItemModel().TransactionType;
        ResetTransaction();
        UpdateDetails(item.GetItemModel());
        if (transactionType == TransactionType.Buy) 
        {
            BuyingInfo(item.GetItemModel());
        }
        else
        {
            SellingInfo(item.GetItemModel());
        }
    }

    private void SellingInfo(ItemCellModel item)
    {
        buyOrSellPriceHeading.text = "Selling";
        buyOrSellPrice.text = item.SellingPrice.ToString();
        buyButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(true);
        sellButton.onClick.AddListener(OnSellButtonClicked);
    }

    private void BuyingInfo(ItemCellModel item)
    {
        buyOrSellPriceHeading.text = "Buying";
        buyOrSellPrice.text = item.BuyingPrice.ToString();
        sellButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(true);
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    private void UpdateDetails(ItemCellModel item)
    {
        itemIcon.GetComponent<Image>().sprite = item.Icon;
        category.text = item.Type.ToString();
        itemName.text = item.Item.ToString();
        description.text = item.ItemDescription;
        weight.text = item.Weight.ToString();
        rarity.text = item.Rarity.ToString();
    }

    private void UpdateTransactionSection() 
    {
        if (CurrentTransactionType == TransactionType.Sell)
        {
            totalPossibleAmountText.text = totalPossibleBuyHeadingText + " " + totalPossibleQuantity.ToString();
            totalGoldRequiredText.text = totalGoldDebitHeadingText + " " + totalGoldForTransaction.ToString();
        }
        else
        {
            totalPossibleAmountText.text = totalPossibleSellHeadingText + " " + totalPossibleQuantity.ToString();
            totalGoldRequiredText.text = totalGoldCreditHeadingText + " " + totalGoldForTransaction.ToString();
        }

        currentAmountText.text = CurrentAmount.ToString();
    }
    private void UpdateTransactionConfirmationSection()
    {
        if (currentTransactionType == TransactionType.Sell)
        {
            int sellingCost = itemCell.GetItemModel().SellingPrice * CurrentAmount;
            transactionDetails.text = "Do you want to sell \"" + itemCell.GetItemModel().Item + "\" of size " + CurrentAmount + "\nat cost of " + sellingCost;
        }
        else
        {
            int buyingCost = itemCell.GetItemModel().BuyingPrice * CurrentAmount;
            transactionDetails.text = "Do you want to buy \"" + itemCell.GetItemModel().Item + "\" of size " + CurrentAmount + "\nat cost of " + buyingCost;
        }
        
    }
}
