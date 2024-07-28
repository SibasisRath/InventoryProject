using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCellModel
{
    private ItemCategory type;
    private Items item;
    private Sprite icon;
    private string itemDescription;
    private TransactionType transactionType;
    private int buyingPrice;
    private int sellingPrice;
    private int weight;
    private RarityLevel rarity;
    private int quantity;

    public ItemCellModel(ItemSO itemSO) 
    {
        type = itemSO.Type;
        item = itemSO.Item;
        icon = itemSO.Icon;
        itemDescription = itemSO.ItemDescription;
        buyingPrice = itemSO.BuyingPrice;
        sellingPrice = itemSO.SellingPrice;
        weight = itemSO.Weight;
        rarity = itemSO.Rarity;
        quantity = 0;
    }

    public ItemCategory Type { get => type; set => type = value; }
    public Items Item { get => item; set => item = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public string ItemDescription { get => itemDescription; set => itemDescription = value; }
    public int BuyingPrice { get => buyingPrice; set => buyingPrice = value; }
    public int SellingPrice { get => sellingPrice; set => sellingPrice = value; }
    public int Weight { get => weight; set => weight = value; }
    public RarityLevel Rarity { get => rarity; set => rarity = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public TransactionType TransactionType { get => transactionType; set => transactionType = value; }
}
