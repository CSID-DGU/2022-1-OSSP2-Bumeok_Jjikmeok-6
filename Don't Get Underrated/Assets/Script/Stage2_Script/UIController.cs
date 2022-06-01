using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    Player_Stage2 CheckGum;

    bool itemBoxStatus = false;

    public GameObject itemBox;

    public Sprite full_Img;

    public Sprite empty_Img;

    // Start is called before the first frame update

    private void Awake()
    {
        if (GameObject.Find("Lantern") && GameObject.Find("Lantern").TryGetComponent(out Player_Stage2 BC))
            CheckGum = BC;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        { 
            if(itemBoxStatus == false)
            {
                itemBox.gameObject.SetActive(true);
                itemBoxStatus = true;
            }
            else
            {
                itemBox.gameObject.SetActive(false);
                itemBoxStatus = false;
            }
        }

        if(CheckGum.checkGumGet == true)
        {
            itemBox.GetComponent<Image>().sprite = full_Img;
        }

        if (CheckGum.checkGumGet == false)
        {
            itemBox.GetComponent<Image>().sprite = empty_Img;
        }
    }
}
