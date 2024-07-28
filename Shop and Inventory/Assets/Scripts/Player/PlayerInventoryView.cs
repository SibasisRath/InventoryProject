using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldAmount;
    [SerializeField] private TextMeshProUGUI weight;
    [SerializeField] private Transform itemCategoryDisplayArea;
    [SerializeField] private Transform itemListDisplayArea;
    [SerializeField] private Button gatherButton;
    [SerializeField] private BaseItemCellView cellView;
    [SerializeField] private PlayerInventoryItemView playerInventoryItemView;
    [SerializeField] private TransactionType transactionType;
    [SerializeField] private SoundTypes successfulClick;


    private void Start()
    {
        playerInventoryController.LoadPlayerInventoryItems();
        playerInventoryController.EnableInitialItemsToDisplay();

        gatherButton.onClick.AddListener(OnGatherButtonClicked);
        UpdateAvailableSpace();
        UpdateGoldAmmount();
    }

    private void OnGatherButtonClicked()
    {
        playerInventoryController.InvokeGatherService();
    }

    private PlayerInventoryController playerInventoryController;

    public PlayerInventoryItemView PlayerInventoryItemView => playerInventoryItemView;

    public BaseItemCellView CellView => cellView;

    public SoundTypes SuccessfulClick { get => successfulClick; }
    public Transform ItemCategoryDisplayArea { get => itemCategoryDisplayArea; }
    public Transform ItemListDisplayArea { get => itemListDisplayArea; }
    public TransactionType TransactionType { get => transactionType;}

    public void SetPlayerInventoryController(PlayerInventoryController playerInventoryController) 
    {
        this.playerInventoryController = playerInventoryController;
    }

    public void UpdateGoldAmmount() 
    {
        goldAmount.text = "Gold: " + playerInventoryController.GetPlayerInventoryModel().GoldAmount.ToString();
    }

    public void UpdateAvailableSpace()
    {
        weight.text = "Weight: " + playerInventoryController.GetPlayerInventoryModel().Weight.ToString();
    }
}
