using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrePool : MonoBehaviour
{
    [SerializeField]
    private Ore _orePrefab;

    private Queue<Ore> _oreQueue;

    private void Awake()
    {
        _oreQueue = new Queue<Ore>();
    }

    public Ore CreateOre(Vector2 spawnPos)
    {
        Ore ore;
        if(_oreQueue.Count > 0)
        {
            ore = _oreQueue.Dequeue();
            ore.transform.position = spawnPos;
            ore.gameObject.SetActive(true);
        }
        else
        {
            ore = Instantiate(_orePrefab, spawnPos, Quaternion.identity, transform);
        }
        return ore;
    }

    public void DestroyOre(Ore ore)
    {
        ore.gameObject.SetActive(false);
        _oreQueue.Enqueue(ore);
    }
}
