using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class UpdatePanel : MonoBehaviour
{
    [SerializeField]
    private Slider _updateSlider;
    [SerializeField]
    private TextMeshProUGUI _updateTitle;
    [SerializeField]
    private TextMeshProUGUI _updatePercentText;

    public void SetTextAndSlider(string text, float value)
    {
        _updateSlider.value = value;
        _updatePercentText.text = text;
    }

    private void Awake()
    {
        DownloadManager.OnUpdateDownload += UpdateTextAndSlider;
        DownloadManager.OnUpdateStartLoading += UpdateTextAndSlider;
        DownloadManager.OnHaveStartLoading += ShowStartLoading;
    }

    private void OnDestroy()
    {
        DownloadManager.OnUpdateDownload -= UpdateTextAndSlider;
        DownloadManager.OnUpdateStartLoading -= UpdateTextAndSlider;
        DownloadManager.OnHaveStartLoading -= ShowStartLoading;
    }

    private void UpdateTextAndSlider(float ratio)
    {
        _updateSlider.value = ratio;
        _updatePercentText.text = $"{(int)(ratio * 100)}%";
    }

    private void ShowStartLoading()
    {
        _updateTitle.text = "게임 시작 중";
        _updateSlider.value = 0;
        _updatePercentText.text = "0%";
    }
}
