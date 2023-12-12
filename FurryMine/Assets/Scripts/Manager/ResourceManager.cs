using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

public static class ResourceManager
{
    public static Action OnComplete { get; set; }

    private static readonly string _animCtrlLabel = "AnimCtrl";
    private static readonly string _minerIconLabel = "MinerIcon";

    private static AsyncOperationHandle<IList<Object>> _animCtrlHandle;
    private static AsyncOperationHandle<IList<Object>> _minerIconHandle;
    public static List<RuntimeAnimatorController> AnimCtrlList { get; private set; } = new List<RuntimeAnimatorController>();

    public static List<Sprite> MinerIconList { get; private set; } = new List<Sprite>();

    public static List<Sprite> EquipIconList { get; private set; } = new List<Sprite>();

    public static void LoadResource()
    {
        GameApp.PlusLoadingCount(2);
        Addressables.LoadAssetsAsync(_animCtrlLabel, (Object obj) =>
        {
            AnimCtrlList.Add(obj as RuntimeAnimatorController);
        }).Completed += (table) =>
        {
            _animCtrlHandle = table;
            //Debug.Log("AnimCtrl Load");
            OnComplete();
        };

        Addressables.LoadResourceLocationsAsync(_minerIconLabel, typeof(Texture2D)).Completed += (handle) =>
        {
            Addressables.LoadAssetsAsync(handle.Result, (Object obj) =>
            {
                Texture2D texture = obj as Texture2D;
                MinerIconList.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
            }).Completed += (table) =>
            {
                _minerIconHandle = table;
                //Debug.Log("MinerIcon Load");
                OnComplete();
            };
        };

        var equipIcons = Resources.LoadAll<Texture2D>("EquipIcon");
        foreach(var texture in equipIcons)
        {
            EquipIconList.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
        }
    }

    public static void UnloadResource()
    {
        Addressables.Release(_animCtrlHandle);
        Addressables.Release(_minerIconHandle);
    }
}
