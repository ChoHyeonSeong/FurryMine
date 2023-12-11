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
        _exploreContent.text = $"Ž�縦 �����Ͻðڽ��ϱ�?\r\n\r\n����� <sprite=0>{_mapGenerator.RequireMoney} �Դϴ�.";
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
