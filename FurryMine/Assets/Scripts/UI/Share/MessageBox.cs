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
        RewardReceiver.OnRandomEnforce += ShowRandomEnforce;
    }

    private void OnDestroy()
    {
        RewardReceiver.OnRandomEnforce -= ShowRandomEnforce;
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
            case EEnforce.STAFF_MINER_COUNT:
                enforceText = "동료 인원";
                break;
            case EEnforce.STAFF_MINING_POWER:
                enforceText = "동료 채광력";
                break;
            case EEnforce.STAFF_MINING_SPEED:
                enforceText = "동료 채광속도";
                break;
            case EEnforce.STAFF_MOVING_SPEED:
                enforceText = "동료 이동속도";
                break;
            case EEnforce.MINE_RESPAWN_SPEED:
                enforceText = "재생성 속도";
                break;
            case EEnforce.MINE_ORE_COUNT:
                enforceText = "광석 갯수";
                break;
            case EEnforce.MINE_MINERAL_COUNT:
                enforceText = "광물 갯수";
                break;
            case EEnforce.MINE_MINERAL_PRICE:
                enforceText = "광물 가치";
                break;
            case EEnforce.SNACK_CHARGE_SPEED:
                enforceText = "새참 준비시간";
                break;
            case EEnforce.SNACK_DURATION_RATIO:
                enforceText = "새참 지속시간";
                break;
            case EEnforce.SNACK_MINING_POWER_BUFF:
                enforceText = "채광력 버프";
                break;
            case EEnforce.SNACK_MINING_SPEED_BUFF:
                enforceText = "채광속도 버프";
                break;
            case EEnforce.SNACK_MOVING_SPEED_BUFF:
                enforceText = "이동속도 버프";
                break;
            default:
                Debug.Log("정의되지 않은 랜덤강화입니다. : MessageBox-ShowRandomEnforce");
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
