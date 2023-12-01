using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public static class ResourceManager
{
    public static Action OnComplete { get; set; }

    private static readonly string _animCtrlLabel = "AnimCtrl";
    public static List<RuntimeAnimatorController> AnimCtrlList { get; private set; } = new List<RuntimeAnimatorController>();

    public static void LoadResource()
    {
        GameApp.AddLoading(1);
        Addressables.LoadAssetsAsync(_animCtrlLabel, (Object obj) =>
        {
            AnimCtrlList.Add(obj as RuntimeAnimatorController);
            OnComplete();
        });

    }

    public static void UnloadResource()
    {
        foreach (var animator in AnimCtrlList)
        {
            Addressables.Release(animator);
        }
    }
}
