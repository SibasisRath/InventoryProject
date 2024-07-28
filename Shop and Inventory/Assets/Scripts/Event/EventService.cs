using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventService
{
    public EventController<SoundTypes> OnButtonSelected { get; private set; }
    public EventController<ItemCellController> OnEntirelySellingAnItem {  get; private set; }
    public EventController OnGatherButtonClicked { get; private set; }
    public EventController<string> OnSpaceUnavailable {  get; private set; } 

    public EventService()
    {
        OnButtonSelected = new ();
        OnEntirelySellingAnItem = new ();
        OnGatherButtonClicked = new ();
        OnSpaceUnavailable = new ();
    }
}
