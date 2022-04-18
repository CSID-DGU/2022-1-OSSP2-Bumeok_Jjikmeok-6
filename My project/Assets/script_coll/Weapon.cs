using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrefab; // ���� ������Ʈ (���⼭�� �Ѿ��� �ǰ���)
    // �ܼ��� ���� �Ѿ��� �ƴ�, �Ѿ��� �����̴� ��� ���� ��Ʋ� GameObject��� �Ѵ�.
    [SerializeField]
    float attackRate = 0.1f;

    int attacklevel = 1;


    [SerializeField]
    GameObject boomPrefab;

    int boomCount = 3;

    public int BoomCount
    {
        get => boomCount;
    }

    public int AttackLevel_I
    {
        set
        {
            if (value >= 4)
                value--;
            attacklevel = value;
        }
        get => attacklevel;
    }
    public void StartFiring()
    {
        StartCoroutine("TryAttack");
    }
    public void StopFiring()
    {
        StopCoroutine("TryAttack");
    }
    // Start is called before the first frame update
    IEnumerator TryAttack()
    {
        while (true)
        {
            AttackLevel();
            
            yield return new WaitForSeconds(attackRate);
        }
    }
    void Start()
    {
        
    }
    public void StartBoom()
    {
        if (boomCount > 0)
        {
            boomCount--;
            Instantiate(boomPrefab, transform.position, Quaternion.identity);
        }
    }
    private void AttackLevel()
    {
        GameObject cloneProjectile = null;
        switch (attacklevel)
        {
            case 1:
                // ���� ���� ���� ������Ʈ ������ �� ����.
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                break;
            case 2:
                // ���� ���� ���� ������Ʈ ������ �� ����.
                Instantiate(projectilePrefab, transform.position + Vector3.left * 0.2f, Quaternion.identity);
                Instantiate(projectilePrefab, transform.position + Vector3.right * 0.2f, Quaternion.identity);
                break;
            case 3:
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                cloneProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                cloneProjectile.GetComponent<Movement2D>().MoveTo(new Vector3(-0.2f, 1, 0));

                cloneProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                cloneProjectile.GetComponent<Movement2D>().MoveTo(new Vector3(0.2f, 1, 0));
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
