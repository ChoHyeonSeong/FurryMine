using UnityEngine;
using UnityEngine.Rendering;

public class MapDisplay : MonoBehaviour
{
    public SpriteRenderer _textureRender;

    public void DrawTexture(Texture2D texture)
    {
        _textureRender.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        _textureRender.transform.localScale = new Vector3(texture.width, texture.height, 1);
    }

    public void SetScale(float scale)
    {
        _textureRender.transform.localScale = new Vector3(scale, scale, 1);
    }
}