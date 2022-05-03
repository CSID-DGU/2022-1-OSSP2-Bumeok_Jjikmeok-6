using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student_Gaze_Info : MonoBehaviour
{
    // Start is called before the first frame update

    Slider slider;

    Camera selectedCamera;
    public bool CanWarp = false;

    ArrayList InterruptArray;

    ArrayList To_Stop_Interrupt;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
        InterruptArray = new ArrayList();
        To_Stop_Interrupt = new ArrayList();
    }
    void Start()
    {
        selectedCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public bool Block_HP(Vector3 TempPosition)
    {
        slider.value += Time.deltaTime / 2;
        if (slider.value >= 0.5f && InterruptArray.Count >= 1)
        {
            To_Stop_Interrupt = new ArrayList();
            foreach (var interrupt in InterruptArray)
            {
                GameObject temp_i = (GameObject)interrupt;
                IEnumerator temp_s = temp_i.GetComponent<InterruptBehaviour>().Trigger_Lazor(TempPosition);
                To_Stop_Interrupt.Add(temp_s);
                StartCoroutine(temp_s);
            }
            return true;
        }
            
        return false;
    }
    
    public void Empty_HP()
    {
        slider.value = 0;
    }
    public IEnumerator Conflict(GameObject e)
    {
        while(true)
        {
            slider.value -= Time.deltaTime / (5 - InterruptArray.Count);
            if (Input.GetMouseButtonDown(0))
                slider.value += (Time.deltaTime * 15);
            if (slider.value <= 0 || slider.value >= 1)
            {
                foreach (var stop in To_Stop_Interrupt)
                {
                    IEnumerator temp_s = (IEnumerator)stop;
                    StopCoroutine(temp_s);
                }

                gameObject.SetActive(false);
                To_Stop_Interrupt = new ArrayList();

                if (slider.value >= 1)
                {
                    slider.value = 0;
                    Destroy(e);
                }
                yield return null;
                yield break;
            }
            yield return null;
        }
    }
    void CheckInterrupt()
    {
        GameObject[] e = GameObject.FindGameObjectsWithTag("Interrupt");
        if (e != null)
        {
            InterruptArray = new ArrayList();
            foreach (var u in e)
            {
                Vector3 screenPoint = selectedCamera.WorldToViewportPoint(u.transform.position);
                bool OnScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (OnScreen)
                {
                    InterruptArray.Add(u);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckInterrupt();
    }
}
