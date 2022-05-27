using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleSystemDestroy : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        if (TryGetComponent(out ParticleSystem user))
            ps = user;
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