using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BossState { Phase01 = 0, Phase02, Phase03 }

public class Boss : HP_Info
{
    // Start is called before the first frame update

    [SerializeField]
    float body_damage = 0.7f;

    [SerializeField]
    AnimationCurve curve;


    [SerializeField]
    GameObject Die_Explosion;

    BossState bossState = BossState.Phase01;

    float[] Move_Array1 = new float[18] { 0, 0, 0, -6, 2, 0, -7, -3, 0, 7, 2, 0, 0, -4, 0, 0, 0, 0 };
    float[] Move_Array2 = new float[18] { 0, 0, 0, -7, -3, 0, 0, 4, 0, 6, -4, 0, -6, 2, 0, 0, 0, 0 };
    float[] Move_Array3 = new float[18] { 0, 0, 0, 0, 4, 0, -6, -4, 0, 6, 3, 0, -6, 2, 0, 0, 0, 0 };

    PlayerControl playerControl;

    float boomDelay = 0.5f;

    SpriteRenderer spriteRenderer;

    [SerializeField]
    GameObject Lightning;

    BossHPSliderViewer bossHPSliderViewer;

    new private void Awake()
    {
        base.Awake();
        playerControl = GameObject.FindGameObjectWithTag("Playerrr").GetComponent<PlayerControl>();
        CurrentHP = MaxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Playerrr"))
        {
            if (!collision.GetComponent<PlayerControl>().Unbeatable_Player)
            {
                collision.GetComponent<PlayerControl>().TakeDamage(body_damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        StartCoroutine("Hit");
        if (CurrentHP <= 0)
        {
            OnDie();
        }
    }
    IEnumerator Hit()
    {
        spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    public void OnDie()
    {
        playerControl.Score += 10000;
        Instantiate(Die_Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void Phase_Start(BossState newState)

    {
        StopCoroutine(bossState.ToString());
        bossState = newState;
        StartCoroutine(bossState.ToString());
    }
    IEnumerator Phase01()
    {
       
        int Boss_Random_Move = Random.Range(0, 3);

        switch (Boss_Random_Move)
        {
            case 0:
                yield return StartCoroutine(Boss_Random_Move_F(Move_Array1));
                break;
            case 1:
                yield return StartCoroutine(Boss_Random_Move_F(Move_Array2));
                break;
            case 2:
                yield return StartCoroutine(Boss_Random_Move_F(Move_Array3));
                break;
        }
        
        GameObject L1 = Instantiate(Lightning, transform.position, Quaternion.identity);

        GameObject L2 = Instantiate(Lightning, transform.position, Quaternion.Euler(new Vector3(0, 0, -60)));
        L2.GetComponent<Movement2D>().MoveTo(new Vector3(1, -0.5714f, 0));

        GameObject L3 = Instantiate(Lightning, transform.position, Quaternion.Euler(new Vector3(0, 0, -180)));
        L3.GetComponent<Movement2D>().MoveTo(new Vector3(-1, -0.5714f, 0));

        GameObject L4 = Instantiate(Lightning, transform.position, Quaternion.Euler(new Vector3(0, 0, 120)));
        L4.GetComponent<Movement2D>().MoveTo(new Vector3(-1, 0.5714f, 0));
    }
    IEnumerator Boss_Random_Move_F(float[] arr)
    { 
        for (int i = 0; i < 6; i++)
        {
            float current = 0;
            float percent = 0;
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / boomDelay;

                transform.position = Vector3.Lerp(transform.position, new Vector3(arr[0 + i * 3], arr[1 + i * 3], arr[2 + i * 3]), curve.Evaluate(percent));
                yield return null;
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
