using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempAdPage : MonoBehaviour
{
    public static Action OnReceiveReward { get; set; }

    [SerializeField]
    private TextMeshProUGUI _timeCountText;

    private int _crtTime;
    private int _limitTime = 5;

    public void ShowTempAd()
    {
        _crtTime = _limitTime;
        _timeCountText.text = $"{_crtTime}ÃÊ ÈÄ¿¡ ±¤°í Ã¢ÀÌ ´ÝÈü´Ï´Ù...";
        StartCoroutine(StartTimeCount());
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator StartTimeCount()
    {
        yield return new WaitForSeconds(1f);
        _crtTime--;
        _timeCountText.text = $"{_crtTime}ÃÊ ÈÄ¿¡ ±¤°í Ã¢ÀÌ ´ÝÈü´Ï´Ù...";
        if (_crtTime == 0)
        {
            OnReceiveReward();
            AdManager.LoadRewardedAd();
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(StartTimeCount());
        }
    }
}
