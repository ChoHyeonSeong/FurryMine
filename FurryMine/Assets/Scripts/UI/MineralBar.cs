using UnityEngine;
using UnityEngine.UI;

public class MineralBar : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.value = 0;
    }

    private void OnEnable()
    {
        Mine.OnSetSubmitMineral += UpdateBar;
    }

    private void OnDisable()
    {
        Mine.OnSetSubmitMineral -= UpdateBar;
    }

    private void UpdateBar(float ratio)
    {
        _slider.value = ratio;
    }
}
