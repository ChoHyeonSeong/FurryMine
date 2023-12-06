using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineContent : MonoBehaviour
{
    [SerializeField]
    private MineItem _mineItemPrefab;

    private Dictionary<int, MineItem> _mineItems = new Dictionary<int, MineItem>();

    private void Awake()
    {
        GameApp.OnGameStart += GameStart;
        Mine.OnSetMining += SetMining;
        Mine.OnCheckDepletion += SetDeposit;
        Mine.OnRemoveMine += RemoveMine;
    }

    private void OnDestroy()
    {
        GameApp.OnGameStart -= GameStart;
        Mine.OnSetMining -= SetMining;
        Mine.OnCheckDepletion -= SetDeposit;
        Mine.OnRemoveMine -= RemoveMine;
    }

    private void GameStart()
    {
        for (int i = 1; i < SaveManager.Save.MineDatas.Count; i++)
        {
            AddMIneItem(i);
        }
        int currentIndex = SaveManager.Save.CurrentMineIndex;
        if (currentIndex != 0)
            _mineItems[currentIndex].SetMining(true);
    }

    private void AddMIneItem(int mineIndex)
    {
        MineItem item = Instantiate(_mineItemPrefab, transform);
        MineData data = SaveManager.Save.MineDatas[mineIndex];
        MineLevelEntity mineLevelEntity = TableManager.MineLevelTable[data.MineLevelId];
        OreTypeEntity oreTypeEntity = TableManager.OreTypeTable[data.OreTypeId];
        OreGradeEntity oreGradeEntity = TableManager.OreGradeTable[data.OreGradeId];
        item.InitItem(mineIndex, data.OreDeposit, oreTypeEntity.Type, oreGradeEntity.Grade, mineLevelEntity.Level, null);
        _mineItems[mineIndex] = item;
    }

    private void SetMining(int mineIndex, bool isMining)
    {
        _mineItems[mineIndex].SetMining(isMining);
    }

    private void SetDeposit(int index, int deposit)
    {
        _mineItems[index].SetRemainText(deposit);
    }

    private void RemoveMine(int index)
    {
        Destroy(_mineItems[index].gameObject);
        _mineItems.Remove(index);
    }
}
