using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPage : MonoBehaviour
{
    [SerializeField]
    private Button _supportBtn;

    private void Start()
    {
        gameObject.SetActive(false);
        _supportBtn.onClick.AddListener(ClickSupport);
    }

    private void ClickSupport()
    {
        Application.OpenURL("https://www.patreon.com/user?u=35286999");
    }
}
