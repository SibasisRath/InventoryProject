using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopService : MonoBehaviour
{
    [SerializeField] private Transform itemTypeArea;
    [SerializeField] private Transform itemListArea;

    [SerializeField] private BaseItemCellView baseCellStructure;

    [SerializeField] private List<ItemCategorySO> itemTypes;

    [SerializeField] private SoundTypes itemSelectedSound;
    [SerializeField] private TransactionType transactionType;

    private List<ItemCategoryController> categoryControllers;

    private ItemCategoryController selectedCategoryCell;

    private EventService eventService;
    private UIService uiService;

    // Start is called before the first frame update
    void Start()
    {
        categoryControllers = new ();

        LoadShopItems();
        EnableInitialItemsToDisplay();
    }

    public void Init(EventService eventService, UIService uIService) 
    {
        this.eventService = eventService;
        this.uiService = uIService;

        foreach (var categoryController in categoryControllers)
        {
            categoryController.Init(eventService, uiService);
        }
    }

    private void LoadShopItems()
    {
        foreach (var itemType in itemTypes)
        {
            // Create the item category model
            ItemCategoryModel itemCategoryModel = new (itemType);

            // Create the item category model
            GameObject categoryCellObject = Instantiate(baseCellStructure.gameObject);
            categoryCellObject.transform.SetParent(itemTypeArea, false);
            categoryCellObject.GetComponent<Image>().sprite = itemType.TypeImage;
            BaseItemCellView categoryCellView = categoryCellObject.GetComponent<BaseItemCellView>();

            // Create the item category controller
            ItemCategoryController itemCategoryController = new(categoryCellView, itemCategoryModel)
            {
                TransactionType = transactionType
            };
            itemCategoryController.GenerateItems(itemListArea);
            // Pass services if already initialized
            if (this.eventService != null && this.uiService != null)
            {
                itemCategoryController.Init(this.eventService, this.uiService);
            }
            categoryCellObject.GetComponent<Button>().onClick.AddListener(() => OnItemCategoryButtonClicked(itemCategoryController));

            categoryControllers.Add(itemCategoryController);
        }
    }

    private void EnableInitialItemsToDisplay()
    {
        if (categoryControllers.Count > 0)
        {
            var firstCategoryController = categoryControllers[0];
            firstCategoryController.SetShopItemsActive(true);
            selectedCategoryCell = firstCategoryController;
        }
    }

    private void OnItemCategoryButtonClicked(ItemCategoryController clickedCategoryController)
    {
        eventService.OnButtonSelected.InvokeEvent(itemSelectedSound);

        var preSelectedCategoryController = selectedCategoryCell;

        if (preSelectedCategoryController != clickedCategoryController)
        {
            preSelectedCategoryController.SetShopItemsActive(false);
        }

        clickedCategoryController.SetShopItemsActive(true);
        selectedCategoryCell = clickedCategoryController;
    }

    public List<ItemCategorySO> ItemTypes => itemTypes;
}
