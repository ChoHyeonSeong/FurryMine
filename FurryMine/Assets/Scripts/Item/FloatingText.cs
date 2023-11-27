using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FloatingText : MonoBehaviour
{
    public static Action<FloatingText> OnFloatingEnd { get; set; }

    private TextMesh _textMesh;

    private static float _damageTime = 0.5f;
    private static float _goldTime = 0.6f;
    private float _floatingTime;
    private float _damageStartY = 1.2f;
    private float _goldStartY = 1.2f;
    private bool _isGold;
    private Vector2 _startPosition;


    private void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
    }

    public void Init(bool isGold, string text)
    {
        _isGold = isGold;
        _textMesh.text = text;
        _floatingTime = 0;
        _startPosition = transform.localPosition;
        _startPosition.y += _isGold ? _goldStartY : _damageStartY;
        Debug.Log(_startPosition);
        transform.localPosition = _startPosition;
        transform.localScale = Vector3.one;
        transform.DOLocalMoveY(_startPosition.y + 1, _isGold ? _goldTime : _damageTime);
        if (!_isGold)
            transform.DOScale(0, _damageTime).SetEase(Ease.InQuad);
    }

    private void Update()
    {
        _floatingTime += Time.deltaTime;
        if (_floatingTime > (_isGold ? _goldTime : _damageTime))
        {
            OnFloatingEnd(this);
        }
    }
}
