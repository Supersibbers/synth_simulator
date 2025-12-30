using UnityEngine;
using UnityEngine.UI;

public class SynthKnob : SynthSwitch
{
    public Sprite[] sprites;

    protected override void KeyDown()
    {
        state = (state + 1) % states;
        GetComponent<Image>().sprite = sprites[state];
        
        DispatchMessage();
    }
}
