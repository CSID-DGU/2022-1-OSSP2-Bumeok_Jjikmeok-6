using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoyBean : Weapon_Devil
{
    [SerializeField]
    [Range(500f, 2000f)] float speed = 1000f;

    [SerializeField]
    float distanceToGround = 0.2f;

    Rigidbody2D rb;

    CameraShake cameraShake;

    [SerializeField]
    GameObject Disappear_Effect;
    float randomX, randomY;

    private int Count;
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        cameraShake = GetComponent<CameraShake>();
        cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Count = 0;
    }

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out HimaController user))
        {
            randomX = user.transform.position.x - transform.position.x;
            randomY = user.transform.position.y - transform.position.y;
        }
        else
        {
            randomX = Random.Range(-1f, 1f);
            randomY = Random.Range(-1f, 1f);
        }
        Vector2 dir = new Vector2(randomX, randomY).normalized;
        rb.AddForce(dir * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player_Info user2))
        {
            user2.TakeDamage(1);
        }
        if (collision.gameObject != null && collision.gameObject.CompareTag("Ground"))
        {
            cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
            cameraShake.mainCamera.transform.rotation = Quaternion.identity;
            cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(cameraShake.Shake_Act(0.15f, 0.15f, 0.05f, false));
            Count++;
            if (Count >= 4)
                OnDie();
        }
    }

    void OnDie()
    {
        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);

        Instantiate(Disappear_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
