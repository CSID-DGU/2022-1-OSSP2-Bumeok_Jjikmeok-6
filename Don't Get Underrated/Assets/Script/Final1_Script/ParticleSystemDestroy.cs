using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemDestroy : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        if (TryGetComponent(out ParticleSystem PS))
            ps = PS;
    }

    void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}