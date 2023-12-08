using System;
using UnityEngine;
using UnityEngine.UI;

public class MineMapPanel : MonoBehaviour
{
    public static Action<MineData> OnClickExplore { get; set; }

    [SerializeField]
    private Button _exploreBtn;

    private void Awake()
    {
        _exploreBtn.onClick.AddListener(ClickExplore);
    }

    private void ClickExplore()
    {
        gameObject.SetActive(false);
        OnClickExplore(MineSignature.CurrentSignature.MineData);
    }
}
