using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource bgm;

    private void Awake()
    {
        if (TryGetComponent(out AudioSource AS))
            bgm = AS;
    }
    void Update()
    {
        if (singleTone.Music_Decrease)
            bgm.volume = singleTone.Music_Volume;
    }

    public void musicCheck()
    {
        bgm.mute = !bgm.mute;
    }

    public void setVolume(float vol)
    {
        singleTone.Music_Volume = vol;
    }
}
