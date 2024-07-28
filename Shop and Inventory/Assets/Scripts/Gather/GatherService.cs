using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherService
{
    private PlayerInventoryController inventoryController;
    private EventService eventService;
    private List<ItemCategorySO> itemCategorySOs;
    private Dictionary<RarityLevel, List<ItemSO>> rarityItemDictionary;

    private readonly int minNumberOfSelection = 2;
    private readonly int maxNumberOfSelection = 7;

    private readonly int minGoldAmount = 100;
    private readonly int maxGoldAmount = 700;

    private System.Random random;


    private const string spaceUnavailableMessage = "There is not enough weight.";

    public GatherService()
    {
       
    }

    public void Init(PlayerInventoryController playerInventoryController, EventService eventService, ShopService shopService)
    {
        this.inventoryController = playerInventoryController;
        this.eventService = eventService;
        itemCategorySOs = shopService.ItemTypes;

        this.random = new System.Random();
        rarityItemDictionary = new Dictionary<RarityLevel, List<ItemSO>>();
        InitializeRarityItemDictionary();

        SubscribeToEvent();
    }

    private void SubscribeToEvent()
    {
        eventService.OnGatherButtonClicked.AddListener(CheckPossibilitiesOfGatheringItem);
    }
    private void UnsubscribeToEvent()
    {
        eventService.OnGatherButtonClicked.RemoveListener(CheckPossibilitiesOfGatheringItem);
    }

    private void InitializeRarityItemDictionary()
    {
        foreach (RarityLevel rarity in Enum.GetValues(typeof(RarityLevel)))
        {
            rarityItemDictionary[rarity] = new List<ItemSO>();
        }

        foreach (ItemCategorySO category in itemCategorySOs)
        {
            foreach (ItemSO item in category.Items)
            {
                rarityItemDictionary[item.Rarity].Add(item);
            }
        }
    }

    public void GatherItems()
    {
        int numberOfItems = GetRandomInt(minNumberOfSelection, maxNumberOfSelection);
        List<float> itemProbabilities = GetRandomFloats(numberOfItems, 0, 100);

        List<ItemCellModel> gatheredItems = new ();
        foreach (var probability in itemProbabilities)
        {
            RarityLevel rarity = GetRarity(probability);
            ItemSO randomItemSO = GetRandomItemByRarity(rarity);
            ItemCellModel newItem = new(randomItemSO)
            {
                TransactionType = TransactionType.Gather
            };
            gatheredItems.Add(newItem);
        }

        foreach (var item in gatheredItems)
        {
            inventoryController.UpdatePlayerInventory(item, 1);
        }
    }

    private void CheckPossibilitiesOfGatheringItem()
    {
        if (inventoryController.GetPlayerInventoryModel().Weight < (inventoryController.GetPlayerInventoryModel().MaxWeight * 1/10))
        {
            eventService.OnButtonSelected.InvokeEvent(SoundTypes.ClickFailure);
            eventService.OnSpaceUnavailable.InvokeEvent(spaceUnavailableMessage);
        }
        else 
        {
            eventService.OnButtonSelected.InvokeEvent(SoundTypes.ClickSuccessfull);
            GatherGold();
            GatherItems();
        }
    }

    private void GatherGold()
    {
        inventoryController.GetPlayerInventoryModel().GoldAmount += UnityEngine.Random.Range(minGoldAmount, maxGoldAmount + 1);
    }

    private int GetRandomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1); // inclusive range
    }

    private List<float> GetRandomFloats(int count, float min, float max)
    {
        List<float> randomFloats = new ();
        for (int i = 0; i < count; i++)
        {
            randomFloats.Add(UnityEngine.Random.Range(min, max));
        }
        return randomFloats;
    }

    private RarityLevel GetRarity(float probability)
    {
        if (probability < 50) // 50% chance
        {
            return RarityLevel.VeryCommon;
        }
        else if (probability < 80) // 30% chance
        {
            return RarityLevel.Common;
        }
        else if (probability < 95) // 15% chance
        {
            return RarityLevel.Rare;
        }
        else if (probability < 99) // 4% chance
        {
            return RarityLevel.Epic;
        }
        else // 1% chance
        {
            return RarityLevel.Legendary;
        }
    }

    private ItemSO GetRandomItemByRarity(RarityLevel rarity)
    {
        List<ItemSO> items = rarityItemDictionary[rarity];
        if (items.Count == 0)
        {
            return null; // Or handle this case as needed
        }

        int randomIndex = random.Next(items.Count);
        return items[randomIndex];
    }

    ~GatherService() => UnsubscribeToEvent();
}
