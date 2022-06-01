using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource bgm;
    float musicVolume = 1f;

    private void Awake()
    {
        if (TryGetComponent(out AudioSource AS))
            bgm = AS;
    }
    void Update()
    {
        bgm.volume = musicVolume;
    }

    public void musicCheck()
    {
        bgm.mute = !bgm.mute;
    }

    public void setVolume(float vol)
    {
        musicVolume = vol;
    }
}
