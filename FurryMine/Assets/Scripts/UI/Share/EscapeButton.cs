using System;
using UnityEngine;
using UnityEngine.UI;

public class EscapeButton : MonoBehaviour
{
    public static Action OnClickEscape { get; set; }

    [SerializeField]
    private Button _escapeBtn;

    private void Awake()
    {
        _escapeBtn.onClick.AddListener(ClickEscape);
    }

    private void ClickEscape()
    {
        OnClickEscape();
    }
}
