using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class EventTirggerTest : MonoBehaviour
{
    void Start()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        var EntryPointEnter = new EventTrigger.Entry();
        EntryPointEnter.eventID = EventTriggerType.PointerEnter;
        EntryPointEnter.callback.AddListener((data) =>
        {
            PrintName((PointerEventData)data);
        });
        eventTrigger.triggers.Add(EntryPointEnter);
    }

    void Update()
    {

    }
    public void SetColor(BaseEventData data)
    {

    }
    public void PrintName(PointerEventData pointerEventData)
    {
        //pointerEventData.hovered
        pointerEventData.pointerEnter.GetComponent<Image>().color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        print(name);
    }

}
