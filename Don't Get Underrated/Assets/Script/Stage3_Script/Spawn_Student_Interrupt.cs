using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Student_Interrupt : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject[] Interrupt;

    [SerializeField]
    GameObject[] Student;

    [SerializeField]
    float Standard_Interrupt_X = 3;

    [SerializeField]
    float Standard_Interrupt_Y = 3;

    [SerializeField]
    float Standard_Student_X = 0;

    public float Floor_Interval = 40;
    // 카메라의 크기는 18
    void Start()
    {
        Spawn_Student_And_Interrupt();
    }
    private void Spawn_Student_And_Interrupt()
    {
        int Max_Student = Student.Length;
        int Max_Interrupt = Interrupt.Length;
        float SpawnRange;
        for (int Floor = 0; Floor < 4; Floor++)
        {
            for (int Floor_Axis = 0; Floor_Axis < 10; Floor_Axis++)
            {
                int Student_Rand = Random.Range(0, Max_Student);
                int Interrupt_Rand = Random.Range(0, Max_Interrupt);
                int eee = Random.Range(0, 2);
                SpawnRange = Random.Range(-3, 3);
                Instantiate(Interrupt[Interrupt_Rand], new Vector3(Standard_Interrupt_X + (15 * Floor_Axis + SpawnRange), Standard_Interrupt_Y + (Floor_Interval * Floor), 1), Quaternion.identity);
 
                if (eee == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        SpawnRange = Random.Range(-3, 3);
                        Instantiate(Student[Student_Rand], new Vector3(Standard_Student_X + (18 * Floor_Axis + SpawnRange), -2.4f + (Floor_Interval * Floor), 1), Quaternion.identity);
                    }     
                }
                else
                {
                    SpawnRange = Random.Range(-3, 3);
                    Instantiate(Student[Student_Rand], new Vector3(Standard_Student_X + (18 * Floor_Axis + SpawnRange), -3.5f + (Floor_Interval * Floor), 1), Quaternion.identity);
                    SpawnRange = Random.Range(-3, 3);
                    Instantiate(Interrupt[Interrupt_Rand], new Vector3(Standard_Interrupt_X + (15 * Floor_Axis + SpawnRange), Standard_Interrupt_Y + (Floor_Interval * Floor), 1), Quaternion.identity);
                }
            }
        }
    }
    // Update is called once per frame
}
