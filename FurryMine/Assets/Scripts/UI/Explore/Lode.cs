using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lode : MonoBehaviour
{
    public static Action OnDiscoverLode { get; set; }

    [SerializeField]
    private ParticleSystem _shine;


    public void DiscoverLode()
    {
        _shine.gameObject.SetActive(true);
        _shine.Play();
        OnDiscoverLode();
    }

    public void OffShine()
    {
        _shine.gameObject.SetActive(false);
        _shine.Pause();
    }
}
