using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaveMapPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _oreDepositText;
    [SerializeField]
    private TextMeshProUGUI _lodeCountText;
    [SerializeField]
    private Slider _miningHealthBar;
    [SerializeField]
    private TextMeshProUGUI _miningHealthText;

    [SerializeField]
    private GameObject _failPanel;
    [SerializeField]
    private GameObject _discoverPanel;

    public static Action OnGenerateCave { get; set; }

    private void Awake()
    {
        MineMapPanel.OnClickExplore += GenerateCave;
        Cave.OnUpdateExploreBoard += UpdateBoard;
        Cave.OnUpdateMiningHealth += SetMiningHealthBar;
        Cave.OnDiscoverMine += DiscoverMine;
        Cave.OnFailDiscover += FailDiscover;
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
        Cave.OnUpdateMiningHealth -= SetMiningHealthBar;
        Cave.OnDiscoverMine -= DiscoverMine;
        Cave.OnFailDiscover -= FailDiscover;
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
        _failPanel.SetActive(false);
        _discoverPanel.SetActive(false);
    }

    private void DiscoverMine(MineData _)
    {
        _discoverPanel.SetActive(true);
    }

    private void FailDiscover()
    {
        _failPanel.SetActive(true);
    }

    private void SetMiningHealthBar(int crtMiningHealth, int miningHealth)
    {
        _miningHealthBar.value = crtMiningHealth / (float)miningHealth;
        _miningHealthText.text = $"{crtMiningHealth}/{miningHealth}";
    }
}
