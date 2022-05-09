using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor_kch : MonoBehaviour
{
    public Texture2D cursorImg_kick;
    public Texture2D cursorImg_stop;
    BallController_kch playerBall;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorImg_kick, Vector2.zero, CursorMode.ForceSoftware);
        this.playerBall = GameObject.Find("football").GetComponent<BallController_kch>();
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
