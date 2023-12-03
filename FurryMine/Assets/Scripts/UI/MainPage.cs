using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    [SerializeField]
    private Transform _lightFrame;

    [SerializeField]
    private Button _headMenu;
    [SerializeField]
    private Button _staffMenu;
    [SerializeField]
    private Button _mineMenu;
    [SerializeField]
    private Button _snackMenu;

    [SerializeField]
    private ScrollRect _scroll;
    [SerializeField]
    private RectTransform _headContent;
    [SerializeField]
    private RectTransform _staffContent;
    [SerializeField]
    private RectTransform _mineContent;
    [SerializeField]
    private RectTransform _snackContent;

    private RectTransform _prevContent;

    private void Awake()
    {
        _headMenu.onClick.AddListener(ShowHeadMenu);
        _staffMenu.onClick.AddListener(ShowStaffMenu);
        _mineMenu.onClick.AddListener(ShowMineMenu);
        _snackMenu.onClick.AddListener(ShowSnackMenu);


        _prevContent = _headContent;
        _staffContent.gameObject.SetActive(false);
        _mineContent.gameObject.SetActive(false);
        _mineContent.gameObject.SetActive(false);
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

    private void ShowMineMenu()
    {
        _scroll.content = _mineContent;
        _prevContent.gameObject.SetActive(false);
        _mineContent.gameObject.SetActive(true);
        _prevContent = _mineContent;
        MoveLightFrame(_mineMenu.transform.position.x);
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
