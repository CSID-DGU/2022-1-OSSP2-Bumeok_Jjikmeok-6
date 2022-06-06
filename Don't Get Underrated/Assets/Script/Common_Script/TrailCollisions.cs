using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailCollisions : MonoBehaviour // 보스가 그리는 트레일 렌더러에 물리적인 요소를 주도록 하는 스크립트
{
    TrailRenderer myTrail;
    EdgeCollider2D myCollider;

    static List<EdgeCollider2D> unusedColliders = new List<EdgeCollider2D>();

    void Awake()
    {
        if (TryGetComponent(out TrailRenderer TR))
            myTrail = TR;
    }
    public void Draw_Collision_Line()
    {
        myCollider = GetVaildCollider();
    }
    EdgeCollider2D GetVaildCollider()
    {
        EdgeCollider2D validCollider;
        if (unusedColliders.Count > 0)
        {
            validCollider = unusedColliders[0];
            validCollider.enabled = true;
            unusedColliders.RemoveAt(0);
        }
        else
            validCollider = new GameObject("TrailCollider", typeof(EdgeCollider2D)).GetComponent<EdgeCollider2D>();
        return validCollider;
    }

    void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        //avoid having default points at (-.5,0),(.5,0)
        if (trail.positionCount == 0)
        {
            points.Add(transform.position);
            points.Add(transform.position);
        }
        else for (int position = 0; position < trail.positionCount; position++)
            {
                //ignores z axis when translating vector3 to vector2
                points.Add(trail.GetPosition(position));
            }
        collider.SetPoints(points);
    }

    void Update()
    {
        if (myCollider != null && myTrail != null)
            SetColliderPointsFromTrail(myTrail, myCollider);
    }
    void OnDestroy()
    {
        if (myCollider != null)
        {
            myCollider.enabled = false;
            unusedColliders.Add(myCollider);
        }
    }
}