using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLeft : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Text text;
    [SerializeField]
    Weapon weapon;
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        text.text = "explosion_left : " + weapon.BoomCount;
    }
}
