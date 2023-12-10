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
            case EEnforce.STAFF_MINER_COUNT:
                enforceText = "���� �ο�";
                break;
            case EEnforce.STAFF_MINING_POWER:
                enforceText = "���� ä����";
                break;
            case EEnforce.STAFF_MINING_SPEED:
                enforceText = "���� ä���ӵ�";
                break;
            case EEnforce.STAFF_MOVING_SPEED:
                enforceText = "���� �̵��ӵ�";
                break;
            case EEnforce.MINE_RESPAWN_SPEED:
                enforceText = "����� �ӵ�";
                break;
            case EEnforce.MINE_ORE_COUNT:
                enforceText = "���� ����";
                break;
            case EEnforce.MINE_MINERAL_COUNT:
                enforceText = "���� ����";
                break;
            case EEnforce.MINE_MINERAL_PRICE:
                enforceText = "���� ��ġ";
                break;
            case EEnforce.SNACK_CHARGE_SPEED:
                enforceText = "���� �غ�ð�";
                break;
            case EEnforce.SNACK_DURATION_RATIO:
                enforceText = "���� ���ӽð�";
                break;
            case EEnforce.SNACK_MINING_POWER_BUFF:
                enforceText = "ä���� ����";
                break;
            case EEnforce.SNACK_MINING_SPEED_BUFF:
                enforceText = "ä���ӵ� ����";
                break;
            case EEnforce.SNACK_MOVING_SPEED_BUFF:
                enforceText = "�̵��ӵ� ����";
                break;
            default:
                Debug.Log("���ǵ��� ���� ������ȭ�Դϴ�. : MessageBox-ShowRandomEnforce");
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
