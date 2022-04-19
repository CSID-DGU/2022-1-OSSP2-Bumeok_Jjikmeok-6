using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroyer : MonoBehaviour
{
    // Start is called before the first frame update

    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!particle.isPlaying)
        {
            Debug.Log("이거 이상한데");
            Destroy(gameObject);
        }
    }
}
