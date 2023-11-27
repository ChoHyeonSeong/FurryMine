using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralCount : MonoBehaviour
{
    private Miner _miner;
    private TextMesh _countText;

    private void Awake()
    {
        _countText = GetComponentInChildren<TextMesh>();
        _miner = transform.parent.GetComponent<Miner>();
    }

    private void OnEnable()
    {
        _miner.OnChangeMineralCount += UpdateText;
    }

    private void OnDisable()
    {
        _miner.OnChangeMineralCount -= UpdateText;
    }

    private void UpdateText(int count)
    {
        _countText.text = count.ToString();
    }
}
