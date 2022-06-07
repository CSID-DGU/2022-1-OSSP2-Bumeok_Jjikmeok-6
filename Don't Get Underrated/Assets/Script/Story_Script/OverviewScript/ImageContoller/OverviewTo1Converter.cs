using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewTo1Converter : MonoBehaviour
{
    GameObject Overview;
    GameObject recentImage;
    GameObject previousImage;
    int recentPhotoNum;

    // Start is called before the first frame update
    void Start()
    {
        Overview = GameObject.Find("EmptyPanel");
        recentImage = GameObject.Find("EmptyPanel").GetComponent<OverviewText>().ImageNow;
        recentPhotoNum = Overview.GetComponent<OverviewText>().PhotoNum;
    }

    // Update is called once per frame
    void Update()
    {

        if (recentPhotoNum == Overview.GetComponent<OverviewText>().PhotoNum)
        {
            recentImage.SetActive(true);
        }
        else
        {
            recentImage.SetActive(false);
            recentPhotoNum = Overview.GetComponent<OverviewText>().PhotoNum;
            recentImage = GameObject.Find("EmptyPanel").GetComponent<OverviewText>().ImageNow;
        }
    }
}