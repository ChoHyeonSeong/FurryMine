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
    public static Dictionary<int, MineLevelEntity> MineLevelTable { get; private set; } = new Dictionary<int, MineLevelEntity>();
    public static Dictionary<int, OreGradeEntity> OreGradeTable { get; private set; } = new Dictionary<int, OreGradeEntity>();
    public static Dictionary<int, OreTypeEntity> OreTypeTable { get; private set; } = new Dictionary<int, OreTypeEntity>();

    private static AssetReferenceT<MinerTable> _minerRef;
    private static AssetReferenceT<EnforceTable> _enforceRef;
    private static AssetReferenceT<EquipTable> _equipRef;
    private static AssetReferenceT<MineLevelTable> _mineLevelRef;
    private static AssetReferenceT<OreGradeTable> _oreGradeRef;
    private static AssetReferenceT<OreTypeTable> _oreTypeRef;

    public static void LoadTable()
    {
        GameApp.PlusLoadingCount(6);

        _minerRef = new AssetReferenceT<MinerTable>("Assets/Datas/MinerTable.asset");
        _minerRef.LoadAssetAsync().Completed += (table) =>
        {
            var minerData = table.Result;
            foreach (var entity in minerData.Table)
                MinerTable[entity.Id] = entity;
            Debug.Log("MinerTable Load");
            OnComplete();
        };

        _enforceRef = new AssetReferenceT<EnforceTable>("Assets/Datas/EnforceTable.asset");
        _enforceRef.LoadAssetAsync().Completed += (table) =>
        {
            var enforceData = table.Result;
            foreach (var entity in enforceData.Table)
                EnforceTable[entity.Id] = entity;
            Debug.Log("EnforceTable Load");
            OnComplete();
        };

        _equipRef = new AssetReferenceT<EquipTable>("Assets/Datas/EquipTable.asset");
        _equipRef.LoadAssetAsync().Completed += (table) =>
        {
            var equipData = table.Result;
            foreach (var entity in equipData.Table)
                EquipTable[entity.Id] = entity;
            Debug.Log("EquipTable Load");
            OnComplete();
        };

        _mineLevelRef = new AssetReferenceT<MineLevelTable>("Assets/Datas/MineLevelTable.asset");
        _mineLevelRef.LoadAssetAsync().Completed += (table) =>
        {
            var mineLevelData = table.Result;
            foreach (var entity in mineLevelData.Table)
                MineLevelTable[entity.Id] = entity;
            Debug.Log("MineLevelTable Load");
            OnComplete();
        };

        _oreGradeRef = new AssetReferenceT<OreGradeTable>("Assets/Datas/OreGradeTable.asset");
        _oreGradeRef.LoadAssetAsync().Completed += (table) =>
        {
            var oreGradeData = table.Result;
            foreach (var entity in oreGradeData.Table)
                OreGradeTable[entity.Id] = entity;
            Debug.Log("OreGradeTable Load");
            OnComplete();
        };

        _oreTypeRef = new AssetReferenceT<OreTypeTable>("Assets/Datas/OreTypeTable.asset");
        _oreTypeRef.LoadAssetAsync().Completed += (table) =>
        {
            var oreTypeData = table.Result;
            foreach (var entity in oreTypeData.Table)
                OreTypeTable[entity.Id] = entity;
            Debug.Log("OreTypeTable Load");
            OnComplete();
        };
    }

    public static void TestLoad()
    {
        _minerRef = new AssetReferenceT<MinerTable>("Assets/Datas/MinerTable.asset");
        _minerRef.LoadAssetAsync().Completed += (table) =>
        {
            var minerData = table.Result;
            foreach (var entity in minerData.Table)
                MinerTable[entity.Id] = entity;
            Debug.Log("MinerTable Load");
            OnComplete();
        };

        _enforceRef = new AssetReferenceT<EnforceTable>("Assets/Datas/EnforceTable.asset");
        _enforceRef.LoadAssetAsync().Completed += (table) =>
        {
            var enforceData = table.Result;
            foreach (var entity in enforceData.Table)
                EnforceTable[entity.Id] = entity;
            Debug.Log("EnforceTable Load");
            OnComplete();
        };

        _equipRef = new AssetReferenceT<EquipTable>("Assets/Datas/EquipTable.asset");
        _equipRef.LoadAssetAsync().Completed += (table) =>
        {
            var equipData = table.Result;
            foreach (var entity in equipData.Table)
                EquipTable[entity.Id] = entity;
            Debug.Log("EquipTable Load");
            OnComplete();
        };

        _mineLevelRef = new AssetReferenceT<MineLevelTable>("Assets/Datas/MineLevelTable.asset");
        _mineLevelRef.LoadAssetAsync().Completed += (table) =>
        {
            var mineLevelData = table.Result;
            foreach (var entity in mineLevelData.Table)
                MineLevelTable[entity.Id] = entity;
            Debug.Log("MineLevelTable Load");
            OnComplete();
        };

        _oreGradeRef = new AssetReferenceT<OreGradeTable>("Assets/Datas/OreGradeTable.asset");
        _oreGradeRef.LoadAssetAsync().Completed += (table) =>
        {
            var oreGradeData = table.Result;
            foreach (var entity in oreGradeData.Table)
                OreGradeTable[entity.Id] = entity;
            Debug.Log("OreGradeTable Load");
            OnComplete();
        };

        _oreTypeRef = new AssetReferenceT<OreTypeTable>("Assets/Datas/OreTypeTable.asset");
        _oreTypeRef.LoadAssetAsync().Completed += (table) =>
        {
            var oreTypeData = table.Result;
            foreach (var entity in oreTypeData.Table)
                OreTypeTable[entity.Id] = entity;
            Debug.Log("OreTypeTable Load");
            OnComplete();
        };
    }

    public static void UnloadTable()
    {
        _minerRef.ReleaseAsset();
        _enforceRef.ReleaseAsset();
        _equipRef.ReleaseAsset();
        _mineLevelRef.ReleaseAsset();
        _oreGradeRef.ReleaseAsset();
        _oreTypeRef.ReleaseAsset();
    }
}