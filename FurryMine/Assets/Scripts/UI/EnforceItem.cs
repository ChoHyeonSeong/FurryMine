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


    private Button _buyBtn;
    private string _titleStr;

    public void SetText(int level, float coeff, int price)
    {
        int value;
        _titleText.text = $"{_titleStr} {level}";
        _priceText.text = price.ToString();
        switch (_unit)
        {
            case EUnit.NONE:
                value = (int)coeff;
                _valueText.text = $"+{level * value}";
                break;
            case EUnit.PERCENT:
                value = (int)(coeff * 101);
                _valueText.text = $"+{level * value}%";
                break;
        }
    }

    private void Awake()
    {
        _buyBtn = GetComponentInChildren<Button>();
        _titleStr = _titleText.text;
        _buyBtn.onClick.AddListener(BuyEnforce);
    }

    private void OnEnable()
    {
        GameApp.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        GameApp.OnGameStart -= GameStart;
    }

    private void GameStart()
    {
        SetText(EnforceManager.LevelDict[Enforce], EnforceManager.CoeffDict[Enforce], EnforceManager.PriceDict[Enforce]);
        if (EnforceManager.LevelDict[_enforce] >= EnforceManager.LimitDict[_enforce])
        {
            _buyBtn.interactable = false;
        }
    }

    private void BuyEnforce()
    {
        OnBuyEnforce(this);
        if (EnforceManager.LevelDict[_enforce] >= EnforceManager.LimitDict[_enforce])
        {
            _buyBtn.interactable = false;
        }
    }
}
