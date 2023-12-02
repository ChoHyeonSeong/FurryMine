using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public static class ResourceManager
{
    public static Action OnComplete { get; set; }

    private static readonly string _animCtrlLabel = "AnimCtrl";

    private static AsyncOperationHandle<IList<Object>> _animCtrlHandle;
    public static List<RuntimeAnimatorController> AnimCtrlList { get; private set; } = new List<RuntimeAnimatorController>();

    public static void LoadResource()
    {
        Addressables.LoadAssetsAsync(_animCtrlLabel, (Object obj) =>
        {
            AnimCtrlList.Add(obj as RuntimeAnimatorController);
        }).Completed += (table) =>
        {
            _animCtrlHandle = table;
            Debug.Log("Resource Load");
            OnComplete();
        };

    }

    public static void UnloadResource()
    {
        Addressables.Release(_animCtrlHandle);
    }
}
