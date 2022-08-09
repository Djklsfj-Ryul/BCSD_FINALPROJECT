using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Respawn
{
    new void Start()
    {
        POINTING point = new POINTING(Overriding_Map);
        MAP = point();
        string a1 = "";
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                a1 = a1 + MAP[j, i];
            }
            Debug.Log(a1);
            a1 = "";
        }
    }
    void Update()
    {
        Player_Move();
    }
    void Player_Move()
    {
        int posx = (int)gameObject.transform.position.x;
        int posy = (int)gameObject.transform.position.y;
        int posz = (int)gameObject.transform.position.z;
        int angle = (int)gameObject.transform.rotation.eulerAngles.y;

        if (Input.GetKeyDown(KeyCode.W))
        {
            switch (angle)
            {
                case 0:
                    gameObject.transform.position = new Vector3(posx + 1, posy, posz);
                    break;
                case 90:
                    gameObject.transform.position = new Vector3(posx, posy, posz - 1);
                    break;
                case 180:
                    gameObject.transform.position = new Vector3(posx - 1, posy, posz);
                    break;
                case 270:
                    gameObject.transform.position = new Vector3(posx, posy, posz + 1);
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.transform.Rotate(0,-90,0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            gameObject.transform.Rotate(0, 90, 0);
        }
    }
}
