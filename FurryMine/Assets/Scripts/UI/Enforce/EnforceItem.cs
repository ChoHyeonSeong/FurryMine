using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EUnit
{
    NONE,
    PERCENT
}

public class EnforceItem : MonoBehaviour
{
    public static Action<EnforceItem> OnBuyEnforce { get; set; }

    public EEnforce Enforce { get => _enforce; }

    public EUnit Unit { get => _unit; }

    [SerializeField]
    private EEnforce _enforce;

    [SerializeField]
    private EUnit _unit;

    [SerializeField]
    private TextMeshProUGUI _titleText;

    [SerializeField]
    private TextMeshProUGUI _valueText;

    [SerializeField]
    private TextMeshProUGUI _priceText;

    [SerializeField]
    private int _roundDigit;

    private Button _buyBtn;
    private string _titleStr;

    public void SetText(int level, float coeff, int price)
    {
        _titleText.text = $"{_titleStr} {level}";
        _priceText.text = price.ToString();
        switch (_unit)
        {
            case EUnit.NONE:
                _valueText.text = $"+{(int)(level * coeff)}";
                break;
            case EUnit.PERCENT:
                _valueText.text = $"+{System.Math.Round(level * coeff * 100, _roundDigit)}%";
                break;
        }
    }
    public void BlockEnforce()
    {
        _buyBtn.interactable = false;
    }

    private void Awake()
    {
        _buyBtn = GetComponentInChildren<Button>();
        _titleStr = _titleText.text;
        _buyBtn.onClick.AddListener(BuyEnforce);
        GameApp.OnGameStart += GameStart;
    }


    private void OnDestroy()
    {
        GameApp.OnGameStart -= GameStart;
    }

    private void GameStart()
    {
        SetText(EnforceManager.GetLevel(_enforce), EnforceManager.GetCoeff(_enforce), EnforceManager.GetPrice(_enforce));
        if (EnforceManager.GetLevel(_enforce) >= EnforceManager.GetLimit(_enforce))
        {
            _buyBtn.interactable = false;
        }
    }

    private void BuyEnforce()
    {
        OnBuyEnforce(this);
    }

}
