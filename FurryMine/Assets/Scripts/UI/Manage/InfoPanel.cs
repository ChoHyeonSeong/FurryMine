using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField]
    private Image _mainIcon;
    [SerializeField]
    private Image _subIcon;
    [SerializeField]
    private TextMeshProUGUI _mainNameText;
    [SerializeField]
    private TextMeshProUGUI _subTitleText;
    [SerializeField]
    private TextMeshProUGUI _miningPowerText;
    [SerializeField]
    private TextMeshProUGUI _miningSpeedText;
    [SerializeField]
    private TextMeshProUGUI _movingSpeedText;
    [SerializeField]
    private TextMeshProUGUI _miningCountText;
    [SerializeField]
    private TextMeshProUGUI _criticalPercentText;
    [SerializeField]
    private TextMeshProUGUI _criticalPowerText;

    private MinerTeam _minerTeam;
    private string _minerSubTitle = "Âø¿ëÇÑ Àåºñ";
    private string _equipSubTitle = "Âø¿ëÇÑ ±¤ºÎ";

    private void Awake()
    {
        _minerTeam = FindAnyObjectByType<MinerTeam>();
        InfoButton.OnPointerDownMinerInfo += ShowMinerInfo;
        InfoButton.OnPointerDownEquipInfo += ShowEquipInfo;
        InfoButton.OnPointerUpInfo += HideInfoPanel;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        InfoButton.OnPointerDownMinerInfo -= ShowMinerInfo;
        InfoButton.OnPointerDownEquipInfo -= ShowEquipInfo;
        InfoButton.OnPointerUpInfo -= HideInfoPanel;
    }

    private void ShowMinerInfo(int minerId, Vector2 infoBtnPos)
    {
        transform.position = infoBtnPos;
        MinerEntity entity = TableManager.MinerTable[minerId];
        _mainNameText.text = entity.Name;
        _subTitleText.text = _minerSubTitle;
        _miningPowerText.text = entity.MiningPower.ToString();
        _miningSpeedText.text = $"{entity.MiningSpeed}½ºÀ®/s";
        _movingSpeedText.text = $"{entity.MovingSpeed}°ÉÀ½/s";
        _miningCountText.text = entity.MiningCount.ToString();
        _criticalPercentText.text = $"{entity.CriticalPercent * 100}%";
        _criticalPowerText.text = $"{entity.CriticalPower * 100}%";
        _mainIcon.sprite = ResourceManager.MinerIconList[minerId];
        Miner miner = _minerTeam.GetMiner(minerId);
        if (miner != null && miner.EquipId != -1)
        {
            _subIcon.sprite = ResourceManager.EquipIconList[miner.EquipId];
            _subIcon.gameObject.SetActive(true);
        }
        else
        {
            _subIcon.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    private void ShowEquipInfo(int equipId, Vector2 infoBtnPos)
    {
        transform.position = infoBtnPos;
        EquipEntity entity = TableManager.EquipTable[equipId];
        _mainNameText.text = entity.Name;
        _subTitleText.text = _equipSubTitle;
        _miningPowerText.text = $"+{entity.MiningPower}";
        _miningSpeedText.text = $"+{entity.MiningSpeed * 100}%";
        _movingSpeedText.text = $"+{entity.MovingSpeed * 100}%";
        _miningCountText.text = $"+{entity.MiningCount}";
        _criticalPercentText.text = $"+{entity.CriticalPercent * 100}%";
        _criticalPowerText.text = $"+{entity.CriticalPower * 100}%";
        _mainIcon.sprite = ResourceManager.EquipIconList[equipId];
        if (_minerTeam.CurrentMinerEquip.ContainsKey(equipId))
        {
            _subIcon.sprite = ResourceManager.MinerIconList[_minerTeam.CurrentMinerEquip[equipId]];
            _subIcon.gameObject.SetActive(true);
        }
        else
        {
            _subIcon.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    private void HideInfoPanel()
    {
        gameObject.SetActive(false);
    }
}
