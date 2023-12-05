using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectMinerItem : MonoBehaviour, IPointerClickHandler
{
    public static Action<int> OnSelectClick { get; set; }


    private int _minerId;
    private static SelectMinerItem _prevItem;

    [SerializeField]
    private Button _infoBtn;
    [SerializeField]
    private Button _selectBtn;
    [SerializeField]
    private Image _minerIcon;
    [SerializeField]
    private TextMeshProUGUI _minerName;
    [SerializeField]
    private TextMeshProUGUI _minerRank;
    [SerializeField]
    private GameObject _lightFrame;

    private void Awake()
    {
        _selectBtn.onClick.AddListener(ClickSelect);
    }

    public void InitItem(int id, string name, string rank, Sprite sprite)
    {
        // 매개변수로 Sprite 추가하고
        // OnHeadMiner
        // OnStaffMiner
        // OnInfoMiner
        // Action 변수 추가
        _minerId = id;
        _minerName.text = name;
        _minerRank.text = $"Rank {rank}";
        _minerIcon.sprite = sprite;
    }
    public void UnselectItem()
    {
        _infoBtn.gameObject.SetActive(false);
        _selectBtn.gameObject.SetActive(false);
        _lightFrame.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_prevItem != null)
            _prevItem.UnselectItem();
        SelectItem();
        _prevItem = this;
    }

    private void SelectItem()
    {
        _infoBtn.gameObject.SetActive(true);
        _selectBtn.gameObject.SetActive(true);
        _lightFrame.SetActive(true);
    }

    private void ClickSelect()
    {
        UnselectItem();
        OnSelectClick(_minerId);
    }

    private void ClickInfo()
    {
        //OnInfoClick(this);
    }
}
