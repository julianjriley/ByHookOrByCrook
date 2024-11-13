using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;


// BUG: Percentages don't stay the same after clicking on another slider. 
public class SliderPercentages : MonoBehaviour
{
    private Slider _slider;
   // private TextMeshProUGUI _tmp;

    private void Start()
    {
        _slider = gameObject.GetComponent<Slider>();
        Debug.Log("Slider = " + _slider);
    }
    public void UpdateText(TextMeshProUGUI textRef)
    {
        //_tmp = textRef;
       // textRef.text = "0 %";
        Debug.Log("Text ref = " + textRef);
        textRef.text = Mathf.RoundToInt(_slider.value * 100) + "%";
        Debug.Log("New values on " + _slider + ": " + textRef.text);
    }
}
