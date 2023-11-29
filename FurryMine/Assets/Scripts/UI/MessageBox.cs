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
        string enforceText = "°­È­¾øÀ½";
        switch (enforce)
        {
            case EEnforce.HEAD_MINING_POWER:
                enforceText = "Ã¤±¤·Â";
                break;
            case EEnforce.HEAD_MINING_SPEED:
                enforceText = "Ã¤±¤¼Óµµ";
                break;
            case EEnforce.HEAD_MOTION_SPEED:
                enforceText = "°î±ªÀÌ¼Óµµ";
                break;
            case EEnforce.HEAD_MOVING_SPEED:
                enforceText = "ÀÌµ¿¼Óµµ";
                break;
            case EEnforce.HEAD_CRITICAL_PERCENT:
                enforceText = "°­Å¸ È®·ü";
                break;
            case EEnforce.HEAD_CRITICAL_POWER:
                enforceText = "°­Å¸ ÆÄ±«·Â";
                break;
        }
        _messageText.text = $"·£´ý°­È­ : {enforceText}";
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
