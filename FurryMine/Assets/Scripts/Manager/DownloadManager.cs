using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class DownloadManager : MonoBehaviour
{
    public static Action OnHaveStartLoading { get; set; }
    public static Action OnHaveDownload { get; set; }
    public static Action OnHaveNotDownload { get; set; }
    public static Action<float> OnUpdateDownload { get; set; }
    public static Action<float> OnUpdateStartLoading { get; set; }

    [SerializeField]
    private AssetLabelReference _tableLabel;
    [SerializeField]
    private AssetLabelReference _animCtrlLabel;
    [SerializeField]
    private AssetLabelReference _minerIconLabel;

    private static string _gameScene = "Game";
    private long _patchSize;
    private float _loading;
    private Dictionary<string, long> _patchMap = new Dictionary<string, long>();


    private void Start()
    {
        LoadPatch();
    }

    public void LoadPatch()
    {
        StartCoroutine(InitAddressable());
        StartCoroutine(CheckUpdateFiles());
    }

    private IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    private IEnumerator CheckUpdateFiles()
    {
        var labels = new List<string> { _tableLabel.labelString, _animCtrlLabel.labelString, _minerIconLabel.labelString };
        _patchSize = default;

        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            yield return handle;
            _patchSize += handle.Result;
        }

        if (_patchSize > decimal.Zero)
        {
            OnHaveDownload();
            StartCoroutine(PatchFiles());
        }
        else
        {   
            OnHaveNotDownload();
            yield return new WaitForSeconds(2f);
            StartCoroutine(StartLoading());
            // æ¿¿¸»Ø
        }
    }

    private IEnumerator PatchFiles()
    {
        var labels = new List<string> { _tableLabel.labelString, _animCtrlLabel.labelString, _minerIconLabel.labelString };

        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            yield return handle;
            if (handle.Result != decimal.Zero)
            {
                StartCoroutine(DownloadLabel(label));
            }
        }

        yield return UpdateDownload();
    }

    private IEnumerator DownloadLabel(string label)
    {
        _patchMap.Add(label, 0);

        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            _patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame();
        }

        _patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }

    private IEnumerator UpdateDownload()
    {
        var total = 0f;
        OnUpdateDownload(total);

        while (true)
        {
            total += _patchMap.Sum(tmp => tmp.Value);
            OnUpdateDownload(total / _patchSize);

            if (total == _patchSize)
            {
                // æ¿¿¸»Ø
                StartCoroutine(StartLoading());
                break;
            }

            total = 0f;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator StartLoading()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(_gameScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        _loading = 0f;
        OnHaveStartLoading();
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                _loading = Mathf.Lerp(_loading, op.progress, timer);
                OnUpdateStartLoading(_loading);
                if (_loading >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                _loading = Mathf.Lerp(_loading, 1f, timer);
                OnUpdateStartLoading(_loading);
                if (_loading == 1f)
                {
                    yield return new WaitForSeconds(2f);
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}