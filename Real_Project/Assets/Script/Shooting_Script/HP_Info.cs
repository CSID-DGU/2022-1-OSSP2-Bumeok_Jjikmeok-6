using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HP_Info : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    float maxHP = 100;

    float currentHP;
    public float CurrentHP
    {
        set { currentHP = value; }
        get { return currentHP; }
    }
    public float MaxHP => maxHP;
    protected virtual void Awake()
    {
        CurrentHP = MaxHP;
    }

    



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
