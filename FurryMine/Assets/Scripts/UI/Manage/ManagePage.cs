using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;

public class ManagePage : MonoBehaviour
{
    [SerializeField]
    private Transform _lightFrame;

    [SerializeField]
    private Button _minerMenu;
    [SerializeField]
    private Button _equipMenu;
    private Button _mineMenu;
    private Button _bagMenu;

    [SerializeField]
    private ScrollRect _scroll;
    [SerializeField]
    private RectTransform _minerContent;
    [SerializeField]
    private RectTransform _equipContent;
    private RectTransform _mineContent;
    private RectTransform _bagContent;

    private RectTransform _prevContent;

    private void Awake()
    {
        _minerMenu.onClick.AddListener(ShowMinerMenu);
        _equipMenu.onClick.AddListener(ShowEquipMenu);
        //_mineMenu.onClick.AddListener(ShowMineMenu);
        //_bagMenu.onClick.AddListener(ShowBagMenu);


        _prevContent = _minerContent;
        _equipContent.gameObject.SetActive(false);
        //_mineContent.gameObject.SetActive(false);
        //_bagContent.gameObject.SetActive(false);
    }

    private void ShowMinerMenu()
    {
        _scroll.content = _minerContent;
        _prevContent.gameObject.SetActive(false);
        _minerContent.gameObject.SetActive(true);
        _prevContent = _minerContent;
        MoveLightFrame(_minerMenu.transform.position.x);
    }

    private void ShowEquipMenu()
    {
        _scroll.content = _equipContent;
        _prevContent.gameObject.SetActive(false);
        _equipContent.gameObject.SetActive(true);
        _prevContent = _equipContent;
        MoveLightFrame(_equipMenu.transform.position.x);
    }

    private void ShowMineMenu()
    {
        _scroll.content = _mineContent;
        _prevContent.gameObject.SetActive(false);
        _mineContent.gameObject.SetActive(true);
        _prevContent = _mineContent;
        MoveLightFrame(_mineMenu.transform.position.x);
    }

    private void ShowBagMenu()
    {
        _scroll.content = _bagContent;
        _prevContent.gameObject.SetActive(false);
        _bagContent.gameObject.SetActive(true);
        _prevContent = _bagContent;
        MoveLightFrame(_bagMenu.transform.position.x);
    }

    private void MoveLightFrame(float xPos)
    {
        Vector3 pos = _lightFrame.position;
        pos.x = xPos;
        _lightFrame.position = pos;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
}
