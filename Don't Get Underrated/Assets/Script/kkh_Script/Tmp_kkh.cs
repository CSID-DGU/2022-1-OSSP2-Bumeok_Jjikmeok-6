using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tmp_kkh : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite jump_sprite;
    public Sprite Stun_sprite;
    public Sprite Collide_sprite;
    public Sprite Up_sprite;
    public Sprite stop_sprite;
    public Sprite Down_sprite;
    public bool isMove;
    private float speed =3.5f;
    [SerializeField]
    private float jumpForce = 5.0f;
    private Rigidbody2D rigid2D;
    [SerializeField]
    public float Power_jump = 2.0f;
    [HideInInspector]
    public bool isJumped = false;
    // Start is called before the first frame update
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask wallLayer;
    public BoxCollider2D boxCollider2D;
    private bool isGrounded;
    private bool isfainted;
    private bool isStuck_left;
    public PhysicsMaterial2D bounceMat, normalMat;
    public float Ch_y_position;
    public float Jump_start_position;
    private bool isJumping = false;
    public Animator animator;
    public GameObject Ground;
    public GameObject Wall;
    public float Prev_pos_y;
    public float Now_pos_y;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

    }
    // Update is called once per frame
    private void Update()
    {//플레이어 오브젝트의 collider 2d min center max 위치정보
     //플레이어의 발 위치 설정
     //플레이어 발 위치에 원생성, 바닥과 닿아있으면 ㅅrue
     //isGrounded = Physics2D.OverlapCircle(footPosition, 0.1f, groundLayer);
     //
        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.3f), new Vector2(0.15f, 0.05f), 0f, groundLayer);
        Ch_y_position = gameObject.transform.position.y;
        isStuck_left = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(0.5f, 0.5f), 0f, wallLayer);

        if (isGrounded)
        {
            if (isStuck_left)
            {
            }
            isJumping = false;
            animator.enabled = true;
            isfainted = false;
        }
        else
        {
            animator.enabled = false;
        }
        float x = Input.GetAxisRaw("Horizontal");
        if (x != 0&& isGrounded)
        {
            animator.SetBool("run", true);
            transform.localScale = new Vector3(-1 * x, 1, 1);
        }
        else
        {

            animator.SetBool("run", false);
        }

        if (!isGrounded && (Ch_y_position > Prev_pos_y) && !isfainted)
        {
            animator.enabled = false;
            spriteRenderer.sprite = Up_sprite;
        }
        else if (!isGrounded && (Ch_y_position < Prev_pos_y) && !isfainted)
        {
            animator.enabled = false;
            spriteRenderer.sprite = Down_sprite;
        }
        else
        {

        }

        if (isGrounded && (Ch_y_position - Jump_start_position < -4.0f))
        {
            animator.enabled = false;
            spriteRenderer.sprite = Stun_sprite;
            Invoke("stand_again", 1f);
        }
        Debug.Log(isGrounded + " " + isStuck_left + " " + isJumping);
        if (!isGrounded && isStuck_left&&isJumping)
        {
            Debug.Log("퍽");
            isfainted = true;
            animator.enabled = false;
            spriteRenderer.sprite = Collide_sprite;
        }
        if (isGrounded)
        {
            if (!isJumping&&!Input.GetKey(KeyCode.Space))
            {
                rigid2D.velocity = new Vector2(speed * x, rigid2D.velocity.y);
            }


            Ready_to_Jump(x);
        }

        Prev_pos_y = rigid2D.transform.position.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.3f), new Vector2(0.15f, 0.05f));
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(0.5f, 0.5f));
        
    }
    public void Ready_to_Jump(float x)
    {
        isJumping = true;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid2D.velocity = new Vector2(0, rigid2D.velocity.y);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            animator.enabled = false;
            spriteRenderer.sprite = jump_sprite;
            Power_jump += 0.25f;
            if (Power_jump > 8f)
            {
                Power_jump = 8f;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Power_jump <= 3f)
            {
                Jump(3f, x);
            }
            else
            {
                Jump(Power_jump, x);

            }
            Power_jump = 0.0f;
            animator.enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject ==Wall&&isGrounded&&isStuck_left)
        {
            rigid2D.velocity = Vector2.zero;
        }
        else
        {

        }
    }
    public void Jump(float y, float x)
    {
        
            //rigid2D.velocity = new Vector2(x * speed, y*1.5f);
            //Debug.Log("이걸로 적용되야하는뎅");
            rigid2D.AddForce(Vector2.up * y * 1.5f, ForceMode2D.Impulse);
            //rigid2D.AddForce(new Vector2(x * speed*1.5f, y * 1.5f), ForceMode2D.Impulse);
            Jump_start_position = gameObject.transform.position.y;
            isJumping = true;

    }
    void stand_again()
    {
        Jump_start_position = 0.0f;
        animator.enabled = true;
    }
}