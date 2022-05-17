using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPosition : MonoBehaviour
{
    IEnumerator move;

    private void Awake()
    {
        move = null;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Start_Down_Or_UP(bool is_Down, int Change_Y)
    {
        if (is_Down && gameObject.activeInHierarchy)
        {
            if (move == null)
            {
                transform.position = new Vector3(-7, Change_Y, 0);
                Debug.Log(transform.position);
                move = Move(-7, Change_Y, -0.5f, 0.2f);
                StartCoroutine(move);
            }
        }
        else
        {
            if (move == null)
            {
                transform.position = new Vector3(43, Change_Y, 0);
                move = Move(43, Change_Y, 0.5f, 0.2f);
                StartCoroutine(move);
            }
        }
    }
    
    public void Stop_Down_Or_UP()
    {
        if (move != null)
        {
            StopCoroutine(move);
            move = null;
        }
            
    }
    IEnumerator Move(int Change_X, int Change_Y, float flag, float time_persist)
    {
        float percent;
        while (true)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                transform.position = Vector3.Lerp(new Vector3(Change_X, Change_Y, 0), new Vector3(Change_X, Change_Y + flag, 0), percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                transform.position = Vector3.Lerp(new Vector3(Change_X, Change_Y + flag, 0), new Vector3(Change_X, Change_Y, 0), percent);
                yield return null;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
