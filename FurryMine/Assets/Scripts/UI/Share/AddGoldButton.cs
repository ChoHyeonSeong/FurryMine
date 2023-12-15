using System;
using UnityEngine;
using UnityEngine.UI;

public class AddGoldButton : MonoBehaviour
{
    public static Action<int> OnAddGold { get; set; }
    private Button _addBtn;

    private void Awake()
    {
        _addBtn = GetComponent<Button>();
        _addBtn.onClick.AddListener(ClickAdd);
    }

    private void ClickAdd()
    {
        OnAddGold(100000);
    }
}
