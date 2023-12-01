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
    public static Dictionary<int, MineEntity> MineTable { get; private set; } = new Dictionary<int, MineEntity>();
    public static Dictionary<int, EnforceEntity> EnforceTable { get; private set; } = new Dictionary<int, EnforceEntity>();

    private static AssetReferenceT<MinerTable> _minerRef;
    private static AssetReferenceT<MineTable> _mineRef;
    private static AssetReferenceT<EnforceTable> _enforceRef;

    public static void LoadTable()
    {
        GameApp.AddLoading(3);
        _minerRef = new AssetReferenceT<MinerTable>("Assets/Datas/MinerTable.asset");

        _minerRef.LoadAssetAsync().Completed += (table) =>
        {
            var minerData = table.Result;
            foreach (var entity in minerData.Table)
            {
                MinerTable[entity.Id] = entity;
            }
            OnComplete();
        };

        _mineRef = new AssetReferenceT<MineTable>("Assets/Datas/MineTable.asset");

        _mineRef.LoadAssetAsync().Completed += (table) =>
        {
            var mineData = table.Result;
            foreach (var entity in mineData.Table)
            {
                MineTable[entity.Level] = entity;
            }
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
            OnComplete();
        };
    }

    public static void UnloadTable()
    {
        _mineRef.ReleaseAsset();
        _enforceRef.ReleaseAsset();
    }
}
