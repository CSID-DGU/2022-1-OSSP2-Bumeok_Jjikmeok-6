using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D_kkh : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite jump_sprite;
    public Sprite Stun_sprite;
    public Sprite Collide_sprite;
    public Sprite Up_sprite;
    public Sprite stop_sprite;
    public Sprite Down_sprite;
    public bool isDelay;
    private float speed = 3.5f;
    [SerializeField]
    private float jumpForce = 5.0f;
    private Rigidbody2D rigid2D;
    [SerializeField]
    private float Power_jump = 2.0f;
    [HideInInspector]
    public bool isJumped = false;
    // Start is called before the first frame update
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask blockLayer;
    [SerializeField]
    private BoxCollider2D Wall;
    [SerializeField]
    private BoxCollider2D Ground;
    private bool isGrounded;
    private bool isfainted;
    private bool isStuck_left;
    private bool isAir = false;
    private Vector3 footPosition;
    public PhysicsMaterial2D bounceMat, normalMat;
    public float Ch_y_position;
    public float Jump_start_position;
    private bool isJumping = false;
    public Animator animator;
    private bool overlap_grounded;
    public GameObject hand;
    public float Prev_pos_y;
    public float Now_pos_y;
    private void Awake()
    {
        hand = GameObject.Find("Grid");
        hand=hand.transform.GetChild(0).gameObject;
        Debug.Log(hand.name);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();
        Wall = GetComponent<BoxCollider2D>();
        Ground = GetComponent<BoxCollider2D>();

    }
    // Update is called once per frame
    private void Update()
    {//플레이어 오브젝트의 collider 2d min center max 위치정보
     //플레이어의 발 위치 설정
     //플레이어 발 위치에 원생성, 바닥과 닿아있으면 ㅅrue
     //isGrounded = Physics2D.OverlapCircle(footPosition, 0.1f, groundLayer);
     //
        Ch_y_position = gameObject.transform.position.y;
        overlap_grounded= Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.3f), new Vector2(0.2f, 0.05f), 0f, groundLayer);

        if (isGrounded)
        {
            isJumping = false;
            animator.enabled = true;
            isfainted = false;
        }
        else
        {
            animator.enabled = false;
        }
        float x = Input.GetAxisRaw("Horizontal");
        if (x != 0)
        {
            animator.SetBool("run", true);
            transform.localScale = new Vector3(-1 * x, 1, 1);
        }
        else
        {

            animator.SetBool("run", false);
        }


        if (isJumping && (Ch_y_position > Prev_pos_y)&&!isfainted)
        {
            animator.enabled = false;
            spriteRenderer.sprite = Up_sprite;
        } else if (isJumping && (Ch_y_position < Prev_pos_y)&&!isfainted)
        {
            animator.enabled = false;
            spriteRenderer.sprite = Down_sprite;
        }
        else
        {

        }

        if (isGrounded && (Ch_y_position - Jump_start_position <-4.0f))
        {
            animator.enabled = false;
            spriteRenderer.sprite = Stun_sprite;
            Invoke("stand_again", 1f);
        }
        if (!isGrounded && isJumping && isStuck_left)
        {
            isfainted = true;
            animator.enabled = false;
            spriteRenderer.sprite = Collide_sprite;
        }
        if (isGrounded)
        {
            if (!Input.GetKey(KeyCode.Space) && !isJumping)
            {
                rigid2D.velocity = new Vector2(speed * x, rigid2D.velocity.y);
            }

            Ready_to_Jump(x);
        }

        Prev_pos_y = rigid2D.transform.position.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

       if (collision.gameObject == hand)
        {
            Debug.Log("충돌!!");
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == hand)
        {
            Debug.Log("충돌 지속중~");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == hand)
        {
            Debug.Log("충돌 끗!");
        }
    }
    private void LateUpdate()
    {

    }
    private void FixedUpdate()
    {



    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        ////Gizmos.DrawSphere(footPosition, 0.1f);
        //////발판
        //Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y-0.3f), new Vector2(0.2f, 0.05f));
        //////머리
        ////Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y+0.2f), new Vector2(0.4f, 0.5f));
        //////왼쪽
        //Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(0.5f, 0.5f));
        //////오른쪽
        ////Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y), new Vector2(0.05f, 0.55f));

    }
    public void Ready_to_Jump(float x)
    {
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
                Jump(3f,x);
            }
            else
            {
                Jump(Power_jump,x);

            }
        }
    }
    public void Jump(float y,float x)
    {
        if (isGrounded == true)
        {
            //rigid2D.velocity = new Vector2(x * speed*0.75f, rigid2D.velocity.y);
            //Debug.Log("이걸로 적용되야하는뎅");
            rigid2D.AddForce(Vector2.up*y*1.5f, ForceMode2D.Impulse);
            //rigid2D.AddForce(new Vector2(x * 7f, y * 1.5f), ForceMode2D.Impulse);
            Jump_start_position = gameObject.transform.position.y;

            isJumping = true;
            Power_jump = 0.0f;
            animator.enabled = true;
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.0f);
        isDelay = false;
    }
    void stand_again()
    {
        Debug.Log("다시 일어나렴");
        Jump_start_position = 0.0f;
        animator.enabled = true;
    }
}