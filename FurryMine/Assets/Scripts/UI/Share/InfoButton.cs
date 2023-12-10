using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Action<int, Vector2> OnPointerDownMinerInfo { get; set; }
    public static Action<int, Vector2> OnPointerDownEquipInfo { get; set; }
    public static Action OnPointerUpInfo { get; set; }

    private int _itemId;
    private bool _isMinerInfo;

    public void SetIsMinerInfo(bool isMinerInfo)
    {
        _isMinerInfo = isMinerInfo;
    }

    public void SetItemId(int itemId)
    {
        _itemId = itemId;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isMinerInfo)
            OnPointerDownMinerInfo(_itemId, transform.position);
        else
            OnPointerDownEquipInfo(_itemId, transform.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpInfo();
    }
}
