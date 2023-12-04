using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelStatus : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelText;

    private void Awake()
    {
        _levelText.text = "1";
    }
    private void OnEnable()
    {
        Owner.OnSetOwnerLevel += UpdateText;
        GameApp.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        Owner.OnSetOwnerLevel -= UpdateText;
        GameApp.OnGameStart -= GameStart;
    }

    private void GameStart()
    {
        _levelText.text = SaveManager.Save.OwnerLevel.ToString();
    }


    private void UpdateText(int level)
    {
        _levelText.text = level.ToString();
    }
}
