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

    private void Awake()
    {
        _cancelBtn.onClick.AddListener(ClickCancel);
        _confirmBtn.onClick.AddListener(ClickConfirm);
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
