using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class MineralPool : MonoBehaviour
{
    [SerializeField]
    private Mineral _mineralPrefab;

    private Queue<Mineral> _mineralQueue;

    private void Awake()
    {
        _mineralQueue = new Queue<Mineral>();
    }

    public Mineral CreateMineral(Vector2 spawnPos)
    {
        Mineral mineral;
        if (_mineralQueue.Count > 0)
        {
            mineral = _mineralQueue.Dequeue();
            mineral.transform.position = spawnPos;
            mineral.gameObject.SetActive(true);
        }
        else
        {
            mineral = Instantiate(_mineralPrefab, spawnPos, Quaternion.identity, transform);
        }
        return mineral;
    }

    public void DestroyMineral(Mineral mineral)
    {
        mineral.gameObject.SetActive(false);
        _mineralQueue.Enqueue(mineral);
    }
}
