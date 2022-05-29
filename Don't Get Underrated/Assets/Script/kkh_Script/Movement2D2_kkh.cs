using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D2_kkh : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite jump_sprite;
    public Sprite Stun_sprite;
    public Sprite Collide_sprite;
    public Sprite Up_sprite;
    public Sprite stop_sprite;
    public Sprite Down_sprite;
    private bool is_bounce = false;
    private bool is_normal = false;
    private float speed = 2.5f;
    [SerializeField]
    private float jumpForce = 5.0f;
    private Rigidbody2D rigid2D;
    [SerializeField]
    private float Power_jump = 2.0f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask wallLayer;
    public PhysicsMaterial2D bounceMat, normalMat;
    public float Ch_y_position;
    public float Jump_start_position;
    private bool isJumping = false;
    private bool isMoving = false;
    private bool is_ready = false;
    public Animator animator;
    private BoxCollider2D Box;
    public GameObject Ground;
    public GameObject Wall;
    public GameObject Ice;
    public float Prev_pos_y;
    public float Now_pos_y;
    private bool overlap_grounded;
    private bool isStuck;
    private bool isStuck2;
    private void Awake()
    {
        Ground = GameObject.Find("Grid");
        Ground = Ground.transform.GetChild(0).gameObject;
        Wall = GameObject.Find("Grid");
        Wall = Wall.transform.GetChild(1).gameObject;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    private void Update()
    {//플레이어 오브젝트의 collider 2d min center max 위치정보
     //플레이어의 발 위치 설정
     //플레이어 발 위치에 원생성, 바닥과 닿아있으면 ㅅrue
     //isGrounded = Physics2D.OverlapCircle(footPosition, 0.1f, groundLayer);
     overlap_grounded= Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.3f), 0.05f, groundLayer);
     isStuck = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(0.4f, 0.5f), 0f, wallLayer);
     isStuck2 = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(0.4f, 0.5f), 0f, groundLayer);

        float x = Input.GetAxisRaw("Horizontal");
        Ch_y_position = gameObject.transform.position.y;

        if (!isJumping&&isMoving&&!is_ready)
        {
            rigid2D.velocity = new Vector2(speed * x, rigid2D.velocity.y);
            if (x != 0)
            {
                animator.SetBool("run", true);
                transform.localScale = new Vector3(-1 * x, 1, 1);
            }
            else
            {

                animator.SetBool("run", false);
            }
        }



        if (!isJumping)
        {

            Ready_to_Jump(x);
        }
        Prev_pos_y = rigid2D.transform.position.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (overlap_grounded)
        {
            Debug.Log("닿음..");
        }
        if ((isStuck2||isStuck) && isJumping && !overlap_grounded && (collision.gameObject == Wall))
        {
            rigid2D.sharedMaterial = bounceMat;
        }
        if ((isStuck2||isStuck)&&isJumping && !overlap_grounded && (collision.gameObject==Ground))
        {
            rigid2D.sharedMaterial = bounceMat; 
        }
        else if ((!isStuck)&&isJumping && !overlap_grounded && collision.gameObject == Ground)
        {
            Debug.Log("overlap 아니고 아슬아슬하게 닿은 경우");
            isJumping = false;
            isMoving = true;
            is_ready = false;
            rigid2D.sharedMaterial = bounceMat; 
        }
        else if(collision.gameObject == Ground &&overlap_grounded) {
            Debug.Log("그냥 바닥인경우");
            isJumping = false;
            isMoving = true;
            is_ready = false;
        }
        else
        {
            Debug.Log("착지후...isMoving: " + isMoving + "isJumping: " + isJumping + "is_ready" + is_ready + "isStuck:" + isStuck + "isStuck2:" + isStuck2);
            Debug.Log("overlap_: " + overlap_grounded);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //if ((isStuck2 || isStuck) && isJumping && !overlap_grounded && (collision.gameObject == Ground))
        //{
        //    rigid2D.sharedMaterial = bounceMat;
        //}
        //if ((isStuck2 || isStuck) && isJumping && !overlap_grounded && (collision.gameObject == Wall))
        //{
        //    rigid2D.sharedMaterial = bounceMat;
        //}
       //if ((!isStuck) && isJumping && !overlap_grounded && collision.gameObject == Ground)
       // {
       //     isMoving = true;
       // }
       // else if(collision.gameObject==Ground&&overlap_grounded)
       // {

       //     isMoving = true;
       // }
        //float x = Input.GetAxisRaw("Horizontal");
        //if (collision.gameObject == Wall&&!overlap_grounded)
        //{
        //    rigid2D.sharedMaterial = bounceMat;
        //}
        //else if (collision.gameObject == Ground&&!overlap_grounded&&isJumping)
        //{
        //    rigid2D.sharedMaterial = bounceMat;
        //}
        //else
        //{

        //    isJumping = false;
        //    isMoving = true;
        //}

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject == Wall)
        //{
        //    isMoving = false;
        //    isJumping = true;
        //    is_ready = false;
        //}
        //if (collision.gameObject == Ground)
        //{
        //    isJumping = true;
        //    isMoving = false;
        //    is_ready = false;

        //}
        
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
        //Gizmos.DrawSphere(footPosition, 0.1f);
        ////발판
        Gizmos.DrawSphere(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y-0.3f), 0.05f);
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(0.4f, 0.5f));

    }
    public void Ready_to_Jump(float x)
    {
        isJumping = false;
        if (!isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                is_ready = true;
                rigid2D.velocity = new Vector2(0, rigid2D.velocity.y);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                is_ready = true;
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
                is_ready = true;
                if (Power_jump <= 3f)
                {
                    Jump(3f, x);
                }
                else
                {
                    Jump(Power_jump, x);

                }
                
            }
        }
 
    }
    public void Jump(float y, float x)
    {
            isMoving = false;
            //rigid2D.velocity = new Vector2(x * speed*0.75f, rigid2D.velocity.y);
            //Debug.Log("이걸로 적용되야하는뎅");
            //rigid2D.AddForce(Vector2.up * y * 1.5f, ForceMode2D.Impulse);
            rigid2D.AddForce(new Vector2(x*speed*1.5f , y * 1.5f), ForceMode2D.Impulse);
            Jump_start_position = gameObject.transform.position.y;

            isJumping = true;
            Power_jump = 0.0f;
            animator.enabled = true;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.0f);
    }
    void stand_again()
    {
        Debug.Log("다시 일어나렴");
        Jump_start_position = 0.0f;
        animator.enabled = true;
    }
}