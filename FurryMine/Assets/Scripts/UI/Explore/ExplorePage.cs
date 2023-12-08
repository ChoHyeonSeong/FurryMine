using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ExplorePage : MonoBehaviour
{
    [SerializeField]
    private int _requireMoney;
    [SerializeField]
    private int _signatureCount;
    [SerializeField]
    private GameObject _mineMap;

    private MineCart _mineCart;
    private MapGenerator _mapGenerator;

    private void Awake()
    {
        _mapGenerator = GetComponentInChildren<MapGenerator>();
        ExplorePanel.OnClickConfirm += CheckMoney;
        GameApp.OnGameStart += GameStart;
    }

    private void OnDestroy()
    {
        GameApp.OnGameStart -= GameStart;
        ExplorePanel.OnClickConfirm -= CheckMoney;
    }

    private void GameStart()
    {
        _mineCart = GameManager.Cart;
    }

    private void CheckMoney()
    {
        if (_mineCart.Money >= _requireMoney)
        {
            // MineSignature »ý¼º
            _mapGenerator.GenerateMap();
            _mapGenerator.GenerateMineSignature(_signatureCount);
            _mineCart.MinusMoney(_requireMoney);
            _mineMap.SetActive(true);
        }
    }
    private void Start()
    {
        _mineMap.SetActive(false);
        gameObject.SetActive(false);
    }
}
