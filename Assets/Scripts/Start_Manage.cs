using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Manage : MonoBehaviour
{
    public GameObject CoverImage;
    public GameObject Camera;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void OnClickStartButton()
    {
        CoverImage.SetActive(false);
        Camera.SetActive(false);
    }

    bool tri = true;

    // Update is called once per frame
    void Update()
    {
        if(Full_System.finish && tri)
        {
            Camera.GetComponent<RectTransform>().anchoredPosition = new Vector3(1500,0,-350);
            Camera.SetActive(true);
            tri = false;
        }
    }
}
