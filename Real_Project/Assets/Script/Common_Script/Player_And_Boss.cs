using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_And_Boss : MonoBehaviour, Life_Of_Basic
{
    // Start is called before the first frame update

    protected SpriteRenderer spriteRenderer;

    private float percent;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void OnDie()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        
    }

    protected void Launch_Weapon_For_Move(GameObject weapon, Vector3 target, Vector3 self)
    {
        weapon.GetComponent<Movement2D_Variation>().MoveTo(new Vector3(target.x - self.x,
                        target.y - self.y, 0));
        Instantiate(weapon, self, Quaternion.identity);
    }
   // GameObject L5 = Instantiate(Boss_Weapon[2], new Vector3(0, 3, 0), Quaternion.Euler(new Vector3(0, 0, -9)));
    protected void Launch_Weapon_For_Move(GameObject weapon, Vector3 target, Quaternion Degree, float Destroy_Time)
    {
        weapon.GetComponent<Movement2D>().MoveTo(target);
        GameObject e = Instantiate(weapon, transform.position, Degree);
        Destroy(e, Destroy_Time);
    }

    protected void Launch_Weapon_For_Still(GameObject weapon, Vector3 instantiate_position, Quaternion Degree, float Destroy_Time)
    {
        GameObject e = Instantiate(weapon, instantiate_position, Degree);
        Destroy(e, Destroy_Time);
    }
    protected IEnumerator Change_Color_Return_To_Origin(Color Origin_C, Color Change_C, float ratio, bool is_Continue)
    { 
        while (true)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * ratio;
                spriteRenderer.color = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * ratio;
                spriteRenderer.color = Color.Lerp(Change_C, Origin_C, percent);
                yield return null;
            }
            if (!is_Continue)
                break;
        }
    }
    protected IEnumerator Change_Color_Temporary(Color Origin_C, Color Change_C, float ratio, float Wait_Second, GameObject Effect)
    {
        if (Effect != null)
            Instantiate(Effect, transform.position, Quaternion.identity);
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * ratio;
            spriteRenderer.color = Color.Lerp(Origin_C, Change_C, percent);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        yield return YieldInstructionCache.WaitForSeconds(Wait_Second);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
