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
        dd.text = "ȯ���մϴ�! " + PlayerPrefs.GetString("username") + "��!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
