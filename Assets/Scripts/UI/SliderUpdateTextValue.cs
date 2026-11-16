using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdateTextValue : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text text;

    public void UpdateText()
    {
        int valueShown = Mathf.RoundToInt(slider.value * 100);
        text.text = valueShown.ToString();
    }
}
