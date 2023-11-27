using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextPool : MonoBehaviour
{
    [SerializeField]
    private FloatingText _floatingTextPrefab;

    private Queue<FloatingText> _floatingTextQueue;

    private void Awake()
    {
        _floatingTextQueue = new Queue<FloatingText>();
    }

    public FloatingText CreateFloatingText(Vector2 spawnPos)
    {
        FloatingText text;
        if (_floatingTextQueue.Count > 0)
        {
            text = _floatingTextQueue.Dequeue();
            text.transform.position = spawnPos;
            text.gameObject.SetActive(true);
        }
        else
        {
            text = Instantiate(_floatingTextPrefab, spawnPos, Quaternion.identity, transform);
        }
        return text;
    }


    public void DestroyFloatingText(FloatingText text)
    {
        text.gameObject.SetActive(false);
        _floatingTextQueue.Enqueue(text);
    }
}
