using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExplorePanel : MonoBehaviour
{
    public static Action OnClickCancel { get; set; }
    public static Action OnClickConfirm { get; set; }

    [SerializeField]
    private Button _cancelBtn;
    [SerializeField]
    private Button _confirmBtn;
    [SerializeField]
    private TextMeshProUGUI _exploreContent;

    private MapGenerator _mapGenerator;

    private void Awake()
    {
        _mapGenerator = FindAnyObjectByType<MapGenerator>();
        _cancelBtn.onClick.AddListener(ClickCancel);
        _confirmBtn.onClick.AddListener(ClickConfirm);
        _exploreContent.text = $"탐사를 시작하시겠습니까?\r\n\r\n비용은 <sprite=0>{_mapGenerator.RequireMoney} 입니다.";
    }

    private void ClickCancel()
    {
        OnClickCancel();
    }

    private void ClickConfirm()
    {
        OnClickConfirm();
    }
}
