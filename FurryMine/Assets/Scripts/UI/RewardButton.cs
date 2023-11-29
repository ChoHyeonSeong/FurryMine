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
        _remainText.gameObject.SetActive(false);
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
        ForbidShowAd();
        AdManager.ShowRewardedAd();
    }

    private void ForbidShowAd()
    {
        _button.interactable = false;
        _remainText.gameObject.SetActive(true);
    }

    private void AllowShowAd()
    {
        _button.interactable = true;
        _remainText.gameObject.SetActive(false);
    }

    private void UpdateRemainText(int seconds)
    {
        _remainText.text = $"{seconds / 60}:{seconds % 60}";
    }
}
