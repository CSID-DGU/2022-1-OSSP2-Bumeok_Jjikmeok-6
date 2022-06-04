using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorichu : Enemy_Info
{
    // Start is called before the first frame upda

    [SerializeField]
    private Vector3 Spawn_Position;

    private Dictionary<int, Vector3> D;

    private IEnumerator size;

    private new void Awake()
    {
        base.Awake();
        D = new Dictionary<int, Vector3>();
        if (GameObject.Find("Enemy_Effect_Sound") && GameObject.Find("Enemy_Effect_Sound").TryGetComponent(out AudioSource AS1))
            EffectSource = AS1;
    }
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && collision.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
                HC.TakeDamage(1);
        }
    }
    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info HC))
        {
            if (!HC.Unbeatable)
                HC.TakeDamage(1);
        }
    }
    private void Start()
    {
        My_Scale = new Vector3(1.7f, 1.7f, 0);
        D.Add(0, My_Position);
        D.Add(1, new Vector3(My_Position.x, 3, 0));
        D.Add(2, new Vector3(My_Position.x, My_Position.y - 4, My_Position.z));
        Run_Life_Act(Move());
    }
    private IEnumerator Move() // 루트3 / 2 (0.85)로 끝맺음 짓는게 좋다.
    {
        Effect_Sound_OneShot(0);
        yield return Move_Straight(D[0], D[1], 1f, inclineCurve);

        Effect_Sound_OneShot(1);
        Run_Life_Act_And_Continue(ref size, Change_My_Size_Infinite(3f));
        yield return Move_Straight(D[1], D[2], 1f, declineCurve);
        Stop_Life_Act(ref size);
        Instantiate(Weapon[0], new Vector3(My_Position.x, 0, 0), Quaternion.identity);
        Destroy(gameObject);
    }
    // Update is called once per frame
}
