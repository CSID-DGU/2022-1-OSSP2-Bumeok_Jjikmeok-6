using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Info : Player_And_Boss
{
    // Start is called before the first frame update

    int score1, score2, score3;
    public override void OnDie()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
