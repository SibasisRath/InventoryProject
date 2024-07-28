using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseItemCellView : MonoBehaviour
{
    [SerializeField] private SoundTypes cellSelectedSound;

    private ItemCellController cellController;

    public SoundTypes CellSelectedSound { get => cellSelectedSound; }

    public void SetCellController(ItemCellController itemCellController)
    {
        this.cellController = itemCellController;
    }

    public ItemCellController GetCellController()
    {
        return cellController;
    }
}
