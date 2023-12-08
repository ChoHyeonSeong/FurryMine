using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MineMapDisplay : MonoBehaviour, IPointerClickHandler
{
    public static Action OnExitSignature { get; set; }

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void DrawTexture(Texture2D texture)
    {
        _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //_image.transform.localScale = new Vector3(texture.width, texture.height, 1);
    }

    public void SetScale(float scale)
    {
        _image.transform.localScale = new Vector3(scale, scale, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MineSignature.SetNullCurrentSignature();
        OnExitSignature();
    }
}