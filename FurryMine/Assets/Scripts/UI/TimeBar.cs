using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.value = 1;
    }

    private void OnEnable()
    {
        MineCart.OnChangeTime += UpdateBar;
    }

    private void OnDisable()
    {
        MineCart.OnChangeTime -= UpdateBar;
    }

    private void UpdateBar(float ratio)
    {
        _slider.value = ratio;
    }
}
