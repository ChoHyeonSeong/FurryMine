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

    private Button _staffMenu;
    private Button _mineMenu;
    private Button _snackMenu;

    private GameObject _headScroll;
    private GameObject _staffScroll;
    private GameObject _mineScroll;
    private GameObject _snackScroll;

    private GameObject _prevScroll;

    private void Awake()
    {
        _headMenu.onClick.AddListener(ShowHeadMenu);
/*        _staffMenu.onClick.AddListener(ShowStaffMenu);
        _mineMenu.onClick.AddListener(ShowMineMenu);
        _snackMenu.onClick.AddListener(ShowSnackMenu);*/

        _headScroll = _headMenu.GetComponentInChildren<ScrollRect>().gameObject;
/*        _staffScroll = _staffMenu.GetComponentInChildren<ScrollRect>().gameObject;
        _mineScroll = _mineMenu.GetComponentInChildren<ScrollRect>().gameObject;
        _snackScroll = _snackMenu.GetComponentInChildren<ScrollRect>().gameObject;*/

        _prevScroll = _headScroll;
/*        _staffScroll.SetActive(false);
        _mineScroll.SetActive(false);
        _snackScroll.SetActive(false);*/
    }

    private void ShowHeadMenu()
    {
        _prevScroll.SetActive(false);
        _headScroll.SetActive(true);
        _prevScroll = _headScroll;
        MoveLightFrame(_headMenu.transform.position.x);
    }

    private void ShowStaffMenu()
    {
        _prevScroll.SetActive(false);
        _staffScroll.SetActive(true);
        _prevScroll = _staffScroll;
        MoveLightFrame(_staffMenu.transform.position.x);
    }

    private void ShowMineMenu()
    {
        _prevScroll.SetActive(false);
        _mineScroll.SetActive(true);
        _prevScroll = _mineScroll;
        MoveLightFrame(_mineMenu.transform.position.x);
    }

    private void ShowSnackMenu()
    {
        _prevScroll.SetActive(false);
        _snackScroll.SetActive(true);
        _prevScroll = _snackScroll;
        MoveLightFrame(_snackMenu.transform.position.x);
    }

    private void MoveLightFrame(float xPos)
    {
        Vector3 pos = _lightFrame.position;
        pos.x = xPos;
        _lightFrame.position = pos;
    }
}
