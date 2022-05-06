using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Another_Practice : MonoBehaviour
{
    // Start is called before the first frame update
    public Subject<int> mySubject = new Subject<int>(); // 정수형의 일종 '객체'를 생성.
    // 기본적으로 Subject 안에는 OnNext, OnCompleted가 들어있는 듯하다.

    IEnumerator Start()
    {
        var wait = new WaitForSeconds(1f);

        yield return wait;
        mySubject.OnNext(1);
        yield return wait;
        mySubject.OnNext(10);
        yield return wait;
        mySubject.OnCompleted();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
