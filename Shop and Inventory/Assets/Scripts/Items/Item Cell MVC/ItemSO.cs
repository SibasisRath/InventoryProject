using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private ItemCategory type;
    [SerializeField] private Items item;
    [SerializeField] private Sprite icon;
    [SerializeField] private string itemDescription;
    [SerializeField] private int buyingPrice;
    [SerializeField] private int sellingPrice;
    [SerializeField] private int weight;
    [SerializeField] private RarityLevel rarity;

    public ItemCategory Type { get => type;}
    public Items Item { get => item;}
    public Sprite Icon { get => icon; }
    public string ItemDescription { get => itemDescription; }
    public int BuyingPrice { get => buyingPrice; }
    public int SellingPrice { get => sellingPrice; }
    public int Weight { get => weight; }
    public RarityLevel Rarity { get => rarity; }
}
