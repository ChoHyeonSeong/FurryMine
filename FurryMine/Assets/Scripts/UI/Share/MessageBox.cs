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
        string enforceText = "��ȭ����";
        switch (enforce)
        {
            case EEnforce.HEAD_MINING_POWER:
                enforceText = "ä����";
                break;
            case EEnforce.HEAD_MINING_SPEED:
                enforceText = "ä���ӵ�";
                break;
            case EEnforce.HEAD_MOVING_SPEED:
                enforceText = "�̵��ӵ�";
                break;
            case EEnforce.HEAD_CRITICAL_PERCENT:
                enforceText = "��Ÿ Ȯ��";
                break;
            case EEnforce.HEAD_CRITICAL_POWER:
                enforceText = "��Ÿ �ı���";
                break;
        }
        _messageText.text = $"������ȭ : {enforceText}";
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
