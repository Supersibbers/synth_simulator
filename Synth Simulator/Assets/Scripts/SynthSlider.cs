using System;
using UnityEngine;
using UnityEngine.UI;

public class SynthSlider : UiControlBase
{
    private Slider slider;
    
    public String my_param;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { SliderChanged(); });
        SliderChanged();
    }

    void SliderChanged()
    {
        if (dispatcher)
        {
            dispatcher.UpdateParam(my_param, slider.value);
        }
    }
}
