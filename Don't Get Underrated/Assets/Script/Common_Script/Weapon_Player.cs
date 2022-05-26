using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Player : MonoBehaviour
{
    [SerializeField]
    GameObject Explosion;
    Movement2D movement2D;

    protected CameraShake cameraShake;

    protected IEnumerator camera_shake;

    protected virtual void Awake()
    {
        if (TryGetComponent(out Movement2D user1))
            movement2D = user1;
        if (TryGetComponent(out CameraShake user2))
        {
            cameraShake = user2;
            cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
    public void W_MoveTo(Vector3 Origin)
    {
        movement2D.MoveTo(Origin);
    }
    public void W_MoveSpeed(float Origin)
    {
        movement2D.MoveSpeed = Origin;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision) // 트리거(콜라이더)
    {
        if (collision.gameObject != null && collision.CompareTag("Student") && collision.gameObject.TryGetComponent(out Student user1))
        {
            if (user1.get_Color() == new Color(0, 0, 1, 1))
                Destroy(gameObject);
        }
        if (collision.gameObject != null && collision.CompareTag("Enemy") && collision.TryGetComponent(out F1_Homming_Enemy user))
        {
            user.OnDie();
            Destroy(gameObject);
        }
        if (collision.CompareTag("Boss") && collision.gameObject.TryGetComponent(out Asura user3))
        {
            if (!user3.Unbeatable)
                user3.TakeDamage(3f);
            Destroy(gameObject);
        }
    }
    public void Weak_Weapon()
    {
        if (Explosion != null)
            Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
   public void Create_Explode()
   {
        Instantiate(Explosion, Vector3.zero, Quaternion.identity);
   }
    protected void Start_Camera_Shake(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (camera_shake != null)
            cameraShake.StopCoroutine(camera_shake);
        camera_shake = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
        cameraShake.StartCoroutine(camera_shake);
    }
    protected IEnumerator Start_Camera_Shake_For_Wait(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        yield return cameraShake.StartCoroutine(cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue));
    } // 되도록이면 사용 자제
}
