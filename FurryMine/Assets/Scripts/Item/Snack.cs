using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snack : MonoBehaviour
{
    public static Action<float> OnSetSnackTime { get; set; }

    private int _chargeTime = 30;
    private int _durationSnack = 10;

    private int _chargeSpeed = 1;
    private int _durationRatio = 1;
    private float _miningPowerBoost = 1;
    private float _miningSpeedBoost = 1;
    private float _movingSpeedBoost = 1;

    private float _finalChargeTime;
    private float _finalDurationSnack;
    private float _finalMiningPowerBoost;
    private float _finalMiningSpeedBoost;
    private float _finalMovingSpeedBoost;

    private float _currentChargeTime;
    private float _currentDurationSnack;
    private float _snackTimeRatio;
    private bool _isSnack;


    public void EnforceSnack(EEnforce enforce)
    {

    }

    private void GameStart()
    {
        _currentChargeTime = 0;
        _currentDurationSnack = 0;
        _isSnack = false;

        _finalChargeTime = _chargeTime;
        _finalDurationSnack = _durationSnack;
    }

    private void OnEnable()
    {
        GameApp.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        GameApp.OnGameStart -= GameStart;
    }

    private void Update()
    {
        if (GameApp.IsGameStart)
        {
            if (!_isSnack)
            {
                _currentChargeTime += Time.deltaTime;
                _snackTimeRatio = _currentChargeTime / _finalChargeTime;
                if (_currentChargeTime >= _finalChargeTime)
                {
                    _isSnack = true;
                    _currentDurationSnack = _finalDurationSnack;
                }
            }
            else
            {
                _currentDurationSnack -= Time.deltaTime;
                _snackTimeRatio = _currentDurationSnack / _finalDurationSnack;
                if (_currentDurationSnack <= 0)
                {
                    _isSnack = false;
                    _currentChargeTime = 0;
                }
            }
            OnSetSnackTime(_snackTimeRatio);
        }
    }
}
