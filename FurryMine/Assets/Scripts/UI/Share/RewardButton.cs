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
        _button.interactable = false;
    }

    private void OnEnable()
    {
        RewardReceiver.OnRemainCoolTime += UpdateRemainText;
        RewardReceiver.OnStartCoolTime += RewardStart;
        RewardReceiver.OnEndCoolTime += AllowShowAd;
    }

    private void OnDisable()
    {
        RewardReceiver.OnRemainCoolTime -= UpdateRemainText;
        RewardReceiver.OnStartCoolTime -= RewardStart;
        RewardReceiver.OnEndCoolTime -= AllowShowAd;
    }

    private void RewardStart(bool isCoolTime)
    {
        if(isCoolTime)
            ForbidShowAd();
        else
            AllowShowAd();
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
        for (int i = 0; i < EnforceManager.EnforceCount; i++)
        {
            EEnforce enforce = (EEnforce)i;
            if (EnforceManager.GetLevel(enforce) < EnforceManager.GetLimit(enforce))
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
