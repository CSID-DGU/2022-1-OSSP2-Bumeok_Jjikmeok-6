using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject Lazor;



    private void Awake()
    {

    }
    public IEnumerator Trigger_Lazor(Vector3 tempPosition) // 이 쪽은 과제, 시험 등이 플레이어랑 경쟁하기 위해 쏘는 레이저 빔 코드
    {
        while(true)
        {
            Lazor.GetComponent<Movement2D_Variation>().MoveTo(new Vector3(tempPosition.x - transform.position.x,
                         tempPosition.y - transform.position.y, 0));
            Instantiate(Lazor, transform.position, Quaternion.identity);
            yield return null;
        }
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
