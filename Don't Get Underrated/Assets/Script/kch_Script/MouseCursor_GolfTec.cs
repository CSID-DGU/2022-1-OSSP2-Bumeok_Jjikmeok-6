using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor_GolfTec : MonoBehaviour
{
    public Texture2D cursorImg_kick;
    public Texture2D cursorImg_stop;
    BallController_GolfTec playerBall;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorImg_kick, Vector2.zero, CursorMode.ForceSoftware);
        playerBall = GameObject.Find("football").GetComponent<BallController_GolfTec>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerBall.ballHit == true)
        {
            Cursor.SetCursor(cursorImg_stop, Vector2.zero, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(cursorImg_kick, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
