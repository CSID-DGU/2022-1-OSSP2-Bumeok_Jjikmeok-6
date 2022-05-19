using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_1_W2_Boom : Weapon_Player
{
    [SerializeField]
    AnimationCurve curve;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(MoveToCenter());
    }
    IEnumerator MoveToCenter()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = Vector3.zero;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percent));
            yield return null;
        }
        animator.SetTrigger("onBoom");
    }
}
