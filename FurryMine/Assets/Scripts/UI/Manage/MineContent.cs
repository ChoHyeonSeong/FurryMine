using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineContent : MonoBehaviour
{
    [SerializeField]
    private MineItem _mineItemPrefab;

    private List<MineItem> _mineItems = new List<MineItem>();

    private void Awake()
    {
        GameApp.OnGameStart += GameStart;
        Mine.OnSetMining += SetMining;
        Mine.OnCheckDepletion += SetDeposit;
        Mine.OnAddMine += AddMIneItem;
        Mine.OnRemoveMine += RemoveMine;
    }

    private void OnDestroy()
    {
        GameApp.OnGameStart -= GameStart;
        Mine.OnSetMining -= SetMining;
        Mine.OnCheckDepletion -= SetDeposit;
        Mine.OnAddMine -= AddMIneItem;
        Mine.OnRemoveMine -= RemoveMine;
    }

    private void GameStart()
    {
        Debug.Log("MineContent-GameStart Begin");
        _mineItems.Add(null);
        for (int i = 1; i < SaveManager.Save.MineDatas.Count; i++)
        {
            AddMIneItem(SaveManager.Save.MineDatas[i]);
        }
        int currentIndex = SaveManager.Save.CurrentMineIndex;
        if (currentIndex != 0)
            _mineItems[currentIndex].SetMining(true);
        Debug.Log("MineContent-GameStart End");
    }

    private void AddMIneItem(MineData data)
    {
        MineItem item = Instantiate(_mineItemPrefab, transform);
        MineLevelEntity mineLevelEntity = TableManager.MineLevelTable[data.MineLevelId];
        OreTypeEntity oreTypeEntity = TableManager.OreTypeTable[data.OreTypeId];
        OreGradeEntity oreGradeEntity = TableManager.OreGradeTable[data.OreGradeId];
        item.InitItem(_mineItems.Count, data.OreDeposit, oreTypeEntity.Type, oreGradeEntity.Grade, mineLevelEntity.Level, null);
        _mineItems.Add(item);
    }

    private void SetMining(int mineIndex, bool isMining)
    {
        if (mineIndex == 0)
            return;
        _mineItems[mineIndex].SetMining(isMining);
    }

    private void SetDeposit(int mineIndex, int deposit)
    {
        if (mineIndex == 0)
            return;
        _mineItems[mineIndex].SetRemainText(deposit);
    }

    private void RemoveMine(int mineIndex)
    {
        if (mineIndex == 0)
            return;
        Destroy(_mineItems[mineIndex].gameObject);
        _mineItems.RemoveAt(mineIndex);
        for (int i = 1; i < _mineItems.Count; i++)
        {
            _mineItems[i].SetIndex(i);
        }
    }
}
