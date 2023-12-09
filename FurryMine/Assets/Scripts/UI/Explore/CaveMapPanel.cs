using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CaveMapPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _oreDepositText;
    [SerializeField]
    private TextMeshProUGUI _lodeCountText;

    public static Action OnGenerateCave { get; set; }

    private void Awake()
    {
        MineMapPanel.OnClickExplore += GenerateCave;
        Cave.OnUpdateExploreBoard += UpdateBoard;
        ExplorePage.OnEndExplore += EndExploreCave;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MineMapPanel.OnClickExplore -= GenerateCave;
        Cave.OnUpdateExploreBoard -= UpdateBoard;
        ExplorePage.OnEndExplore -= EndExploreCave;
    }

    private void GenerateCave()
    {
        OnGenerateCave();
        gameObject.SetActive(true);
    }

    private void UpdateBoard(int oreDeposit, int crtLodeCount, int lodeCount)
    {
        _oreDepositText.text = $"±§ºÆ ∏≈¿Â∑Æ : {oreDeposit}";
        _lodeCountText.text = $"πﬂ∞ﬂ«— ±§∏∆ : {crtLodeCount}/{lodeCount}";
    }

    private void EndExploreCave()
    {
        gameObject.SetActive(false);
    }
}
