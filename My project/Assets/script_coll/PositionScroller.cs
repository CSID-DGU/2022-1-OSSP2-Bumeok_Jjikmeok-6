using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionScroller : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float scrollRange = 9.9f;
    
    [SerializeField]
    float moveSpeed = 3.0f;

    [SerializeField]
    Vector3 MoveDirection = Vector3.down; // æÓ∂ª∞‘ ∫∏∏È ¥‹¿ß∫§≈Õ¿Œ º¿¿Ã¡ˆ »Ï!
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += MoveDirection * moveSpeed * Time.deltaTime;
        if (transform.position.y <= -scrollRange)
        {
            transform.position = target.position + Vector3.up * scrollRange;
        }
    }

}
