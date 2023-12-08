using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapPanel : MonoBehaviour
{
    private CaveGenerator _caveGenerator;

    private void Awake()
    {
        _caveGenerator = GetComponentInChildren<CaveGenerator>();
        MineMapPanel.OnClickExplore += GenerateCave;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MineMapPanel.OnClickExplore -= GenerateCave;
    }

    private void GenerateCave(MineData data)
    {
        _caveGenerator.GenerateMap(data.MineLevelId);
        gameObject.SetActive(true);
    }
}
