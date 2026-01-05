using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UiControlBase : MonoBehaviour
{
    public MonosynthDispatcher dispatcher;
    private EventTrigger trigger;

    protected virtual void Awake()
    {   
        trigger = GetComponent<EventTrigger>();
    }

    protected void AddEventTrigger(EventTriggerType eventType, System.Action action)
    {
        var entry = new EventTrigger.Entry
        {
            eventID = eventType
        };

        entry.callback.AddListener(_ => action());
        trigger.triggers.Add(entry);
    }
}
