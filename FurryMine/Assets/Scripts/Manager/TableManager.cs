using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class TableManager
{
    public static Action OnComplete { get; set; }
    public static Dictionary<int, MinerEntity> MinerTable { get; private set; } = new Dictionary<int, MinerEntity>();
    public static Dictionary<int, EnforceEntity> EnforceTable { get; private set; } = new Dictionary<int, EnforceEntity>();

    public static Dictionary<int, EquipEntity> EquipTable { get; private set; } = new Dictionary<int, EquipEntity>();

    private static AssetReferenceT<MinerTable> _minerRef;
    private static AssetReferenceT<EnforceTable> _enforceRef;
    private static AssetReferenceT<EquipTable> _equipRef;

    public static void LoadTable()
    {
        GameApp.PlusLoadingCount(3);
        _minerRef = new AssetReferenceT<MinerTable>("Assets/Datas/MinerTable.asset");

        _minerRef.LoadAssetAsync().Completed += (table) =>
        {
            var minerData = table.Result;
            foreach (var entity in minerData.Table)
            {
                MinerTable[entity.Id] = entity;
            }
            Debug.Log("MinerTable Load");
            OnComplete();
        };


        _enforceRef = new AssetReferenceT<EnforceTable>("Assets/Datas/EnforceTable.asset");

        _enforceRef.LoadAssetAsync().Completed += (table) =>
        {
            var enforceData = table.Result;
            foreach (var entity in enforceData.Table)
            {
                EnforceTable[entity.Id] = entity;
            }
            Debug.Log("EnforceTable Load");
            OnComplete();
        };

        _equipRef = new AssetReferenceT<EquipTable>("Assets/Datas/EquipTable.asset");

        _equipRef.LoadAssetAsync().Completed += (table) =>
        {
            var equipData = table.Result;
            foreach (var entity in equipData.Table)
            {
                EquipTable[entity.Id] = entity;
            }
            Debug.Log("EquipTable Load");
            OnComplete();
        };
    }

    public static void UnloadTable()
    {
        _minerRef.ReleaseAsset();
        _enforceRef.ReleaseAsset();
        _equipRef.ReleaseAsset();
    }
}
