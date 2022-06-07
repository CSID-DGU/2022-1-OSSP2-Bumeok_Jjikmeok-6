using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2to3Converter : MonoBehaviour
{
    GameObject Overview;
    GameObject recentImage;
    GameObject previousImage;
    int recentPhotoNum;

    // Start is called before the first frame update
    void Start()
    {
        Overview = GameObject.Find("EmptyPanel");
        recentImage = GameObject.Find("EmptyPanel").GetComponent<Stage2to3Text>().ImageNow;
        recentPhotoNum = Overview.GetComponent<Stage2to3Text>().PhotoNum;
    }

    // Update is called once per frame
    void Update()
    {

        if (recentPhotoNum == Overview.GetComponent<Stage2to3Text>().PhotoNum)
        {
            recentImage.SetActive(true);
        }
        else
        {
            recentImage.SetActive(false);
            recentPhotoNum = Overview.GetComponent<Stage2to3Text>().PhotoNum;
            recentImage = GameObject.Find("EmptyPanel").GetComponent<Stage2to3Text>().ImageNow;
        }
    }
}