using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public static Action<int, bool> OnSetMining { get; set; }
    public static Action<int, int> OnCheckDepletion { get; set; }
    public static Action<int> OnRemoveMine { get; set; }
    public static Action<MineData> OnAddMine { get; set; }
    public static Action<int> OnSellMine { get; set; }

    public int CurrentMineIndex { get => _currentMineIndex; }
    public List<MineData> MineDataList { get => _mineDataList; }

    private int _currentMineIndex;
    private int _oreDeposit;

    private int _oreHealth;
    private float _respawnTime;

    private int _oreCount;
    private int _mineralCount;
    private int _mineralPrice;

    private float _finalRespawnSpeed;
    private int _finalOreCount;
    private int _finalMineralCount;
    private int _finalMineralPrice;

    private MinerTeam _minerTeam;
    private OreSpawner _oreSpawner;
    private MineralSpawner _mineralSpawner;

    // 가지고 있는 광산목록
    private List<MineData> _mineDataList;

    private void Awake()
    {
        _minerTeam = FindAnyObjectByType<MinerTeam>();
        _oreSpawner = FindAnyObjectByType<OreSpawner>();
        _mineralSpawner = FindAnyObjectByType<MineralSpawner>();
    }

    private void OnEnable()
    {
        GameApp.OnPreGameStart += PreGameStart;
        MineItem.OnMiningClick += SetMiningMine;
        SellPanel.OnClickConfirmSelling += SellMine;
        Cave.OnDiscoverMine += AddMineData;
        _oreSpawner.IsSpawnable += CheckSpawnable;
        _oreSpawner.OnCollectOre += CheckDepletion;
    }

    private void OnDisable()
    {
        GameApp.OnPreGameStart -= PreGameStart;
        MineItem.OnMiningClick -= SetMiningMine;
        SellPanel.OnClickConfirmSelling -= SellMine;
        Cave.OnDiscoverMine -= AddMineData;
        _oreSpawner.IsSpawnable -= CheckSpawnable;
        _oreSpawner.OnCollectOre -= CheckDepletion;
    }

    private void PreGameStart()
    {
        _mineDataList = SaveManager.Save.MineDatas.ToList();
        ChangeMine(SaveManager.Save.CurrentMineIndex);
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

    private bool CheckSpawnable(int currentOreCount)
    {
        if (_currentMineIndex == 0)
            return true;
        return currentOreCount < _oreDeposit;
    }

    private void CheckDepletion()
    {
        if (_currentMineIndex == 0)
            return;
        if (--_oreDeposit <= 0)
        {
            // 현재 광산 제거
            _mineDataList.RemoveAt(_currentMineIndex);
            OnRemoveMine(_currentMineIndex);

            // 기본 광산으로 바꾸기
            ChangeMine(0);
            RecalculateMineStat();
            _minerTeam.GoToOtherMine();
            _oreSpawner.CollectAllOre();

#if UNITY_EDITOR
#else
            SaveManager.SaveGame();
#endif
            return;
        }
        _mineDataList[_currentMineIndex].OreDeposit = _oreDeposit;
        OnCheckDepletion(_currentMineIndex, _oreDeposit);
    }

    private void ChangeMine(int index)
    {
        MineData data = _mineDataList[index];
        MineLevelEntity mineLevelEntity = TableManager.MineLevelTable[data.MineLevelId];
        OreTypeEntity oreTypeEntity = TableManager.OreTypeTable[data.OreTypeId];
        OreGradeEntity oreGradeEntity = TableManager.OreGradeTable[data.OreGradeId];
        _oreDeposit = data.OreDeposit;
        _oreHealth = (int)(oreTypeEntity.BaseHealth * oreGradeEntity.CoeffHealth);
        _respawnTime = mineLevelEntity.RespawnTime;
        _oreCount = mineLevelEntity.OreCount;
        _mineralCount = oreGradeEntity.MineralCount;
        _mineralPrice = oreTypeEntity.MineralPrice;
        _currentMineIndex = index;
        _oreSpawner.SetOreHealth(_oreHealth);
    }

    private void RecalculateMineStat()
    {
        for (int i = 9; i < 13; i++)
        {
            EnforceMine((EEnforce)i);
        }
    }

    private void SetMiningMine(MineItem mineItem)
    {
        if (_currentMineIndex != 0)
        {
            _mineDataList[_currentMineIndex].OreDeposit = _oreDeposit;
            OnSetMining(_currentMineIndex, false);
        }
        ChangeMine(mineItem.MineIndex);
        RecalculateMineStat();
        mineItem.SetMining(true);
        _minerTeam.GoToOtherMine();
        _oreSpawner.CollectAllOre();
        mineItem.InitSelectItem();

#if UNITY_EDITOR
#else
            SaveManager.SaveGame();
#endif
    }

    private void SellMine(MineItem mineItem)
    {
        MineData data = _mineDataList[mineItem.MineIndex];
        OreTypeEntity oreTypeEntity = TableManager.OreTypeTable[data.OreTypeId];
        OreGradeEntity oreGradeEntity = TableManager.OreGradeTable[data.OreGradeId];
        OnSellMine((int)(data.OreDeposit * oreTypeEntity.MineralPrice * oreGradeEntity.MineralCount * 0.6f));
        if (mineItem.MineIndex < _currentMineIndex)
            _currentMineIndex--;
        _mineDataList.RemoveAt(mineItem.MineIndex);
        OnRemoveMine(mineItem.MineIndex);
    }

    private void AddMineData(MineData data)
    {
        _mineDataList.Add(data);
        OnAddMine(data);
#if UNITY_EDITOR
#else
            SaveManager.SaveGame();
#endif
    }
}

[Serializable]
public class MineData
{
    public int OreTypeId;
    public int OreGradeId;
    public int MineLevelId;
    public int OreDeposit;
}
