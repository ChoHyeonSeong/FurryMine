using System;
using UnityEngine;
using UnityEngine.UI;

public class MineMapPanel : MonoBehaviour
{
    public static Action OnClickExplore { get; set; }

    [SerializeField]
    private Button _exploreBtn;

    private void Awake()
    {
        _exploreBtn.onClick.AddListener(ClickExplore);
        ExplorePage.OnEndExplore += ActivePanel;
    }

    private void OnDestroy()
    {
        ExplorePage.OnEndExplore -= ActivePanel;
    }

    private void ClickExplore()
    {
        gameObject.SetActive(false);
        OnClickExplore();
    }

    private void ActivePanel()
    {
        gameObject.SetActive(true);
    }
}
