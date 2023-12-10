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
    public static Action OnEndExplore { get; set; }

    [SerializeField]
    private int _requireMoney;
    [SerializeField]
    private GameObject _mineMap;

    private MineCart _mineCart;
    private MapGenerator _mapGenerator;

    private void Awake()
    {
        _mapGenerator = GetComponentInChildren<MapGenerator>();
        ExplorePanel.OnClickConfirm += CheckMoney;
        ExplorePanel.OnClickCancel += CancelExplore;
        GameApp.OnGameStart += GameStart;
        Cave.OnCompleteExplore += EndExplore;
    }

    private void OnDestroy()
    {
        GameApp.OnGameStart -= GameStart;
        ExplorePanel.OnClickCancel -= CancelExplore;
        ExplorePanel.OnClickConfirm -= CheckMoney;
        Cave.OnCompleteExplore -= EndExplore;
    }

    private void Start()
    {
        _mineMap.SetActive(false);
        gameObject.SetActive(false);
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
            _mapGenerator.GenerateMineSignature();
            _mineCart.MinusMoney(_requireMoney);
            _mineMap.SetActive(true);
        }
    }

    private void EndExplore()
    {
        StartCoroutine(ReserveEnd(3));
    }

    private void CancelExplore()
    {
        gameObject.SetActive(false);
    }


    private IEnumerator ReserveEnd(float time)
    {
        yield return new WaitForSeconds(time);
        OnEndExplore();
        gameObject.SetActive(false);
        _mineMap.SetActive(false);
    }
}
