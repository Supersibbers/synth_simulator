using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynthKey : UiControlBase
{
    public int note;

    public Color pressed_tint;

    private Image sprite;
    private float lastClickTime;
    private bool held = false;

    void Start()
    {
        sprite = GetComponent<Image>();

        KeyEventBus.OnKeyHeld += Release;

        AddEventTrigger(EventTriggerType.PointerEnter, () => MouseOver(note));
        AddEventTrigger(EventTriggerType.PointerExit, MouseExit);
        AddEventTrigger(EventTriggerType.PointerDown, KeyDown);
        AddEventTrigger(EventTriggerType.PointerUp, KeyUp);
    }

    public void MouseOver(int note)
    {
        if (Input.GetMouseButton(0))
        {
            dispatcher.RecieveNote(note);
            sprite.color = pressed_tint;
        }
    }

    public void MouseExit()
    {
        if (!held) sprite.color = Color.white;
    }

    public void KeyDown()
    {
        KeyEventBus.KeyHeld();
        if (Time.time - lastClickTime < 0.3f)
        {
            held = true;
        }
        else held = false;
        
        lastClickTime = Time.time;

        dispatcher.RecieveNote(note);   
        dispatcher.KeyDown();
        sprite.color = pressed_tint;
    }

    public void KeyUp()
    {
        if (!held)
        {
            dispatcher.KeyUp();
            sprite.color = Color.white;
        }
    }

    public void Release()
    {
        if (held)
        {
            sprite.color = Color.white;
            held = false;
        }
    }
}


public static class KeyEventBus
{
    // Fires when any key is pressed
    public static event Action OnKeyHeld;

    public static void KeyHeld()
    {
        OnKeyHeld?.Invoke();
    }
}
