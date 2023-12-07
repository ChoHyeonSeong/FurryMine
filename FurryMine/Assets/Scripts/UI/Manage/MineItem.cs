using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MineItem : MonoBehaviour, IPointerClickHandler
{
    public static Action<MineItem> OnMiningClick { get; set; }
    public static Action<MineItem> OnSellClick { get; set; }

    public int MineIndex { get => _mineIndex; }

    private bool _isMining;
    private int _mineIndex;
    private static MineItem _prevItem;

    [SerializeField]
    private Button _miningBtn;
    [SerializeField]
    private Button _sellBtn;
    [SerializeField]
    private Image _mineIcon;
    [SerializeField]
    private TextMeshProUGUI _mineName;
    [SerializeField]
    private TextMeshProUGUI _mineGrade;
    [SerializeField]
    private TextMeshProUGUI _mineRemain;
    [SerializeField]
    private GameObject _lightFrame;
    [SerializeField]
    private GameObject _miningLabel;

    public void InitItem(int index, int remain, string name, string grade, int level, Sprite sprite)
    {
        // 매개변수로 Sprite 추가하고
        // OnHeadMiner
        // OnStaffMiner
        // OnInfoMiner
        // Action 변수 추가
        _mineIndex = index;
        _mineName.text = $"{name} 광산 Lv.{level}";
        _mineGrade.text = $"{grade}급";
        _mineRemain.text = remain.ToString();
        //_mineIcon.sprite = sprite;
    }

    public void SetMining(bool isMining)
    {
        _isMining = isMining;
        _miningLabel.SetActive(isMining);
    }

    public void SetRemainText(int remain)
    {
        _mineRemain.text = remain.ToString();
    }

    public void SetIndex(int index)
    {
        _mineIndex = index;
    }

    public void UnselectItem()
    {
        _miningBtn.gameObject.SetActive(false);
        _sellBtn.gameObject.SetActive(false);
        _lightFrame.SetActive(false);
    }

    public void InitSelectItem()
    {
        _prevItem = null;
        UnselectItem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_prevItem != null)
            _prevItem.UnselectItem();
        SelectItem();
        _prevItem = this;
    }

    private void Awake()
    {
        _miningBtn.onClick.AddListener(ClickMining);
        _sellBtn.onClick.AddListener(ClickSell);
    }

    private void SelectItem()
    {
        if (!_isMining)
        {
            _miningBtn.gameObject.SetActive(true);
            _sellBtn.gameObject.SetActive(true);
        }
        _lightFrame.SetActive(true);
    }

    private void ClickMining()
    {
        OnMiningClick(this);
    }
    private void ClickSell()
    {
        OnSellClick(this);
    }
}
