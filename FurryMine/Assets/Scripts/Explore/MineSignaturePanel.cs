using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineSignaturePanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _mineLevelText;
    [SerializeField]
    private TextMeshProUGUI _oreTypeText;
    [SerializeField]
    private TextMeshProUGUI _oreGradeText;
    [SerializeField]
    private Button _exploreBtn;

    private void Awake()
    {
        MineSignature.OnEnterSignature += ShowPanel;
        MineMapDisplay.OnExitSignature += HIdePanel;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MineSignature.OnEnterSignature -= ShowPanel;
        MineMapDisplay.OnExitSignature -= HIdePanel;
    }

    private void ShowPanel(string grade, string type, int level, Vector3 pos)
    {
        pos.y += 40;
        transform.position = pos;
        _oreGradeText.text = $"{grade}±Þ";
        _oreTypeText.text = $"{type} ±¤»ê";
        _mineLevelText.text = $"Lv. {level}";
        _exploreBtn.interactable = true;
        gameObject.SetActive(true);
    }

    private void HIdePanel()
    {
        _exploreBtn.interactable = false;
        gameObject.SetActive(false);
    }
}
