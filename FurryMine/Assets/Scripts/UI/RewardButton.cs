using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    private Button _button;
    private TextMeshProUGUI _remainText;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _remainText = GetComponentInChildren<TextMeshProUGUI>();
        _button.onClick.AddListener(ShowRewardedAd);
    }

    private void OnEnable()
    {
        RewardReceiver.OnRemainCoolTime += UpdateRemainText;
        RewardReceiver.OnEndCoolTime += AllowShowAd;
        GameApp.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        RewardReceiver.OnRemainCoolTime -= UpdateRemainText;
        RewardReceiver.OnEndCoolTime -= AllowShowAd;
        GameApp.OnGameStart -= GameStart;
    }

    private void GameStart()
    {
        if (SaveManager.Save.RemainCoolTime > 0)
        {
            ForbidShowAd();
        }
    }

    private void ShowRewardedAd()
    {
        if (CheckReceivableReward())
        {
            ForbidShowAd();
            AdManager.ShowRewardedAd();
        }
    }

    private bool CheckReceivableReward()
    {
        var enumArray = Enum.GetValues(typeof(EEnforce));
        foreach (EEnforce enforce in enumArray)
        {
            if (EnforceManager.LevelDict[enforce] < EnforceManager.LimitDict[enforce])
                return true;
        }
        return false;
    }

    private void ForbidShowAd()
    {
        _button.interactable = false;
    }

    private void AllowShowAd()
    {
        _button.interactable = true;
        _remainText.text = "·£´ý°­È­";
    }

    private void UpdateRemainText(int seconds)
    {
        _remainText.text = $"{seconds / 60}:{seconds % 60}";
    }
}
