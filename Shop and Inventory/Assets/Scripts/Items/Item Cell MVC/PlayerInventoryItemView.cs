using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amount;

    private ItemCellController cellController;

    public void UpdateNumber(int num) 
    {
        amount.text = num.ToString();
    }

    public void SetCellController(ItemCellController itemCellController) 
    {
        this.cellController = itemCellController;
    }

    public ItemCellController GetCellController() 
    {
        return cellController;
    }

    public void UpdatePlayerInventoryItemView(int quantity)
    {
        amount.text = quantity.ToString();
    }
}
