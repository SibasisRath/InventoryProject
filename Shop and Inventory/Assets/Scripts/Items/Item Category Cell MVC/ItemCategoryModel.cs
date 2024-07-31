using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCategoryModel 
{
    private ItemCategory itemCategory;
    private List<ItemSO> items;

    public ItemCategoryModel(ItemCategorySO itemCategorySO)
    {
        itemCategory = itemCategorySO.Type;
        items = itemCategorySO.Items;
    }

    public ItemCategory ItemCategory { get => itemCategory; set => itemCategory = value; }
    public List<ItemSO> Items { get => items; set => items = value; }
}
