using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MineSignature : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Action<string, string, int> OnEnterSignature;
    public static Action OnExitSignature;

    private MineData _mineData;

    public void InitMineData(MineData data)
    {
        _mineData = data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnterSignature(TableManager.OreGradeTable[_mineData.OreGradeId].Grade, TableManager.OreTypeTable[_mineData.OreTypeId].Type, TableManager.MineLevelTable[_mineData.MineLevelId].Level);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExitSignature();
    }
}
