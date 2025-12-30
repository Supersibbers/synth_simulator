using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UiControlBase : MonoBehaviour
{
    public PdDispatcher dispatcher;
    private EventTrigger trigger;

    protected virtual void Awake()
    {
        if (dispatcher == null)
            Debug.LogError($"{name}: No AudioDispatcher assigned", this);
        
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
