using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState { MoveToAppearPoint = 0, Phase01, Phase02, Phase03 }

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float bossAppearPoint = 2.5f;

    [SerializeField]
    GameObject explosionPrefab;

    [SerializeField]
    StageData stageData;

    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    string nextSceneName;

    Movement2D movement2D;

    BossWeapon bossWeapon;

    BossHP bossHP;

    BossState bossState = BossState.MoveToAppearPoint;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        bossWeapon = GetComponent<BossWeapon>();
        bossHP = GetComponent<BossHP>();
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
        while(true)
        {
            if (bossHP.CurrentHP <= bossHP.MaxHP * 0.7f)
            {
                bossWeapon.StopFiring(AttackType.CircleFire);
                ChangeState(BossState.Phase02);
            }
            yield return null;
        }
    }
    IEnumerator Phase02()
    {
        bossWeapon.StartFiring(AttackType.SingleFireToCenterPosition);

        Vector3 direction = Vector3.right;
        movement2D.MoveTo(direction);

        while (true)
        {
            if (transform.position.x <= stageData.LimitMin.x || 
                transform.position.x >= stageData.LimitMax.x)
            {
                direction *= -1;
                movement2D.MoveTo(direction);
            }
            if (bossHP.CurrentHP <= bossHP.MaxHP * 0.3f)
            {
                bossWeapon.StopFiring(AttackType.SingleFireToCenterPosition);
                ChangeState(BossState.Phase03);
            }

            yield return null;
        }
    }
    IEnumerator Phase03()
    {
        bossWeapon.StartFiring(AttackType.CircleFire);
        bossWeapon.StartFiring(AttackType.SingleFireToCenterPosition);

        Vector3 direction = Vector3.right;
        movement2D.MoveTo(direction);

        while(true)
        {
            if (transform.position.x <= stageData.LimitMin.x || 
                transform.position.x >= stageData.LimitMax.x)
            {
                direction *= -1;
                movement2D.MoveTo(direction);
            }
            yield return null;
        }
    }
    public void OnDie()
    {
        GameObject clone = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Debug.Log("¾ß1");
        clone.GetComponent<BossExplosion>().Setup(playerController, nextSceneName);
        Debug.Log("¾ß2");
        Destroy(gameObject);
        Debug.Log("¾ß3");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
