using System.Collections.Generic;
using UnityEngine;

public enum EMinerLabel
{
    NONE,
    STAFF,
    HEAD
}

public class MinerContent : MonoBehaviour
{

    [SerializeField]
    private MinerItem _minerItemPrefab;

    private Dictionary<int, MinerItem> _minerItems = new Dictionary<int, MinerItem>();

    public MinerItem GetMinerItem(int id)
    {
        return _minerItems[id];
    }

    public void InitSelectItem(int minerId)
    {
        MinerItem.PrevItem = null;
        _minerItems[minerId].UnselectItem();
    }

    private void Awake()
    {
        GameApp.OnGameStart += GameStart;
        MinerTeam.OnSetLabel += SetLabel;
        MinerTeam.OnInitSelectMiner += InitSelectItem;
    }

    private void OnDestroy()
    {
        GameApp.OnGameStart -= GameStart;
        MinerTeam.OnSetLabel -= SetLabel;
        MinerTeam.OnInitSelectMiner -= InitSelectItem;
    }

    private void GameStart()
    {
        foreach (int id in SaveManager.Save.MinerIds)
        {
            AddMinerItem(id);
        }
        _minerItems[SaveManager.Save.CurrentHeadId].SetLabel(EMinerLabel.HEAD);
        foreach (int id in SaveManager.Save.CurrentStaffIds)
        {
            _minerItems[id].SetLabel(EMinerLabel.STAFF);
        }
    }

    private void AddMinerItem(int minerId)
    {
        MinerItem item = Instantiate(_minerItemPrefab, transform);
        MinerEntity entity = TableManager.MinerTable[minerId];
        item.InitItem(minerId, entity.Name, entity.Rank, ResourceManager.MinerIconList[minerId]);
        _minerItems[minerId] = item;
    }

    private void SetLabel(int minerId, EMinerLabel label)
    {
        _minerItems[minerId].SetLabel(label);
    }
}
