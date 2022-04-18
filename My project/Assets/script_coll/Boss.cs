using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState { MoveToAppearPoint = 0, Phase01, }

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float bossAppearPoint = 2.5f;

    Movement2D movement2D;

    BossWeapon bossWeapon;

    BossState bossState = BossState.MoveToAppearPoint;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        bossWeapon = GetComponent<BossWeapon>();
    }
    public void ChangeState(BossState newState)
    {
        StopCoroutine(bossState.ToString());
        bossState = newState;
        StartCoroutine(bossState.ToString());

    }
    IEnumerator MoveToAppearPoint()
    {
        movement2D.MoveTo(Vector3.down);
        while(true)
        {
            if (transform.position.y <= bossAppearPoint)
            {
                movement2D.MoveTo(Vector3.zero);
                ChangeState(BossState.Phase01);
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator Phase01()
    {
        bossWeapon.StartFiring(AttackType.CircleFire);
        yield return null;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
