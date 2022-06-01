using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_kkh : MonoBehaviour
{

    [SerializeField]
    AudioSource Ground_Sound;

    private GameObject Player;
   
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Player"))
            Player = GameObject.Find("Player");
        if (GameObject.Find("Ground_Sound") && GameObject.Find("Ground_Sound").TryGetComponent(out AudioSource AS))
            Ground_Sound = AS;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Player == null)
            return;
        if (collision.gameObject == Player)
            Ground_Sound.Play();
        else
        {

        }
    }
}
