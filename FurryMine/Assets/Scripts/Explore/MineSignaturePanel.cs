using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class MineSignaturePanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _mineLevelText;
    [SerializeField]
    private TextMeshProUGUI _oreTypeText;
    [SerializeField]
    private TextMeshProUGUI _oreGradeText;

    private void Awake()
    {
        MineSignature.OnEnterSignature += SetText;
        MineSignature.OnExitSignature += InitText;
    }

    private void SetText(string grade, string type, int level)
    {
        _oreGradeText.text = $"{grade}±Þ";
        _oreTypeText.text = $"{type} ±¤»ê";
        _mineLevelText.text = $"Lv. {level}";
    }

    private void InitText()
    {
        _oreGradeText.text = "";
        _oreTypeText.text = "";
        _mineLevelText.text = "";
    }
}
