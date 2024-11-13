using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System;


// BUG: Percentages don't stay the same after clicking on another slider. 
public class SliderPercentages : MonoBehaviour
{
    [SerializeField]
    private OptionsMenuManager _optionsMenuManager;

    [SerializeField]
    private int _index;


    public void UpdateText()
    {
        _optionsMenuManager._tmpList[_index].text = Mathf.RoundToInt(_optionsMenuManager._sliderList[_index].value * 100) + "%";
    }
}
