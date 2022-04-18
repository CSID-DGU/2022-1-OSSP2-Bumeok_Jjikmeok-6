using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { CircleFire = 0,}

public class BossWeapon : MonoBehaviour
{

    [SerializeField]
    GameObject projectilePrefab;

    public void StartFiring(AttackType attackType)
    {
        StartCoroutine(attackType.ToString());
    }
    public void StopFiring(AttackType attackType)
    {
        StopCoroutine(attackType.ToString());
    }
    IEnumerator CircleFire()
    {
        float attackRate = 0.5f;
        int count = 10;
        float intervalAngle = 360 / count;
        float weightAngle = 0;
        while (true)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                float angle = weightAngle + intervalAngle * i;
                float x = Mathf.Cos(angle * Mathf.PI / 100.0f);
                float y = Mathf.Sin(angle * Mathf.PI / 100.0f);
                clone.GetComponent<Movement2D>().MoveTo(new Vector3(x, y));
            }
            weightAngle += 1;

            yield return new WaitForSeconds(attackRate);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
