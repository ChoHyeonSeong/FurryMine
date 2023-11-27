using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelStatus : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelText;

    private void Awake()
    {
        _levelText.text = "1";
    }

    private void OnEnable()
    {
        GameManager.OnLevelUp += UpdateText;
    }

    private void OnDisable()
    {
        GameManager.OnLevelUp -= UpdateText;
    }

    private void UpdateText(int level)
    {
        _levelText.text = level.ToString();
    }
}
