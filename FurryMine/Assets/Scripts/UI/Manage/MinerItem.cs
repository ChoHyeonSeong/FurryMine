using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinerItem : MonoBehaviour, IPointerClickHandler
{
    public static Func<int> GetHeadId { get; set; }
    public static Action<MinerItem, MinerItem> OnClickHead { get; set; }
    public static Action<MinerItem> OnClickStaff { get; set; }

    public EMinerLabel MinerLabel { get => _minerLabel; }
    public int MinerId { get => _minerId; }

    private int _minerId;
    private static MinerItem _prevItem;
    [SerializeField]
    private InfoButton _infoBtn;
    [SerializeField]
    private Button _staffBtn;
    [SerializeField]
    private Button _headBtn;
    [SerializeField]
    private Image _minerIcon;
    [SerializeField]
    private TextMeshProUGUI _minerName;
    [SerializeField]
    private TextMeshProUGUI _minerRank;
    [SerializeField]
    private GameObject _lightFrame;
    [SerializeField]
    private GameObject _headLabel;
    [SerializeField]
    private GameObject _staffLabel;

    private MinerContent _minerContent;
    private GameObject _prevLabel;
    private EMinerLabel _minerLabel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_prevItem != null)
            _prevItem.UnselectItem();
        SelectItem();
        _prevItem = this;
    }

    public void UnselectItem()
    {
        _infoBtn.gameObject.SetActive(false);
        _staffBtn.gameObject.SetActive(false);
        _headBtn.gameObject.SetActive(false);
        _lightFrame.SetActive(false);
    }

    public void InitSelectItem()
    {
        _prevItem = null;
        UnselectItem();
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
        _infoBtn.SetItemId(id);
    }

    public void SetLabel(EMinerLabel label)
    {
        if (_prevLabel != null)
            _prevLabel.SetActive(false);
        switch (label)
        {
            case EMinerLabel.HEAD:
                _headLabel.SetActive(true);
                _prevLabel = _headLabel;
                break;
            case EMinerLabel.STAFF:
                _staffLabel.SetActive(true);
                _prevLabel = _staffLabel;
                break;
            case EMinerLabel.NONE:
                _prevLabel = null;
                break;
        }
        _minerLabel = label;
    }

    private void Awake()
    {
        _minerContent = GetComponentInParent<MinerContent>();
        _headBtn.onClick.AddListener(ClickHead);
        _staffBtn.onClick.AddListener(ClickStaff);
        _infoBtn.SetIsMinerInfo(true);
    }


    private void SelectItem()
    {
        _infoBtn.gameObject.SetActive(true);
        if (_minerLabel != EMinerLabel.STAFF)
            _staffBtn.gameObject.SetActive(true);
        if (_minerLabel != EMinerLabel.HEAD)
            _headBtn.gameObject.SetActive(true);
        _lightFrame.SetActive(true);
    }

    private void ClickHead()
    {
        OnClickHead(this, _minerContent.GetMinerItem(GetHeadId()));
    }

    private void ClickStaff()
    {
        OnClickStaff(this);
    }
}
