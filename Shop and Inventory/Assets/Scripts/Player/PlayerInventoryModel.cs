using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryModel
{
    private readonly int maxWeight;
    private int weight;
    private int goldAmount;


    public PlayerInventoryModel(PlayerInventorySO playerInventorySO) 
    {
        maxWeight = playerInventorySO.Weight;
        weight = maxWeight;
        GoldAmount = 0;
    }

    public int Weight { get => weight; set => weight = value; }
    public int GoldAmount { get => goldAmount; set => goldAmount = value; }
    public int MaxWeight => maxWeight;
}
