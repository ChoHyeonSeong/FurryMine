using System.Collections.Generic;
using UnityEngine;

public class EquipContent : MonoBehaviour
{
    [SerializeField]
    private EquipItem _equipItemPrefab;

    private Dictionary<int, EquipItem> _equipItems = new Dictionary<int, EquipItem>();

    private void Awake()
    {
        GameApp.OnGameStart += GameStart;
        MinerTeam.OnSetWear += SetWear;
    }

    private void OnDestroy()
    {
        GameApp.OnGameStart -= GameStart;
        MinerTeam.OnSetWear -= SetWear;
    }

    private void GameStart()
    {
        foreach (int id in SaveManager.Save.EquipIds)
        {
            AddEquipItem(id);
        }

        foreach(MinerEquip minerEquip in SaveManager.Save.CurrentMinerEquip)
        {
            _equipItems[minerEquip.EquipId].SetWear(true);
        }
    }

    private void AddEquipItem(int equipId)
    {
        EquipItem item = Instantiate(_equipItemPrefab, transform);
        EquipEntity entity = TableManager.EquipTable[equipId];
        item.InitItem(equipId, entity.Name, entity.Rank, ResourceManager.EquipIconList[equipId]);
        _equipItems[equipId] = item;
    }

    private void SetWear(int equipId, bool isWear)
    {
        _equipItems[equipId].SetWear(isWear);
    }
}
