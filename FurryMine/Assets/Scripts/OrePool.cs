using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrePool : MonoBehaviour
{
    [SerializeField]
    private Ore _orePrefab;

    public Ore CreateOre(Vector2 spawnPos)
    {
        return Instantiate(_orePrefab, spawnPos, Quaternion.identity, transform);
    }
}
