using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    GameObject waterGun;

    [SerializeField]
    GameObject Boom;

    [SerializeField]
    int BoomCount = 3;

    [SerializeField]
    TextMeshProUGUI BoomCountText;

    private void Awake()
    {
        BoomCountText.text = "��ź : " + BoomCount;
        BoomCountText.color = new Color(BoomCountText.color.r, BoomCountText.color.g, BoomCountText.color.b, 0);
    }
  
    void Start()
    {
        StartCoroutine("FadeText");
    }
    IEnumerator FadeText()
    {
        while(BoomCountText.color.a < 1.0f)
        {
            BoomCountText.color = new Color(BoomCountText.color.r, BoomCountText.color.g, BoomCountText.color.b, BoomCountText.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
    }
    public void StartFiring()
    {
        StartCoroutine("Water_firing");
    }
    public void StopFiring()
    {
        StopCoroutine("Water_firing");
    }
    public void StartBoom()
    {
       if  (BoomCount > 0)
       {
            BoomCount--;
            BoomCountText.text = "��ź : " + BoomCount;
            Instantiate(Boom, transform.position, Quaternion.identity);
            // �׳� transform.position�� �� �� �ִ� ������ �� weapon Ŭ������ Player
            // ���̷���Ű�� �ҼӵǾ��ֱ� �����̴�.
       }
    }
    IEnumerator Water_firing()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(waterGun, transform.position, Quaternion.identity);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate() // ���⼭ ��ġ�� �̵���Ű�� �ű�
    {

    }
}
