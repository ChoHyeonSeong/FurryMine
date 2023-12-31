using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    [SerializeField]
    private TempAdPage _tempAd;
    private Button _button;
    private TextMeshProUGUI _remainText;
    private bool _isTempAd;

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
        AdManager.OnCompleteAdLoading += SetTempAd;
    }

    private void OnDisable()
    {
        RewardReceiver.OnRemainCoolTime -= UpdateRemainText;
        RewardReceiver.OnStartCoolTime -= RewardStart;
        RewardReceiver.OnEndCoolTime -= AllowShowAd;
        AdManager.OnCompleteAdLoading -= SetTempAd;
    }

    private void SetTempAd(bool isLoadAd)
    {
        _isTempAd = !isLoadAd;
    }

    private void RewardStart(bool isCoolTime)
    {
        if (isCoolTime)
            ForbidShowAd();
        else
            AllowShowAd();
    }

    private void ShowRewardedAd()
    {
        if (CheckReceivableReward())
        {
            ForbidShowAd();
            if (_isTempAd)
            {
                _tempAd.ShowTempAd();
            }
            else
            {
                AdManager.ShowRewardedAd();
            }
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
        _remainText.text = "������ȭ";
    }

    private void UpdateRemainText(int seconds)
    {
        _remainText.text = $"{seconds / 60}:{seconds % 60}";
    }
}
