using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HommingMissile : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    private Rigidbody2D rb;
    public float speed = 5;
    public float rotateSpeed = 200f;

    public GameObject ExplosionEffect;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
            StartCoroutine("DestroyAfter");
        }
    }
    IEnumerator DestroyAfter()
    {
        GameObject noejeol = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Destroy(noejeol);
        Destroy(gameObject);
        yield return null;
    }
}
