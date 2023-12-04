using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    private TextMeshProUGUI _messageText;

    private void Awake()
    {
        _messageText = GetComponentInChildren<TextMeshProUGUI>();

        gameObject.SetActive(false);
        RewardReceiver.OnRandEnforce += ShowRandomEnforce;
    }

    private void OnDestroy()
    {
        RewardReceiver.OnRandEnforce -= ShowRandomEnforce;
    }

    private void ShowRandomEnforce(EEnforce enforce)
    {
        string enforceText = "강화없음";
        switch (enforce)
        {
            case EEnforce.HEAD_MINING_POWER:
                enforceText = "채광력";
                break;
            case EEnforce.HEAD_MINING_SPEED:
                enforceText = "채광속도";
                break;
            case EEnforce.HEAD_MOVING_SPEED:
                enforceText = "이동속도";
                break;
            case EEnforce.HEAD_CRITICAL_PERCENT:
                enforceText = "강타 확률";
                break;
            case EEnforce.HEAD_CRITICAL_POWER:
                enforceText = "강타 파괴력";
                break;
        }
        _messageText.text = $"랜덤강화 : {enforceText}";
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        StartCoroutine(HideBox());
    }

    private IEnumerator HideBox()
    {
        yield return new WaitForSeconds(2);
        transform.DOScaleX(0, 1).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
