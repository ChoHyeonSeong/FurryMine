using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private int _oreId;
    private int _oreDeposit;
    private int _spawnedOreCount;
    private int _minedOreCount;

    private int _oreHealth;
    private float _respawnTime;

    private int _oreCount;
    private int _mineralCount;
    private int _mineralPrice;

    private float _finalRespawnSpeed;
    private int _finalOreCount;
    private int _finalMineralCount;
    private int _finalMineralPrice;

    private OreSpawner _oreSpawner;
    private MineralSpawner _mineralSpawner;

    private void Awake()
    {
        _mineralSpawner = FindAnyObjectByType<MineralSpawner>();
        _oreSpawner = FindAnyObjectByType<OreSpawner>();
    }

    private void OnEnable()
    {
        GameApp.OnPreGameStart += PreGameStart;
        _oreSpawner.IsSpawnable += CheckSpawnable;
    }

    private void OnDisable()
    {
        GameApp.OnPreGameStart -= PreGameStart;
        _oreSpawner.IsSpawnable -= CheckSpawnable;
    }

    private void PreGameStart()
    {
        int index = SaveManager.Save.CurrentMineIndex;
        MineData data = SaveManager.Save.MineDatas[index];
        _oreId = data.OreId;
        _oreDeposit = data.OreDeposit;
        _oreHealth = data.OreHealth;
        _respawnTime = data.RespawnTime;
        _oreCount = data.OreCount;
        _mineralCount = data.MineralCount;
        _mineralPrice = data.MineralPrice;

        _oreSpawner.SetOreHealth(_oreHealth);
    }

    public void EnforceMine(EEnforce enforce)
    {
        float figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
        switch (enforce)
        {
            case EEnforce.MINE_RESPAWN_SPEED:
                _finalRespawnSpeed = figure;
                _oreSpawner.SetRespawnTime(_respawnTime / _finalRespawnSpeed);
                break;
            case EEnforce.MINE_ORE_COUNT:
                _finalOreCount = _oreCount + (int)figure;
                _oreSpawner.SetOreCount(_finalOreCount);
                break;
            case EEnforce.MINE_MINERAL_COUNT:
                _finalMineralCount = _mineralCount + (int)figure;
                _oreSpawner.SetMineralCount(_finalMineralCount);
                break;
            case EEnforce.MINE_MINERAL_PRICE:
                _finalMineralPrice = _mineralPrice + (int)figure;
                _mineralSpawner.SetMineralPrice(_finalMineralPrice);
                break;
        }
    }

    private bool CheckSpawnable()
    {
        if (_oreDeposit == -1)
            return true;
        if (_spawnedOreCount < _oreDeposit)
        {
            ++_spawnedOreCount;
            return true;
        }
        return false;
    }

    private void CheckDepletion()
    {
        if (_oreDeposit == -1)
            return;
        if (_oreDeposit < ++_minedOreCount)
        {
            // 기본 광산으로 바꾸기
        }
    }
}

[Serializable]
public class MineData
{
    public int OreId;
    public int OreDeposit;
    public int OreHealth;
    public float RespawnTime;
    public int OreCount;
    public int MineralCount;
    public int MineralPrice;
}
