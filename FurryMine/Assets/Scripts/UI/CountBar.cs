using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountBar : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.value = 0;
    }

    private void OnEnable()
    {
        MineCart.OnChangeCount += UpdateBar;
    }

    private void OnDisable()
    {
        MineCart.OnChangeCount -= UpdateBar;
    }

    private void UpdateBar(float ratio)
    {
        _slider.value = ratio;
    }
}
