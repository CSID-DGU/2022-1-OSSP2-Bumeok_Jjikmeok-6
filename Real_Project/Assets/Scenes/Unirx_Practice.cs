using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Unirx_Practice : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        var mySubject = GetComponent<Another_Practice>().mySubject;
        mySubject.Subscribe(n => SomeMethod(n)); // 애초에 지금의 mySubject가 Subject라는 타입을 가졌다. 
    }

    private void SomeMethod(int n)
    {
        Debug.Log(n);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
