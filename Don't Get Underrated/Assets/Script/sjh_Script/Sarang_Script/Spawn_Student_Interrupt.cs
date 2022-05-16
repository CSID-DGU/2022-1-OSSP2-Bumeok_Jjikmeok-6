using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Student_Interrupt : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject Student;

    [SerializeField]
    GameObject Interrupt;

    [SerializeField]
    float Standard_Interrupt_X = 3;

    [SerializeField]
    float Standard_Interrupt_Y = 3;

    [SerializeField]
    float Standard_Student_X = 0;

    [SerializeField]
    float Standard_Student_Y = -2;
    // 카메라의 크기는 18
    void Start()
    {
        StartCoroutine(Spawn_Student_And_Interrupt());
    }
    IEnumerator Spawn_Student_And_Interrupt()
    {
        for (int i = 0; i < 18; i++)
        {
            int eee = Random.Range(0, 2);
            float SpawnRange = Random.Range(-3, 3);
            Instantiate(Interrupt, new Vector3(Standard_Interrupt_X + (26 * i + SpawnRange), Standard_Interrupt_Y, 1), Quaternion.identity);
            if (eee == 0)
                Instantiate(Student, new Vector3(Standard_Student_X + (18 * i + SpawnRange), -4, 1), Quaternion.identity);
            else
                Instantiate(Student, new Vector3(Standard_Student_X + (18 * i + SpawnRange), -1, 1), Quaternion.identity);
            yield return null;
        }

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
