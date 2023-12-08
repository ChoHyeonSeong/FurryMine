using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MineSignature : MonoBehaviour, IPointerClickHandler
{
    public static Action<string, string, int, Vector3> OnEnterSignature { get; set; }

    public static MineSignature CurrentSignature { get; private set; } = null;

    public MineData MineData { get => _mineData; }

    private MineData _mineData;


    public static void SetNullCurrentSignature()
    {
        CurrentSignature = null;
    }

    public void InitMineData(MineData data)
    {
        _mineData = data;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentSignature == this)
            return;
        CurrentSignature = this;
        OnEnterSignature(TableManager.OreGradeTable[_mineData.OreGradeId].Grade, TableManager.OreTypeTable[_mineData.OreTypeId].Type, TableManager.MineLevelTable[_mineData.MineLevelId].Level, transform.position);
    }
}
