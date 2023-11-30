using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    private FloatingTextPool _floatingTextPool;
    private void Awake()
    {
        _floatingTextPool = GetComponent<FloatingTextPool>();
    }
    private void OnEnable()
    {
        Ore.OnHitText += SpawnText;
        MineCart.OnPlusText += SpawnText;
        FloatingText.OnFloatingEnd += CollectText;
    }

    private void OnDisable()
    {
        Ore.OnHitText -= SpawnText;
        MineCart.OnPlusText -= SpawnText;
        FloatingText.OnFloatingEnd -= CollectText;
    }

    private void CollectText(FloatingText text)
    {
        _floatingTextPool.DestroyFloatingText(text);
    }

    private void SpawnText(bool isGold, string content, Vector2 pos)
    {
        FloatingText text = _floatingTextPool.CreateFloatingText(pos);
        text.Init(isGold, content);
    }
}
