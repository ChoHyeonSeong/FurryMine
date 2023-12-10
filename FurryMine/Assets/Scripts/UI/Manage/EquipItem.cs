using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipItem : MonoBehaviour, IPointerClickHandler
{
    public static Action<EquipItem> OnClickWear { get; set; }
    public int EquipId { get => _equipId; }

    private int _equipId;
    private static EquipItem _prevItem;

    [SerializeField]
    private InfoButton _infoBtn;
    [SerializeField]
    private Button _wearBtn;
    [SerializeField]
    private Image _equipIcon;
    [SerializeField]
    private TextMeshProUGUI _equipName;
    [SerializeField]
    private TextMeshProUGUI _equipRank;
    [SerializeField]
    private GameObject _lightFrame;
    [SerializeField]
    private GameObject _wearLabel;



    public void InitItem(int id, string name, string rank, Sprite sprite)
    {
        // 매개변수로 Sprite 추가하고
        // OnHeadMiner
        // OnStaffMiner
        // OnInfoMiner
        // Action 변수 추가
        _equipId = id;
        _equipName.text = name;
        _equipRank.text = $"Rank {rank}";
        _equipIcon.sprite = sprite;
        _infoBtn.SetItemId(id);
    }

    public void SetWear(bool isWear)
    {
        _wearLabel.SetActive(isWear);
    }

    public void UnselectItem()
    {
        _infoBtn.gameObject.SetActive(false);
        _wearBtn.gameObject.SetActive(false);
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
        _wearBtn.onClick.AddListener(ClickWear);
        _infoBtn.SetIsMinerInfo(false);
    }

    private void SelectItem()
    {
        _infoBtn.gameObject.SetActive(true);
        _wearBtn.gameObject.SetActive(true);
        _lightFrame.SetActive(true);
    }

    private void ClickWear()
    {
        OnClickWear(this);
    }

}
