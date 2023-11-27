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
}
