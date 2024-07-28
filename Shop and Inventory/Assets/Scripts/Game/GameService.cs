using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameService : MonoBehaviour
{
    // Services:
    private EventService eventService;
    private SoundService soundService;
    private GatherService gatherService;
    [SerializeField] private PlayerInventoryView playerInventoryView;
    [SerializeField] private PlayerInventorySO playerInventorySO;
    [SerializeField] private ShopService shop;
    [SerializeField] private UIService uiService;
    [SerializeField] private SoundSO soundSO;

    // Scene References:
    [SerializeField] private AudioSource sfxSource;

    private PlayerInventoryController playerInventoryController;

    private void Start()
    {
        InitializeServices();
        InjectDependencies();
    }

    private void InitializeServices()
    {
        eventService = new ();
        soundService = new (soundSO, sfxSource);
        playerInventoryController = new (playerInventoryView, playerInventorySO);
        gatherService = new ();
    }

    private void InjectDependencies()
    {
        soundService.Init(eventService);
        shop.Init(eventService, uiService);
        uiService.Init(playerInventoryController, eventService);
        playerInventoryController.Init(shop, eventService, uiService);
        gatherService.Init(playerInventoryController, eventService, shop);
    }
}