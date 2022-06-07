using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutroConverter : MonoBehaviour
{
    GameObject Overview;
    GameObject recentImage;
    GameObject previousImage;
    int recentPhotoNum;

    // Start is called before the first frame update
    void Start()
    {
        Overview = GameObject.Find("EmptyPanel");
        recentImage = GameObject.Find("EmptyPanel").GetComponent<Outro>().ImageNow;
        recentPhotoNum = Overview.GetComponent<Outro>().PhotoNum;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(recentPhotoNum == Overview.GetComponent<Outro>().PhotoNum)
        {
            recentImage.SetActive(true);
        }
        else
        {
            recentImage.SetActive(false);
            recentPhotoNum = Overview.GetComponent<Outro>().PhotoNum;
            recentImage = GameObject.Find("EmptyPanel").GetComponent<Outro>().ImageNow;
        }
    }
}