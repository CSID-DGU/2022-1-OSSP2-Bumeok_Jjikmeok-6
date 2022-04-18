using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Fall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject whiteLine;

    [SerializeField]
    GameObject meteor;

    [SerializeField]
    StageData stageData;
    public void Awake()
    {
        StartCoroutine("MeteorFall");
    }

    IEnumerator MeteorFall()
    {
        while (true)
        {
            float positionX = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);
            GameObject alertLine = Instantiate(whiteLine, new Vector3(positionX, 0, 0), Quaternion.identity);
            yield return new WaitForSeconds(2.0f);
            Destroy(alertLine);

            Instantiate(meteor, new Vector3(positionX, stageData.LimitMax.y + 1.0f, 0.0f), Quaternion.identity);
            yield return new WaitForSeconds(3.0f);
            
        }
        //yield return null;
        //float positionX = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);
        //Instantiate(whiteLine, new Vector3(positionX, stageData.LimitMax.y + 1.0f, 0.0f), Quaternion.identity);
        //yield return new WaitForSeconds(2.0f);
        //Destroy(whiteLine);
        //yield return null;

        //Instantiate(meteor, new Vector3(positionX, stageData.LimitMax.y + 1.0f, 0.0f), Quaternion.identity);
        //yield return new WaitForSeconds(4.0f);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
