using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mousecursor : MonoBehaviour
{
    Player_Stage2 playerBall;

    public Texture2D cursorImg_kick;

    public Texture2D cursorImg_stop;

    DecideController decideController;
   
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorImg_kick, Vector2.zero, CursorMode.ForceSoftware);
        if (GameObject.Find("Lantern") && GameObject.Find("Lantern").TryGetComponent(out Player_Stage2 BC))
            playerBall = BC;
        if (GameObject.Find("HallOfFame") && GameObject.Find("HallOfFame").TryGetComponent(out DecideController DC))
            decideController = DC;
    }

    // Update is called once per frame
    void Update()
    {
        if (decideController != null)
        {
            if (!decideController.GameDone)
            {
                if (playerBall.ballHit == true)
                    Cursor.SetCursor(cursorImg_stop, Vector2.zero, CursorMode.ForceSoftware);
                else
                    Cursor.SetCursor(cursorImg_kick, Vector2.zero, CursorMode.ForceSoftware);
            }
            else
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
