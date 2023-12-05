using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionEquip : MonoBehaviour
{
    private Dictionary<int, PossessionEquip> _equipDict;

    private void Awake()
    {
        _equipDict = new Dictionary<int, PossessionEquip>();
    }

}
