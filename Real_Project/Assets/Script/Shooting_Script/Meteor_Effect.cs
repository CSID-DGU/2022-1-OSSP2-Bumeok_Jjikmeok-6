using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Effect : MonoBehaviour
{
    // Start is called before the first frame update

    Movement2D movement2D;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    GameObject Meteor_Line_O;

    [SerializeField]
    GameObject Meteor_Traffic_O;

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
        
        GameObject e = Instantiate(Meteor_Line_O, new Vector3(0, temp, 0), Quaternion.identity);
        GameObject f = Instantiate(Meteor_Traffic_O, new Vector3(8, temp, 0), Quaternion.identity);

        IEnumerator u = e.GetComponent<Meteor_Line>().Change_Color(R1, R2, R3);
        StartCoroutine(u);
        yield return StartCoroutine(f.GetComponent<Meteor_Traffic>().Change_Color());
        yield return StartCoroutine(f.GetComponent<Meteor_Traffic>().Shake_Act());
        StopCoroutine(u);
  
        Destroy(f);
        Destroy(e);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        movement2D.MoveTo(Vector3.left);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
