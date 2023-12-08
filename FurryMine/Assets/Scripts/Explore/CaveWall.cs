using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CaveWall : MonoBehaviour, IPointerClickHandler
{
    public static Action<Vector2Int> OnBreakCaveWall { get; set; }

    [SerializeField]
    private Image _renderer;
    [SerializeField]
    private GameObject _hitableObj;
    private int _wallHealth;
    private Vector2Int _wallPos;
    private bool _hitable;

    public void Init(int wallHealth, Vector2Int pos)
    {
        _hitable = false;
        _wallHealth = wallHealth;
        _wallPos = pos;
        _hitableObj.SetActive(false);
        SetHealthColor();
    }

    public void TrueHitable()
    {
        _hitableObj.SetActive(true);
        _hitable = true;
    }

    public void Hit(int Damage)
    {
        _wallHealth -= Damage;
        if (_wallHealth <= 0)
        {
            gameObject.SetActive(false);
            OnBreakCaveWall(_wallPos);
            return;
        }
        SetHealthColor();
    }

    private void SetHealthColor()
    {
        _renderer.color = new Color(_wallHealth * 0.1f, _wallHealth * 0.1f, _wallHealth * 0.1f, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_hitable)
        {
            Hit(1);
        }
    }
}
