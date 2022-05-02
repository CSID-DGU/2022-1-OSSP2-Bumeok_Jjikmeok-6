using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Trace : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject player;

    private void Update()
    {
        if (player)
        {
            Vector3 newPos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
            transform.position = newPos;
        }
        else
            player = GameObject.FindGameObjectWithTag("Player");
    }
}
