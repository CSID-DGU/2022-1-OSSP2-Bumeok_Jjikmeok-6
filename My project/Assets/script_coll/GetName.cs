using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetName : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Text dd;

    void Start()
    {
        dd.text = "환영합니다! " + PlayerPrefs.GetString("username") + "님!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
