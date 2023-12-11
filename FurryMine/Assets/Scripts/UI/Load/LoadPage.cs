using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPage : MonoBehaviour
{
    [SerializeField]
    private GameObject _checkPanel;
    [SerializeField]
    private UpdatePanel _updatePanel;

    private void Awake()
    {
        _checkPanel.SetActive(true);
        _updatePanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        DownloadManager.OnHaveDownload += ShowUpdatePanel;
        DownloadManager.OnHaveNotDownload += ShowNotUpdatePanel;
    }

    private void OnDisable()
    {
        DownloadManager.OnHaveDownload -= ShowUpdatePanel;
        DownloadManager.OnHaveNotDownload -= ShowNotUpdatePanel;
    }

    private void ShowUpdatePanel()
    {
        _updatePanel.SetTextAndSlider("0%", 0);
        _checkPanel.SetActive(false);
        _updatePanel.gameObject.SetActive(true);
    }

    private void ShowNotUpdatePanel()
    {
        _updatePanel.SetTextAndSlider("100%", 1f);
        _checkPanel.SetActive(false);
        _updatePanel.gameObject.SetActive(true);
    }
}
