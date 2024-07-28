using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewItemCategory", menuName = "ScriptableObjects/Inventory/ItemCategory")]
public class ItemCategorySO : ScriptableObject
{
    [SerializeField] private ItemCategory type;
    [SerializeField] private Sprite typeImage;
    [SerializeField] private List<ItemSO> items;

    public ItemCategory Type { get => type; }
    public Sprite TypeImage { get => typeImage; }
    public List<ItemSO> Items { get => items; }
}
