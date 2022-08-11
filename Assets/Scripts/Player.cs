using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int posx = 0;
    private int posy = 0;
    private int posz = 0;

    bool Pick = false;
    RaycastHit hit;

    GameObject PickUpObject;
    private static Respawn_Player therespawn;
    void Start()
    {
        therespawn = FindObjectOfType<Respawn_Player>();
    }
    void Update()
    {
        Player_Move();
        Pick_Up();
    }
    void Player_Move()
    {
        posx = (int)gameObject.transform.position.x;
        posy = (int)gameObject.transform.position.y;
        posz = (int)gameObject.transform.position.z;
        int angle = (int)gameObject.transform.rotation.eulerAngles.y;

        if (Input.GetKeyDown(KeyCode.W)) 
        {
            therespawn.MAP[posz, posx] = 0;
            switch (angle)
            {
                case 0:
                    if (Object_Check(posx, posz, 1, 0))
                    {
                        if (Pick)
                        {
                            PickUpObject.transform.position = new Vector3(posx + 1, posy + 2.5f, posz);
                        }
                        gameObject.transform.position = new Vector3(posx + 1, posy, posz);
                        posx++;
                    }
                    break;
                case 90:
                    if (Object_Check(posx, posz, 0, -1))
                    {
                        if (Pick)
                        {
                            PickUpObject.transform.position = new Vector3(posx, posy + 2.5f, posz - 1);
                        }
                        gameObject.transform.position = new Vector3(posx, posy, posz - 1);
                        posz--;
                    }
                    break;
                case 180:
                    if (Object_Check(posx, posz, -1, 0))
                    {
                        if (Pick)
                        {
                            PickUpObject.transform.position = new Vector3(posx - 1, posy + 2.5f, posz);
                        }
                        gameObject.transform.position = new Vector3(posx - 1, posy, posz);
                        posx--;
                    } 
                    break;
                case 270:
                    if (Object_Check(posx, posz, 0, 1))
                    {
                        if (Pick)
                        {
                            PickUpObject.transform.position = new Vector3(posx, posy + 2.5f, posz + 1);
                        }
                        gameObject.transform.position = new Vector3(posx, posy, posz + 1);
                        posz++;
                    }
                    break;
            }
            therespawn.MAP[posz, posx] = 9;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.transform.Rotate(0,-90,0);
            if(Pick)
            {
                PickUpObject.transform.Rotate(0,-90,0);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            gameObject.transform.Rotate(0, 90, 0);
            if (Pick)
            {
                PickUpObject.transform.Rotate(0, +90, 0);
            }
        }
    }
    bool Object_Check(int posx, int posz, int var_x, int var_y)
    {
        Debug.Log(posz + "," + posx);
        Debug.Log(var_y + "," + var_x);

        if (therespawn.MAP[posz + var_y, posx + var_x] == 0)
            return true;
        else
            return false;

    }
    void Pick_Up()
    {
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.right * 10, Color.red);
        if(Physics.Raycast(gameObject.transform.position, gameObject.transform.right, out hit, 1.0f) && Input.GetKeyDown(KeyCode.Space))
        {
            Pick = true;
            PickUpObject = hit.transform.gameObject;

            Remove_Point();

            PickUpObject.GetComponent<Transform>().transform.position = new Vector3(posx, posy+2.5f, posz);
            PickUpObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | 
                                                                 RigidbodyConstraints.FreezeRotationX | 
                                                                 RigidbodyConstraints.FreezeRotationZ | 
                                                                 RigidbodyConstraints.FreezeRotationY;
        }
    }
    void Remove_Point()
    {
        int y = therespawn.MAP.GetLength(0);
        int x = therespawn.MAP.GetLength(1);

        for (int i = 0; i < Respawn_Player.Catridge_Big; i++)
        {
            if (PickUpObject.name == therespawn.Object_Big[i].name + "(Clone)")
            {
                for (int dy = -2; dy < 3; dy++)
                {
                    for (int dx = -2; dx < 3; dx++)
                    {
                        therespawn.MAP[(int)PickUpObject.transform.position.z + dy, (int)PickUpObject.transform.position.x + dx] = 0;
                    }
                }
            } 
        }
        for (int i = 0; i < Respawn_Player.Catridge_Medium; i++)
        {
            if (PickUpObject.name == therespawn.Object_Medium[i].name + "(Clone)")
            {
                for (int dy = -1; dy < 2; dy++)
                {
                    for (int dx = -1; dx < 2; dx++)
                    {
                        therespawn.MAP[(int)PickUpObject.transform.position.z + dy, (int)PickUpObject.transform.position.x + dx] = 0;
                    }
                }
            }
        }
        for (int i = 0; i < Respawn_Player.Catridge_Small; i++)
        {
            if (PickUpObject.name == therespawn.Object_Small[i].name + "(Clone)")
            {
                therespawn.MAP[(int)PickUpObject.transform.position.z, (int)PickUpObject.transform.position.x] = 0;
            }
        }
    }
}
