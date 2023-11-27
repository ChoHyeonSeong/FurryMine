using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public static Action<FloatingText> OnFloatingEnd { get; set; }

    private TextMesh _textMesh;

    private static float _damageTime = 0.2f;
    private static float _goldTime = 0.3f;
    private float _floatingTime;
    private float _damageStartY = 1.2f;
    private float _goldStartY = 1.2f;
    private float _floatingScale = 1;
    private bool _isGold;
    private Vector2 _startPosition;
    private float _endPositionY = 1f;


    private void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
    }

    public void Init(bool isGold, string text)
    {
        _isGold = isGold;
        _textMesh.text = text;
        _floatingTime = 0;
        _startPosition = transform.position;
        _startPosition.y += _isGold ? _goldStartY : _damageStartY;
        transform.position = _startPosition;
        transform.localScale = Vector3.one;
    }
}
