using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CaveWall : MonoBehaviour, IPointerClickHandler
{
    public static Action OnHitCaveWall { get; set; }
    public static Action<CaveWall> OnBreakCaveWall { get; set; }

    public Vector2Int WallPos { get => _wallPos; }

    [SerializeField]
    private Image _renderer;
    [SerializeField]
    private TextMeshProUGUI _hpText;
    private int _wallHealth;
    private Vector2Int _wallPos;
    private bool _hitable;
    private Lode _lode;

    public void Init(int wallHealth, Vector2Int pos)
    {
        _hitable = false;
        _wallHealth = wallHealth;
        _wallPos = pos;
        _lode = null;
        _hpText.gameObject.SetActive(false);
        SetHealthText();
    }

    public void SetLode(Lode lode)
    {
        _lode = lode;
    }


    public void TrueHitable()
    {
        _hpText.gameObject.SetActive(true);
        _hitable = true;
    }

    public void Hit(int Damage)
    {
        _wallHealth -= Damage;
        if (_wallHealth <= 0)
        {
            if (_lode != null)
                _lode.DiscoverLode();
            gameObject.SetActive(false);
            OnBreakCaveWall(this);
        }
        SetHealthText();
        OnHitCaveWall();
    }

    private void SetHealthText()
    {
        _hpText.text = _wallHealth.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_hitable)
        {
            Hit(1);
        }
    }
}
