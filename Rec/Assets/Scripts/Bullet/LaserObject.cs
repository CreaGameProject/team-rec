using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LaserObject : MonoBehaviour
{
    private GameObject razerHead;
    [SerializeField] private Component razerParticles;

    private void Awake()
    {
        razerHead = new GameObject("RazerHead");
        razerHead.transform.SetParent(this.gameObject.transform);
        razerParticles = GetComponentInChildren(typeof(ParticleSystem));
    }

    private void OnDisable()
    {
        razerHead.transform.position = this.transform.position;
    }
}