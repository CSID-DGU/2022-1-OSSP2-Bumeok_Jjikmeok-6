using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Effect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject Meteor_Line;

    [SerializeField]
    GameObject Meteor_Traffic;

    GameObject copy_Meteor_Line, copy_Meteor_Traffic;

    Movement2D movement2D;

    SpriteRenderer spriteRenderer;

    IEnumerator u, h, q;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);

        movement2D.MoveTo(Vector3.zero);
    }

    void Start()
    {
    }

    public IEnumerator Meteor_Launch_Act(float temp, float R1, float R2, float R3)
    {
        copy_Meteor_Line = Instantiate(Meteor_Line, new Vector3(0, temp, 0), Quaternion.identity);
        copy_Meteor_Traffic = Instantiate(Meteor_Traffic, new Vector3(8, temp, 0), Quaternion.identity);

        u = copy_Meteor_Line.GetComponent<Meteor_Line>().Change_Color(R1, R2, R3);
        h = copy_Meteor_Traffic.GetComponent<Meteor_Traffic>().Change_Color();
        q = copy_Meteor_Traffic.GetComponent<Meteor_Traffic>().Shake_Act();

        StartCoroutine(u);

        yield return StartCoroutine(h);
        yield return StartCoroutine(q);

        if (u != null)
            StopCoroutine(u);
        if (h != null)
            StopCoroutine(h);
        if (q != null)
            StopCoroutine(q);

        Destroy(copy_Meteor_Line);
        Destroy(copy_Meteor_Traffic);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        movement2D.MoveTo(Vector3.left);
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
