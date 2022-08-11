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
    private Respawn_Player therespawn;
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
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            gameObject.transform.Rotate(0, 90, 0);
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
            
            Debug.Log(PickUpObject);
            Find();

            PickUpObject.GetComponent<Transform>().transform.position = new Vector3(posx, posy+2.5f, posz);
            PickUpObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
    void Find()
    {
        for (int i = 0; i < therespawn.Catridge_Big; i++)
        {
            if (PickUpObject == therespawn.Object_Big[i])
                for (int dimension = 1; dimension <= therespawn.MAP.Rank; dimension++)
                    Console.WriteLine("   Dimension {0}: {1,3}", dimension, therespawn.MAP.GetUpperBound(dimension - 1) + 1);
            therespawn.MAP[]
        }
        for (int i = 0; i < therespawn.Catridge_Medium; i++)
        {
            if (PickUpObject == therespawn.Object_Medium[i])
        }
        for (int i = 0; i < therespawn.Catridge_Small; i++)
        {
            if (PickUpObject == therespawn.Object_Small[i])

        }
    }
}
