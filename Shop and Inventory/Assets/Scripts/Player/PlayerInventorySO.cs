using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Inventory SO", menuName = "ScriptableObjects/Inventory/PlayerInventory")]
public class PlayerInventorySO : ScriptableObject
{
    [SerializeField] private int weight;

    public int Weight { get => weight;}
}
