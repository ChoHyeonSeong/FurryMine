using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMinerContent : MonoBehaviour
{
    [SerializeField]
    private SelectMinerItem _selectMinerItemPrefab;

    private Queue<SelectMinerItem> _showQueue;
    private Queue<SelectMinerItem> _hideQueue;

    private void Awake()
    {
        _showQueue = new Queue<SelectMinerItem>();
        _hideQueue = new Queue<SelectMinerItem>();
    }

    public void PoolContent()
    {
        while (_showQueue.Count > 0)
            _hideQueue.Enqueue(_showQueue.Dequeue());
    }

    public void InitContent(bool includeHead)
    {
        if (includeHead)
        {
            CreateItem(GameManager.Team.HeadMinerId);
        }

        // staff �޾ƿ���
        List<int> staffIdList = GameManager.Team.GetCurrentStaffIdList();
        foreach (int staffId in staffIdList)
        {
            CreateItem(staffId);
        }
    }

    private void CreateItem(int minerId)
    {
        SelectMinerItem item;
        MinerEntity entity = TableManager.MinerTable[minerId];
        if (_hideQueue.Count > 0)
        {
            item = _hideQueue.Dequeue();
            item.gameObject.SetActive(true);
        }
        else
        {
            item = Instantiate(_selectMinerItemPrefab, transform);
        }
        item.InitItem(minerId, entity.Name, entity.Rank, ResourceManager.MinerIconList[minerId]);
        _showQueue.Enqueue(item);
    }
}
