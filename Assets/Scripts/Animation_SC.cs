using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_SC : MonoBehaviour
{
    public Animation ani;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            ani.CrossFade("Walk",0.1f);
        }
    }
}
