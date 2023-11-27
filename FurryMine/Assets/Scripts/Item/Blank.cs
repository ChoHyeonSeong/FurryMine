using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blank : MonoBehaviour
{
    public int ObjectCount { get; private set; } = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Consts.OreTag) || collision.CompareTag(Consts.MinerTag))
        {
            ObjectCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Consts.OreTag) || collision.CompareTag(Consts.MinerTag))
        {
            ObjectCount--;
        }
    }
}
