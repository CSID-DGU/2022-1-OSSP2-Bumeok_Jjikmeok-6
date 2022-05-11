using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorichu : Enemy_Info
{
    // Start is called before the first frame upda

    [SerializeField]
    Vector3 Spawn_Position;

    Dictionary<int, Vector3> D;

    private new void Awake()
    {
        base.Awake();
        D = new Dictionary<int, Vector3>();
       
    }

    void Start()
    {
        transform.localScale = new Vector3(0.6f, 0.6f, 0);
        D.Add(0, transform.position);
        D.Add(1, new Vector3(transform.position.x, 3, 0));
        D.Add(2, new Vector3(transform.position.x, transform.position.y - 4, transform.position.z));
        StartCoroutine(Move());
    }
    IEnumerator Move() // 루트3 / 2 (0.85)로 끝맺음 짓는게 좋다.
    {
        Plus_Speed = 0;

        yield return StartCoroutine(Position_Lerp(D[0], D[1], 1f, inclineCurve));

        IEnumerator size = Size_Change_Infinite(3f);
        StartCoroutine(size);
        yield return StartCoroutine(Position_Lerp(D[1], D[2], 1f, declineCurve));
        StopCoroutine(size);
        Instantiate(Weapon[0], new Vector3(transform.position.x, 0, 0), Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
