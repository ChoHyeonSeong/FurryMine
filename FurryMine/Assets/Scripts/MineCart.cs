using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCart : MonoBehaviour
{
    public static Action<int> OnPlusMoney { get; set; }

    private int _money = 0;

    public void PlusMoney(int price)
    {
        _money += price;
        OnPlusMoney(_money);
    }
}
