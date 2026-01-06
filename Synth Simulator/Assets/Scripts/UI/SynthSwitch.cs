using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynthSwitch : UiControlBase
{
    public string my_param;
    public int states;
    public int initial_state;
    public int offset;

    protected Image sprite;
    protected Vector2 initial_position;
    protected int state;
    protected RectTransform rt;

    protected virtual void Start()
    {
        rt = GetComponent<RectTransform>();
        initial_position = rt.anchoredPosition;
        sprite = GetComponent<Image>();
        state = initial_state;
        AddEventTrigger(EventTriggerType.PointerDown, KeyDown);
        DispatchMessage();
    }

    protected virtual void KeyDown()
    {
        state = (state + 1) % states;
        if (state == 0) { rt.anchoredPosition = initial_position + new Vector2(0, offset); };
        if (state == 1) { rt.anchoredPosition = initial_position; };
        if (state == 2) { rt.anchoredPosition = initial_position - new Vector2(0, offset); };
        DispatchMessage();
    }

    protected void DispatchMessage()
    {
        if (dispatcher)
        {
            float f = state;
            dispatcher.UpdateParam(my_param, f);
        }
    }

}
