using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snack : MonoBehaviour
{
    public static Action<float> OnSetSnackTime { get; set; }

    public static float MiningPowerBuff { get; private set; }
    public static float MiningSpeedBuff { get; private set; }
    public static float MovingSpeedBuff { get; private set; }

    private int _chargeTime = 600;
    private int _durationSnack = 10;

    private float _finalChargeTime;
    private float _finalDurationSnack;
    private float _finalMiningPowerBuff;
    private float _finalMiningSpeedBuff;
    private float _finalMovingSpeedBuff;

    private float _currentChargeTime;
    private float _currentDurationSnack;
    private float _snackTimeRatio;
    private bool _isSnack;

    private MinerTeam _minerTeam;


    public void EnforceSnack(EEnforce enforce)
    {
        float figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
        switch (enforce)
        {
            case EEnforce.SNACK_CHARGE_SPEED:
                _finalChargeTime = _chargeTime / figure;
                break;
            case EEnforce.SNACK_DURATION_RATIO:
                _finalDurationSnack = _durationSnack * figure;
                if (_isSnack)
                    _currentDurationSnack += _durationSnack * EnforceManager.GetCoeff(enforce);
                break;
            case EEnforce.SNACK_MINING_POWER_BUFF:
                _finalMiningPowerBuff = figure;
                if (_isSnack)
                    ApplyMiningPowerBuff(_finalMiningPowerBuff);
                break;
            case EEnforce.SNACK_MINING_SPEED_BUFF:
                _finalMiningSpeedBuff = figure;
                if (_isSnack)
                    ApplyMiningSpeedBuff(_finalMiningSpeedBuff);
                break;
            case EEnforce.SNACK_MOVING_SPEED_BUFF:
                _finalMovingSpeedBuff = figure;
                if (_isSnack)
                    ApplyMovingSpeedBuff(_finalMovingSpeedBuff);
                break;
            default:
                Debug.Log("정의되지 않은 강화 입니다.");
                break;
        }
    }

    private void PreGameStart()
    {
        _currentChargeTime = 0;
        _currentDurationSnack = 0;
        _isSnack = false;

        MiningPowerBuff = 1;
        MiningSpeedBuff = 1;
        MovingSpeedBuff = 1;
    }

    private void Awake()
    {
        _minerTeam = GetComponent<MinerTeam>();
    }

    private void OnEnable()
    {
        GameApp.OnPreGameStart += PreGameStart;
    }

    private void OnDisable()
    {
        GameApp.OnPreGameStart -= PreGameStart;
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
                    ApplyBuff(_finalMiningPowerBuff, _finalMiningSpeedBuff, _finalMovingSpeedBuff);
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
                    ApplyBuff(1, 1, 1);
                }
            }
            OnSetSnackTime(_snackTimeRatio);
        }
    }

    private void ApplyBuff(float miningPower, float miningSpeed, float movingSpeed)
    {
        ApplyMiningPowerBuff(miningPower);
        ApplyMiningSpeedBuff(miningSpeed);
        ApplyMovingSpeedBuff(movingSpeed);
    }

    private void ApplyMiningPowerBuff(float miningPower)
    {
        MiningPowerBuff = miningPower;
        _minerTeam.EnforceHead(EEnforce.HEAD_MINING_POWER);
        _minerTeam.EnforceStaff(EEnforce.STAFF_MINING_POWER);
    }

    private void ApplyMiningSpeedBuff(float miningSpeed)
    {
        MiningSpeedBuff = miningSpeed;
        _minerTeam.EnforceHead(EEnforce.HEAD_MINING_SPEED);
        _minerTeam.EnforceStaff(EEnforce.STAFF_MINING_SPEED);
    }

    private void ApplyMovingSpeedBuff(float movingSpeed)
    {
        MovingSpeedBuff = movingSpeed;
        _minerTeam.EnforceHead(EEnforce.HEAD_MOVING_SPEED);
        _minerTeam.EnforceStaff(EEnforce.STAFF_MOVING_SPEED);
    }
}
