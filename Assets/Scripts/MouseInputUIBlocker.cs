using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class MouseInputUIBlocker : MonoBehaviour
{
    public static bool BlockedByUI;
    private EventTrigger eventTrigger;

    private const string LOG_KEY = "UI-Block";

    private void Awake()
    {
        Console.Log(LOG_KEY, "Unknown");
    }

    private void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            EventTrigger.Entry enterUIEntry = new EventTrigger.Entry();
            // Pointer Enter
            enterUIEntry.eventID = EventTriggerType.PointerEnter;
            enterUIEntry.callback.AddListener((eventData) => { EnterUI(); });
            eventTrigger.triggers.Add(enterUIEntry);

            //Pointer Exit
            EventTrigger.Entry exitUIEntry = new EventTrigger.Entry();
            exitUIEntry.eventID = EventTriggerType.PointerExit;
            exitUIEntry.callback.AddListener((eventData) => { ExitUI(); });
            eventTrigger.triggers.Add(exitUIEntry);
        }
    }

    public void EnterUI()
    {
        BlockedByUI = true;
        Console.Log(LOG_KEY, BlockedByUI);
    }
    public void ExitUI()
    {
        BlockedByUI = false;
        Console.Log(LOG_KEY, BlockedByUI);
    }

}