using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorichu : Enemy_Info
{
    // Start is called before the first frame upda

    [SerializeField]
    Vector3 Spawn_Position;

    Dictionary<int, Vector3> D;

    IEnumerator size;

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
        Run_Life_Act(Move());
    }
    IEnumerator Move() // 루트3 / 2 (0.85)로 끝맺음 짓는게 좋다.
    {
        yield return StartCoroutine(Move_Straight(D[0], D[1], 1f, inclineCurve));

        Run_Life_Act_And_Continue(ref size, Change_My_Size_Infinite(3f));
        yield return Move_Straight(D[1], D[2], 1f, declineCurve);
        Stop_Life_Act(ref size);
        Instantiate(Weapon[0], new Vector3(transform.position.x, 0, 0), Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
}
