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

    public GameObject Main_Camera;
    GameObject PickUpObject;
    private static Respawn_Player therespawn;
    void Start()
    {
        Main_Camera.SetActive(true);
        therespawn = FindObjectOfType<Respawn_Player>();
    }
    void Update()
    {
        Player_Move();
        if (Input.GetKeyDown(KeyCode.Space) && !Pick)
            Pick_Up();
        else if (Input.GetKeyDown(KeyCode.Space) && Pick)
            Pick_Down();
    }
    void Player_Move()
    {
        posx = (int)Mathf.Ceil(gameObject.transform.position.x);
        posy = (int)Mathf.Ceil(gameObject.transform.position.y);
        posz = (int)Mathf.Ceil(gameObject.transform.position.z);
        int angle = (int)gameObject.transform.rotation.eulerAngles.y;

        if (Full_System.player_move)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log(Full_System.Stamina_Player);
                therespawn.MAP[posz, posx] = 0;
                switch (angle)
                {
                    case 0:
                        if (Object_Check(posx, posz, 1, 0))
                        {
                            if (Pick)
                            {
                                PickUpObject.transform.position = new Vector3(posx + 1, posy + 3f, posz);
                                gameObject.transform.position = new Vector3(posx + 1, posy, posz);
                                Full_System.Stamina_Player -= 2;
                                posx++;
                            }
                            else
                            {
                                gameObject.transform.position = new Vector3(posx + 1, posy, posz);
                                Full_System.Stamina_Player--;
                                posx++;
                            }
                        }
                        break;
                    case 90:
                        if (Object_Check(posx, posz, 0, -1))
                        {
                            if (Pick)
                            {
                                PickUpObject.transform.position = new Vector3(posx, posy + 3f, posz - 1);
                                gameObject.transform.position = new Vector3(posx, posy, posz - 1);
                                Full_System.Stamina_Player -= 2;
                                posz--;
                            }
                            else
                            {
                                gameObject.transform.position = new Vector3(posx, posy, posz - 1);
                                Full_System.Stamina_Player--;
                                posz--;
                            }
                        }
                        break;
                    case 180:
                        if (Object_Check(posx, posz, -1, 0))
                        {
                            if (Pick)
                            {
                                PickUpObject.transform.position = new Vector3(posx - 1, posy + 3f, posz);
                                gameObject.transform.position = new Vector3(posx - 1, posy, posz);
                                Full_System.Stamina_Player -= 2;
                                posx--;
                            }
                            else
                            {
                                gameObject.transform.position = new Vector3(posx - 1, posy, posz);
                                Full_System.Stamina_Player--;
                                posx--;
                            }
                        }
                        break;
                    case 270:
                        if (Object_Check(posx, posz, 0, 1))
                        {
                            if (Pick)
                            {
                                PickUpObject.transform.position = new Vector3(posx, posy + 3f, posz + 1);
                                gameObject.transform.position = new Vector3(posx, posy, posz + 1);
                                Full_System.Stamina_Player -= 2;
                                posz++;
                            }
                            else
                            {
                                gameObject.transform.position = new Vector3(posx, posy, posz + 1);
                                Full_System.Stamina_Player--;
                                posz++;
                            }
                        }
                        break;
                }
                therespawn.MAP[posz, posx] = 9;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                gameObject.transform.Rotate(0, -90, 0);
                if (Pick)
                {
                    PickUpObject.transform.Rotate(0, -90, 0);
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
            if (Input.GetKeyUp(KeyCode.Y))
            {
                Debug.Log("턴을 종료하겠습니까?");
                if (Input.GetKeyDown(KeyCode.Y))
                    Full_System.Stamina_Player = 0;
            }
        }
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Long_Sight)
            {
                GameObject.Find("Main_Camera").transform.position = new Vector3(posx, posy, posz);
                GameObject.Find("Main_Camera").transform.Rotate(-20, 0, 0);
                Long_Sight = false;
            }
            else
            {
                GameObject.Find("Main_Camera").transform.position = new Vector3(posx, posy + 3.5f, posz -3.6f);
                GameObject.Find("Main_Camera").transform.Rotate(20, 0, 0);
                Long_Sight = true;
            }
        }*/
    }
    bool Object_Check(int posx, int posz, int var_x, int var_y)
    {
        //Debug.Log(posy +","+ posx);
        if (therespawn.MAP[posz + var_y, posx + var_x] == 0)
            return true;
        else
            return false;

    }
    void Pick_Up()
    {
        if(Physics.Raycast(gameObject.transform.position, gameObject.transform.right, out hit, 1.0f))
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
    void Pick_Down()
    {
        int x = (int)gameObject.transform.position.x;
        int z = (int)gameObject.transform.position.z;
        bool[] Clear = new bool[] { true, true, true };


        for (int i = 0; i < Respawn_Player.Catridge_Big; i++)
        {
            if (PickUpObject.name == therespawn.Object_Big[i].name + "(Clone)")
            {
                Point(3,5);
                Pick = false;
            }
        }

        for (int i = 0; i < Respawn_Player.Catridge_Medium; i++)
        {
            if (PickUpObject.name == therespawn.Object_Medium[i].name + "(Clone)")
            {
                Point(2,3);
                Pick = false;
            }
        }
        for (int i = 0; i < Respawn_Player.Catridge_Small; i++)
        {
            if (PickUpObject.name == therespawn.Object_Small[i].name + "(Clone)")
            {
                Point(1,1);
                Pick = false;
            }
        }
    }
    void Remove_Point()
    {
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
    void Point(int num, int large)
    {
        int angle = (int)gameObject.transform.rotation.eulerAngles.y;
        int x = (int)gameObject.transform.position.x;
        int z = (int)gameObject.transform.position.z;
        int y = (int)gameObject.transform.position.y;
        int check = 0;
        switch (angle)
        {
            case 0:
                for (int dy = -(num - 1); dy < num; dy++)
                {
                    for (int dx = -(num - 1); dx < num; dx++)
                    {
                        if (therespawn.MAP[z + dy, x + dx + num] == 0) check++;
                        else
                        {
                            Debug.Log("Can't Pick Down");
                            return;
                        }
                    }
                }
                if (check == (large * large))
                    for (int dy = -(num - 1); dy < num; dy++)
                    {
                        for (int dx = -(num - 1); dx < num; dx++)
                        {
                            if (dx == 0 && dy == 0) therespawn.MAP[z + dy, x + dx + num] = 8;
                            else                    therespawn.MAP[z + dy, x + dx + num] = 7;
                        }
                    }
                PickUpObject.transform.position = new Vector3(x + num,y,z);
                break;
            case 90:
                for (int dy = -(num - 1); dy < num; dy++)
                {
                    for (int dx = -(num - 1); dx < num; dx++)
                    {
                        if (therespawn.MAP[z + dy - num, x + dx] == 0) check++;
                        else
                        {
                            Debug.Log("Can't Pick Down");
                            return;
                        }
                    }
                }
                if (check == (large * large))
                    for (int dy = -(num - 1); dy < num; dy++)
                    {
                        for (int dx = -(num - 1); dx < num; dx++)
                        {
                            if (dx == 0 && dy == 0) therespawn.MAP[z + dy - num, x + dx] = 8;
                            else                    therespawn.MAP[z + dy - num, x + dx] = 7;
                        }
                    }
                PickUpObject.transform.position = new Vector3(x, y, z - num);
                break;
            case 180:
                for (int dy = -(num - 1); dy < num; dy++)
                {
                    for (int dx = -(num - 1); dx < num; dx++)
                    {
                        if (therespawn.MAP[z + dy - num, x + dx] == 0) check++;
                        else
                        {
                            Debug.Log("Can't Pick Down");
                            return;
                        }
                    }
                }
                if (check == (large * large))
                    for (int dy = -(num - 1); dy < num; dy++)
                    {
                        for (int dx = -(num - 1); dx < num; dx++)
                        {
                            if (dx == 0 && dy == 0) therespawn.MAP[z + dy, x + dx - num] = 8;
                            else                    therespawn.MAP[z + dy, x + dx - num] = 7;
                        }
                    }
                PickUpObject.transform.position = new Vector3(x - num, y, z);
                break;
            case 270:
                for (int dy = -(num - 1); dy < num; dy++)
                {
                    for (int dx = -(num - 1); dx < num; dx++)
                    {
                        if (therespawn.MAP[z + dy - num, x + dx] == 0) check++;
                        else
                        {
                            Debug.Log("Can't Pick Down");
                            return;
                        }
                    }
                }
                if (check == (large * large))
                    for (int dy = -(num - 1); dy < num; dy++)
                    {
                        for (int dx = -(num - 1); dx < num; dx++)
                        {
                            if (dx == 0 && dy == 0) therespawn.MAP[z + dy + num, x + dx] = 8;
                            else                    therespawn.MAP[z + dy + num, x + dx] = 7;
                        }
                    }
                PickUpObject.transform.position = new Vector3(x, y, z + num);
                break;
        }
        PickUpObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX |
                                                             RigidbodyConstraints.FreezeRotationZ |
                                                             RigidbodyConstraints.FreezeRotationY;
        string a1 = "";
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                a1 = a1 +   therespawn.MAP[j, i];
            }
            //Debug.Log(a1);
            a1 = "";
        }
    }
}
