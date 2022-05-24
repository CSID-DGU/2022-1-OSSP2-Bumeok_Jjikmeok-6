using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Effect : Weapon_Devil
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject Meteor_Line;

    [SerializeField]
    GameObject Meteor_Traffic;

    GameObject copy_Meteor_Line, copy_Meteor_Traffic;

    SpriteRenderer spriteRenderer;

    private new void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);
        W_MoveTo(Vector3.zero);
    }
    public IEnumerator Pattern02_Meteor(float temp)
    {
        copy_Meteor_Line = Instantiate(Meteor_Line, new Vector3(temp, 0, 0), Quaternion.Euler(0, 0, 90));
        copy_Meteor_Traffic = Instantiate(Meteor_Traffic, new Vector3(temp, 3.5f, 0), Quaternion.identity);

        Meteor_Line meteor_Line = null;

        IEnumerator u = null;

        if (copy_Meteor_Line.TryGetComponent(out Meteor_Line user1))
        {
            meteor_Line = user1;
            u = meteor_Line.Change_Color(1, 1, 1, 0.15f, true);
            meteor_Line.StartCoroutine(u);
        }
        if (copy_Meteor_Traffic.TryGetComponent(out Meteor_Traffic user2))
            yield return user2.StartCoroutine(user2.Shake_Act(0.3f, 0.8f));
        if (u != null && meteor_Line != null)
            meteor_Line.StopCoroutine(u);

        Destroy(copy_Meteor_Traffic);
        Destroy(copy_Meteor_Line);

        spriteRenderer.color = new Color(1, 1, 1, 1);
        W_MoveTo(Vector3.down);

        yield return null;
    }

    public IEnumerator Meteor_Launch_Act(float temp, float R1, float R2, float R3)
    {
        copy_Meteor_Line = Instantiate(Meteor_Line, new Vector3(0, temp, 0), Quaternion.identity);
        copy_Meteor_Traffic = Instantiate(Meteor_Traffic, new Vector3(8, temp, 0), Quaternion.identity);

        Meteor_Line meteor_Line = null;

        IEnumerator u = null;

        if (copy_Meteor_Line.TryGetComponent(out Meteor_Line user1))
        {
            meteor_Line = user1;
            u = meteor_Line.Change_Color(R1, R2, R3, 0.25f, true);
            meteor_Line.StartCoroutine(u);
        }
        if (copy_Meteor_Traffic.TryGetComponent(out Meteor_Traffic user2))
        {
            yield return user2.StartCoroutine(user2.Change_Color());
            yield return user2.StartCoroutine(user2.Shake_Act(0.6f, 3));
        }
        if (u != null && meteor_Line != null)
            meteor_Line.StopCoroutine(u);

        Destroy(copy_Meteor_Traffic);
        Destroy(copy_Meteor_Line);
        
        spriteRenderer.color = new Color(1, 1, 1, 1);
        W_MoveTo(Vector3.left);
        yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}