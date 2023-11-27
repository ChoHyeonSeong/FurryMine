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

    public static Action<EnforceItem> OnInitInformation { get; set; } 

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


    private Button _buyBtn;
    private string _titleStr;

    public void SetText(int level, int coeff, int price)
    {
        _titleText.text = $"{_titleStr} {level}";
        _priceText.text = price.ToString();
        switch (_unit)
        {
            case EUnit.NONE:
                _valueText.text = $"+{level * coeff}";
                break;
            case EUnit.PERCENT:
                _valueText.text = $"+{level * coeff}%";
                break;
        }
    }

    private void Awake()
    {
        _buyBtn = GetComponentInChildren<Button>();
        _titleStr = _titleText.text;
        _buyBtn.onClick.AddListener(BuyEnforce);
    }
    private void Start()
    {
        OnInitInformation(this);
    }

    private void BuyEnforce()
    {
        OnBuyEnforce(this);
    }
}
