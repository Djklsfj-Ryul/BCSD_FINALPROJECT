using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Manage : MonoBehaviour
{
    public GameObject CoverImage;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void OnClickStartButton()
    {
        CoverImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
