using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera_kkh : MonoBehaviour
{
    private Player_Stage1 Player_y_position;

    private void Awake()
    {
        if (GameObject.Find("Player") && GameObject.Find("Player").TryGetComponent(out Player_Stage1 MK))
            Player_y_position = MK;
    }
    void Update()
    {
        if (Player_y_position.Ch_y_position >= 5.0f)
        {
            transform.position = new Vector3(0, 8, -10);
        }

        else if (Player_y_position.Ch_y_position < 5.0f)
        {
            transform.position = new Vector3(0, 2, -10);
        }

        if (Player_y_position.Ch_y_position >= 11.0f)
        {
            transform.position = new Vector3(0, 14, -10);
        }
        else if (Player_y_position.Ch_y_position > 5.0f && Player_y_position.Ch_y_position < 11.0f)
        {
            transform.position = new Vector3(0, 8, -10);
        }

        if (Player_y_position.Ch_y_position >= 17.0f)
        {
            transform.position = new Vector3(0, 20, -10);
        }
        else if (Player_y_position.Ch_y_position > 11.0f && Player_y_position.Ch_y_position < 17.0f)
        {
            transform.position = new Vector3(0, 14, -10);
        }

        if (Player_y_position.Ch_y_position >= 23.0f)
        {
            transform.position = new Vector3(0, 26, -10);
        }
        else if (Player_y_position.Ch_y_position > 17.0f && Player_y_position.Ch_y_position < 23.0f)
        {
            transform.position = new Vector3(0, 20, -10);
        }

        if (Player_y_position.Ch_y_position >= 29.0f)
        {
            transform.position = new Vector3(0, 32, -10);
        }
        else if (Player_y_position.Ch_y_position > 23.0f && Player_y_position.Ch_y_position < 29.0f)
        {
            transform.position = new Vector3(0, 26, -10);
        }

        if (Player_y_position.Ch_y_position >= 35.0f)
        {
            transform.position = new Vector3(0, 38, -10);
        }
        else if (Player_y_position.Ch_y_position > 29.0f && Player_y_position.Ch_y_position < 35.0f)
        {
            transform.position = new Vector3(0, 32, -10);
        }

        if (Player_y_position.Ch_y_position >= 41.0f)
        {
            transform.position = new Vector3(0, 44, -10);
        }
        else if (Player_y_position.Ch_y_position > 35.0f && Player_y_position.Ch_y_position < 41.0f)
        {
            transform.position = new Vector3(0, 38, -10);
        }

        if (Player_y_position.Ch_y_position >= 47.0f)
        {
            transform.position = new Vector3(0, 50, -10);
        }
        else if (Player_y_position.Ch_y_position > 41.0f && Player_y_position.Ch_y_position < 47.0f)
        {
            transform.position = new Vector3(0, 44, -10);
        }

        if (Player_y_position.Ch_y_position >= 53.0f)
        {
            transform.position = new Vector3(0, 56, -10);
        }
        else if (Player_y_position.Ch_y_position > 47.0f && Player_y_position.Ch_y_position < 53.0f)
        {
            transform.position = new Vector3(0, 50, -10);
        }
    }
}
