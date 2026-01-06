using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynthKey : UiControlBase, IDropHandler
{
    public int note;

    public Color pressed_tint;

    private Image sprite;

    void Start()
    {
        sprite = GetComponent<Image>();

        AddEventTrigger(EventTriggerType.PointerEnter, () => MouseOver());
        AddEventTrigger(EventTriggerType.PointerExit, () => MouseExit());
        AddEventTrigger(EventTriggerType.PointerDown, () => KeyDown(true));
        AddEventTrigger(EventTriggerType.PointerUp, () => KeyUp());
    }

    public void Tint()
    {
        sprite.color = pressed_tint;
    }

    public void Release()
    {
        sprite.color = Color.white;
    }

    private void MouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            dispatcher.KeyPressed(this, true);
        }
    }

    private void MouseExit()
    {
        if (Input.GetMouseButton(0))
        {
            dispatcher.KeyReleased(this);
        }
    }

    public void KeyDown( bool human_push = false)
    {
        dispatcher.KeyPressed(this, human_push);
    }

    public void KeyUp()
    {
        dispatcher.KeyReleased(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableWeight weight = dropped.GetComponent<DraggableWeight>();
        if (weight != null)
        {
            weight.parentAfterDrag = transform;
            KeyDown();
        }
    }
}
