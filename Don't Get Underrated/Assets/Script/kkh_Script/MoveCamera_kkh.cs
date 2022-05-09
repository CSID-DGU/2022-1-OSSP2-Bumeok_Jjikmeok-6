using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera_kkh : MonoBehaviour
{
    private Tmp_kkh Player_y_position;
    // Start is called before the first frame update
    void Start()
    {
        Player_y_position = GameObject.Find("Player").GetComponent<Tmp_kkh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player_y_position.Ch_y_position >= 5.0f)
        {
            gameObject.transform.position =new Vector3(0,8,-10);
        }
        if (Player_y_position.Ch_y_position >= 11.0f)
        {
            gameObject.transform.position = new Vector3(0, 14, -10);
        }
        if (Player_y_position.Ch_y_position < 5.0f)
        {
            gameObject.transform.position = new Vector3(0, 2, -10);
        }
        if(Player_y_position.Ch_y_position > 5.0f&&Player_y_position.Ch_y_position < 11.0f)
        {
            gameObject.transform.position = new Vector3(0, 8, -10);
        }
    }
}
