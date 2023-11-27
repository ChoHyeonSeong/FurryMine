using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnforceList : MonoBehaviour
{
    public Dictionary<EEnforce, EnforceItem> EnforceDict { get; private set; }

    private void Awake()
    {
        var enforceItems = GetComponentsInChildren<EnforceItem>();
        EnforceDict = new Dictionary<EEnforce, EnforceItem>();
        foreach (var item in enforceItems)
        {
            EnforceDict.Add(item.Enforce, item);
        }
    }
}
