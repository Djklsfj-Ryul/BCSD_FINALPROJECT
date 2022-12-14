using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject gogo;

    public static int posx = 0;
    public static int posy = 0;
    public static int posz = 0;
    public GameObject STAMINA;
    public int[,] Phase_Map = new int[20,20];
    public GameObject MAP_IMAGE;

    bool Pick = false;
    RaycastHit hit;

    public GameObject Enemy_Image;
    public GameObject Big_Image;
    public GameObject[] Medium_Image;
    public GameObject[] Small_Image;

    public GameObject Main_Camera;
    GameObject PickUpObject;
    public Text MyScore;
    public Text EnScore;
    private static Respawn_Player therespawn;
    public static Enemy therespawn_E;
    private static Respawn_Enemy res;

    void Start()
    {
        Main_Camera.SetActive(true);
        therespawn = FindObjectOfType<Respawn_Player>();
        res = FindObjectOfType<Respawn_Enemy>();
    }
    void Update()
    {
        Player_Move();
        Drawing_Map();

        if(!Full_System.player_move)
        {
            gogo.SetActive(true);
        }
        else
        {
            gogo.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        EnScore.text = string.Format($"{ Full_System.enemy_point}");
        MyScore.text = string.Format($"{ Full_System.player_point}");

        if (Input.GetKeyDown(KeyCode.Space) && !Pick)
            Pick_Up();
        else if (Input.GetKeyDown(KeyCode.Space) && Pick)
            Pick_Down();
        if(Full_System.finish)
        {
            Main_Camera.SetActive(false);
        }
    }
    void Drawing_Map()
    {
        Big_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3((res.Rand_Pos[0].x * 10) - 100, (res.Rand_Pos[0].z * 10) - 100);
        Medium_Image[0].GetComponent<RectTransform>().anchoredPosition = new Vector3((res.Rand_Pos[1].x * 10) - 100, (res.Rand_Pos[1].z * 10) - 100);
        Medium_Image[1].GetComponent<RectTransform>().anchoredPosition = new Vector3((res.Rand_Pos[2].x * 10) - 100, (res.Rand_Pos[2].z * 10) - 100);
        Small_Image[0].GetComponent<RectTransform>().anchoredPosition = new Vector3((res.Rand_Pos[3].x * 10) - 100, (res.Rand_Pos[3].z * 10) - 100);
        Small_Image[1].GetComponent<RectTransform>().anchoredPosition = new Vector3((res.Rand_Pos[4].x * 10) - 100, (res.Rand_Pos[4].z * 10) - 100);
        Small_Image[2].GetComponent<RectTransform>().anchoredPosition = new Vector3((res.Rand_Pos[5].x * 10) - 100, (res.Rand_Pos[5].z * 10) - 100);
        Enemy_Image.GetComponent<RectTransform>().anchoredPosition = new Vector3((res.Rand_Pos[6].x * 10) - 100, (res.Rand_Pos[6].z * 10) - 100);
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
                Respawn_Player.MAP[posz, posx] = 0;
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
                Respawn_Player.MAP[posz, posx] = 9;
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
                Full_System.Stamina_Player = 0;
            }
        }
        STAMINA.GetComponent<Slider>().value = Full_System.Stamina_Player;
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
        if (Respawn_Player.MAP[posz + var_y, posx + var_x] == 0)
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

        for (int i = 0; i < Respawn_Player.Catridge_Big; i++)
        {
            if (PickUpObject.name == therespawn.Object_Big[i].name + "(Clone)")
            {
                if(Point(3,5))
                    Pick = false;
            }
        }

        for (int i = 0; i < Respawn_Player.Catridge_Medium; i++)
        {
            if (PickUpObject.name == therespawn.Object_Medium[i].name + "(Clone)")
            {
                if(Point(2,3))
                    Pick = false;
            }
        }
        for (int i = 0; i < Respawn_Player.Catridge_Small; i++)
        {
            if (PickUpObject.name == therespawn.Object_Small[i].name + "(Clone)")
            {
                if(Point(1,1))
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
                        Respawn_Player.MAP[(int)PickUpObject.transform.position.z + dy, (int)PickUpObject.transform.position.x + dx] = 0;
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
                        Respawn_Player.MAP[(int)PickUpObject.transform.position.z + dy, (int)PickUpObject.transform.position.x + dx] = 0;
                    }
                }
            }
        }
        for (int i = 0; i < Respawn_Player.Catridge_Small; i++)
        {
            if (PickUpObject.name == therespawn.Object_Small[i].name + "(Clone)")
            {
                Respawn_Player.MAP[(int)PickUpObject.transform.position.z, (int)PickUpObject.transform.position.x] = 0;
            }
        }
    }
    bool Point(int num, int large)
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
                        if (Respawn_Player.MAP[z + dy, x + dx + num] == 0) check++;
                    }
                }
                if (check == (large * large))
                    for (int dy = -(num - 1); dy < num; dy++)
                    {
                        for (int dx = -(num - 1); dx < num; dx++)
                        {
                            if (dx == 0 && dy == 0) Respawn_Player.MAP[z + dy, x + dx + num] = 8;
                            else Respawn_Player.MAP[z + dy, x + dx + num] = 7;
                        }
                    }
                else
                {
                    Debug.Log("Can't Pick Down");
                    return false;
                }
                PickUpObject.transform.position = new Vector3(x + num,y,z);
                break;
            case 90:
                for (int dy = -(num - 1); dy < num; dy++)
                {
                    for (int dx = -(num - 1); dx < num; dx++)
                    {
                        if (Respawn_Player.MAP[z + dy - num, x + dx] == 0) check++;
                    }
                }
                if (check == (large * large))
                    for (int dy = -(num - 1); dy < num; dy++)
                    {
                        for (int dx = -(num - 1); dx < num; dx++)
                        {
                            if (dx == 0 && dy == 0) Respawn_Player.MAP[z + dy - num, x + dx] = 8;
                            else Respawn_Player.MAP[z + dy - num, x + dx] = 7;
                        }
                    }
                else
                {
                    Debug.Log("Can't Pick Down");
                    return false;
                }
                PickUpObject.transform.position = new Vector3(x, y, z - num);
                break;
            case 180:
                for (int dy = -(num - 1); dy < num; dy++)
                {
                    for (int dx = -(num - 1); dx < num; dx++)
                    {
                        if (Respawn_Player.MAP[z + dy - num, x + dx] == 0) check++;
                    }
                }
                if (check == (large * large))
                    for (int dy = -(num - 1); dy < num; dy++)
                    {
                        for (int dx = -(num - 1); dx < num; dx++)
                        {
                            if (dx == 0 && dy == 0) Respawn_Player.MAP[z + dy, x + dx - num] = 8;
                            else Respawn_Player.MAP[z + dy, x + dx - num] = 7;
                        }
                    }
                else
                {
                    Debug.Log("Can't Pick Down");
                    return false;
                }
                PickUpObject.transform.position = new Vector3(x - num, y, z);
                break;
            case 270:
                for (int dy = -(num - 1); dy < num; dy++)
                {
                    for (int dx = -(num - 1); dx < num; dx++)
                    {
                        if (Respawn_Player.MAP[z + dy - num, x + dx] == 0) check++;
                    }
                }
                if (check == (large * large))
                    for (int dy = -(num - 1); dy < num; dy++)
                    {
                        for (int dx = -(num - 1); dx < num; dx++)
                        {
                            if (dx == 0 && dy == 0) Respawn_Player.MAP[z + dy + num, x + dx] = 8;
                            else Respawn_Player.MAP[z + dy + num, x + dx] = 7;
                        }
                    }
                else
                {
                    Debug.Log("Can't Pick Down");
                    return false;
                }
                PickUpObject.transform.position = new Vector3(x, y, z + num);
                break;
        }
        PickUpObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX |
                                                             RigidbodyConstraints.FreezeRotationZ |
                                                             RigidbodyConstraints.FreezeRotationY;
        return true;
    }
}
