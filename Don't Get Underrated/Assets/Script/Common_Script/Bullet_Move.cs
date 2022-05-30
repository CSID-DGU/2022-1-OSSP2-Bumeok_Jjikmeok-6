﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Move : Weapon_Devil {

    [SerializeField]
    private float speed = 10f;

    private new void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        //생성으로부터 2초 후 삭제
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        //두번째 파라미터에 Space.World를 해줌으로써 Rotation에 의한 방향 오류 수정
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
    }
}
