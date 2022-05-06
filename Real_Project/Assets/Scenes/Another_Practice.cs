using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Another_Practice : MonoBehaviour
{
    // Start is called before the first frame update
    public Subject<int> mySubject = new Subject<int>(); // �������� ���� '��ü'�� ����.
    // �⺻������ Subject �ȿ��� OnNext, OnCompleted�� ����ִ� ���ϴ�.

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
