using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Line_Info
{
    public Vector3 Line_Pos;
    public Color Line_Color;
    public Quaternion Line_Rotation;
    public Meteor_Line_Info(Vector3 Line_Pos, Color Line_Color, Quaternion Line_Rotation)
    {
        this.Line_Pos = Line_Pos;
        this.Line_Color = Line_Color;
        this.Line_Rotation = Line_Rotation;
    }
    public Meteor_Line_Info(Meteor_Line_Info mine)
    {
        Line_Pos = mine.Line_Pos;
        Line_Color = mine.Line_Color;
        Line_Rotation = mine.Line_Rotation;
    }
}
public class Meteor_Traffic_Info : Meteor_Line_Info
{
    public Vector3 Traffic_Pos;
    public float Bright_Time;
    public int Bright_Count;
    public float Shake_Time;
    public float Shake_Intensity;
    public Meteor_Traffic_Info(Meteor_Line_Info mine, Vector3 Traffic_Pos, float Bright_Time, int _bright_Count, float Shake_Time, float Shake_Intensity) : base(mine)
    {
        this.Traffic_Pos = Traffic_Pos;
        this.Bright_Time = Bright_Time;
        Bright_Count = _bright_Count;
        this.Shake_Time = Shake_Time;
        this.Shake_Intensity = Shake_Intensity;
    }
}

// case 1 : 선이 빛날 때
// case 2 : 선이 빛나지 않을 때
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
    public IEnumerator Pattern02_Meteor(Meteor_Traffic_Info MT_I, Vector3 Target)
    {
        copy_Meteor_Line = Instantiate(Meteor_Line, MT_I.Line_Pos, MT_I.Line_Rotation);
        copy_Meteor_Traffic = Instantiate(Meteor_Traffic, MT_I.Traffic_Pos, Quaternion.identity);

        if (copy_Meteor_Line.TryGetComponent(out Meteor_Line ML) && copy_Meteor_Traffic.TryGetComponent(out Meteor_Traffic MT))
        {
            ML.StartCoroutine(ML.Change_Color(MT_I.Line_Color, MT_I.Bright_Count, MT_I.Bright_Time));
            yield return MT.Change_Color(MT_I.Bright_Count, MT_I.Bright_Time);
            yield return MT.Shake_Act(MT_I.Shake_Time, MT_I.Shake_Intensity);
        }
        Destroy(copy_Meteor_Traffic);
        Destroy(copy_Meteor_Line);

        spriteRenderer.color = new Color(1, 1, 1, 1);
        W_MoveTo(Target);

        yield return null;
    }
    public void Pattern04_Meteor_Launch(float temp, float R1, float R2, float R3)
    {
        Meteor_Traffic_Info MT = new Meteor_Traffic_Info(
            new Meteor_Line_Info(new Vector3(0, temp, 0), new Color(R1, R2, R3, 1), Quaternion.identity),
            new Vector3(8, temp, 0), 0.25f, 2, 0.6f, 1);
        StartCoroutine(Pattern02_Meteor(MT, Vector3.left));
    }
    public void Pattern02_Meteor_Launch(int Rand)
    {
        Meteor_Traffic_Info MT = new Meteor_Traffic_Info(
            new Meteor_Line_Info(new Vector3(Rand, 0, 0), Color.white, Quaternion.Euler(0, 0, 90)), 
            new Vector3(Rand, 3.7f, 0), 0, 0, 0.2f, 3);
        StartCoroutine(Pattern02_Meteor(MT, Vector3.down));
    }
    private void LateUpdate()
    {
        if (transform.position.x <= -10 || transform.position.y <= -5.7f)
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}