using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PageButton : MonoBehaviour
{
    [SerializeField]
    private Button _enforcePageButton;
    [SerializeField]
    private Button _managePageButton;
    [SerializeField]
    private Button _explorePageButton;
    [SerializeField]
    private Button _shopPageButton;

    [SerializeField]
    private GameObject _enforcePage;
    [SerializeField]
    private GameObject _managePage;
    [SerializeField]
    private GameObject _explorePage;
    [SerializeField]
    private GameObject _shopPage;

    private GameObject _prevPage;

    private void Awake()
    {
        _enforcePageButton.onClick.AddListener(ShowEnforcePage);
        _managePageButton.onClick.AddListener(ShowManagePage);
        _explorePageButton.onClick.AddListener(ShowExplorePage);
        _prevPage = _enforcePage;
    }

    private void ShowEnforcePage()
    {
        if(_enforcePage != _prevPage)
        {
            _prevPage.SetActive(false);
            _enforcePage.SetActive(true);
            _prevPage = _enforcePage;
        }
    }

    private void ShowManagePage()
    {
        if (_managePage != _prevPage)
        {
            _prevPage.SetActive(false);
            _managePage.SetActive(true);
            _prevPage = _managePage;
        }
    }
    private void ShowExplorePage()
    {
        if (_explorePage != _prevPage)
        {
            _explorePage.SetActive(true);
        }
    }
}
