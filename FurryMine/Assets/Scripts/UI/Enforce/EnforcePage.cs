using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnforcePage : MonoBehaviour
{
    [SerializeField]
    private Transform _lightFrame;

    [SerializeField]
    private Button _headMenu;
    [SerializeField]
    private Button _staffMenu;
    [SerializeField]
    private Button _miningMenu;
    [SerializeField]
    private Button _snackMenu;

    [SerializeField]
    private ScrollRect _scroll;
    [SerializeField]
    private RectTransform _headContent;
    [SerializeField]
    private RectTransform _staffContent;
    [SerializeField]
    private RectTransform _miningContent;
    [SerializeField]
    private RectTransform _snackContent;

    private RectTransform _prevContent;

    private void Awake()
    {
        _headMenu.onClick.AddListener(ShowHeadMenu);
        _staffMenu.onClick.AddListener(ShowStaffMenu);
        _miningMenu.onClick.AddListener(ShowMiningMenu);
        _snackMenu.onClick.AddListener(ShowSnackMenu);
    }

    private void Start()
    {
        _prevContent = _headContent;
        _staffContent.gameObject.SetActive(false);
        _miningContent.gameObject.SetActive(false);
        _snackContent.gameObject.SetActive(false);
    }

    private void ShowHeadMenu()
    {
        _scroll.content = _headContent;
        _prevContent.gameObject.SetActive(false);
        _headContent.gameObject.SetActive(true);
        _prevContent = _headContent;
        MoveLightFrame(_headMenu.transform.position.x);
    }

    private void ShowStaffMenu()
    {
        _scroll.content = _staffContent;
        _prevContent.gameObject.SetActive(false);
        _staffContent.gameObject.SetActive(true);
        _prevContent = _staffContent;
        MoveLightFrame(_staffMenu.transform.position.x);
    }

    private void ShowMiningMenu()
    {
        _scroll.content = _miningContent;
        _prevContent.gameObject.SetActive(false);
        _miningContent.gameObject.SetActive(true);
        _prevContent = _miningContent;
        MoveLightFrame(_miningMenu.transform.position.x);
    }

    private void ShowSnackMenu()
    {
        _scroll.content = _snackContent;
        _prevContent.gameObject.SetActive(false);
        _snackContent.gameObject.SetActive(true);
        _prevContent = _snackContent;
        MoveLightFrame(_snackMenu.transform.position.x);
    }

    private void MoveLightFrame(float xPos)
    {
        Vector3 pos = _lightFrame.position;
        pos.x = xPos;
        _lightFrame.position = pos;
    }
}
