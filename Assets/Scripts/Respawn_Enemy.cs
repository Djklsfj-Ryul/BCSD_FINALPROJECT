using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Array
{
    public Array(GameObject _Collect, int _y, int _x)
    {
        Collect = _Collect;
        x = _x;
        y = _y;
    }
    public int x, y;
    public GameObject Collect;
}

public class Respawn_Enemy : MonoBehaviour
{
    public Vector3[] Rand_Pos = new Vector3[20];
    public Array[] Manage = new Array[Count_Big + Count_Medium + Count_Small + Count_Enemy];

    public static int Count_num = 0;
    static public int Catridge_Big    = 2;
    static public int Catridge_Medium = 2;
    static public int Catridge_Small  = 1;
    static public int Catridge_Enemy  = 1;

    [SerializeField] static public int Count_Big    = 1;
    [SerializeField] static public int Count_Medium = 2;
    [SerializeField] static public int Count_Small  = 3;
    [SerializeField] static public int Count_Enemy  = 1;

    [SerializeField] private GameObject[] Object_Big    = new GameObject[Catridge_Big];
    [SerializeField] private GameObject[] Object_Medium = new GameObject[Catridge_Medium];
    [SerializeField] private GameObject[] Object_Small  = new GameObject[Catridge_Small];
    [SerializeField] private GameObject[] Object_Enemy  = new GameObject[Catridge_Enemy];
    [SerializeField] private GameObject Object_Ground;

    [SerializeField] public GameObject Instant_Big;
    [SerializeField] public GameObject Instant_Medium;
    [SerializeField] public GameObject Instant_Small; 
    [SerializeField] public GameObject Instant_Enemy;

    public GameObject[,] manage = new GameObject[2, Count_Small];
    public GameObject CoverImage;

    BoxCollider Range_Collider;

    static int MAP_X = 20;
    static int MAP_Z = 20;
    public static int[,] MAP = new int[MAP_X, MAP_Z];

    public bool Phase_3 = true;
    public bool Phase_2 = true;
    public bool Phase_1 = true;

    int range_X = 0;
    int range_Z = 0;
    private Vector3 RandomPostion = new Vector3(0f, 5.0f, 0f);

    protected bool[] Trigger = new bool[] { true, true, true, true };

    private void Awake()
    {
        Range_Collider = Object_Ground.GetComponent<BoxCollider>();
    }
    Full_System e;
    public void Start()
    {
        Making_Map();
    }
    private void Update()
    {
        if (!CoverImage.activeSelf && Phase_3)
        {
            Phase_3 = false;
            Random_Respawn_E();
        }
        if (Full_System.Phase == 2 && Phase_2)
        {
            Phase_2 = false;
            Clear();
            Random_Respawn_E1();
        }
        if (Full_System.Phase == 1 && Phase_1)
        {
            Phase_1 = false;
            Clear();
            Random_Respawn_E2();
        }
    }
    void Clear()
    {
        for (int i = 1; i < 19; i++)
        {
            for (int j = 1; j < 19; j++)
            {
                MAP[j, i] = 0;
            }
        }
        Destroy(Instant_Big);
        Destroy(manage[0, 0]);
        Destroy(manage[0, 1]);
        Destroy(manage[1, 0]);
        Destroy(manage[1, 1]);
        Destroy(manage[1, 2]);
        Destroy(Instant_Enemy);
        Trigger[0] = true;
        Trigger[1] = true;
        Trigger[2] = true;
        Trigger[3] = true;
    }
    void Random_Respawn_E()
    { 
        while (Trigger[3] == true)
        {
            if (Trigger[0] == true)
            {
                for (int i = 0; i < Count_Big; i++)
                {
                    int a = Random.Range(0, Catridge_Big);
                    int rand_a = Random.Range(1, 5);
                    rand_a *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_a, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Big = Instantiate(Object_Big[a], Rand_Pos[Count_num], quaternion);
                    /*
                    Manage[Count_num].Collect = Object_Big[a];
                    Manage[Count_num].x = (int)Rand_Pos[Count_num].x;
                    Manage[Count_num].y = (int)Rand_Pos[Count_num].z;
                    */
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[0] = false;
            }
            else if (Trigger[1] == true)
            {
                for (int i = 0; i < Count_Medium; i++)
                {
                    int b = Random.Range(0, Catridge_Medium);
                    int rand_b = Random.Range(1, 5);
                    rand_b *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_b, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Medium = Instantiate(Object_Medium[b], Rand_Pos[Count_num], quaternion);
                    manage[0, i] = Instant_Medium;
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[1] = false;
            }
            else if (Trigger[2] == true)
            {
                for (int i = 0; i < Count_Small; i++)
                {
                    int c = Random.Range(0, Catridge_Small);
                    int rand_c = Random.Range(1, 5);
                    rand_c *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_c, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Small = Instantiate(Object_Small[c], Rand_Pos[Count_num], quaternion);
                    manage[1, i] = Instant_Small;
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                    
                }
                Trigger[2] = false;
            }
            else if (Trigger[3] == true)
            {
                for (int i = 0; i < Count_Enemy; i++)
                {
                    int d = Random.Range(0, Catridge_Enemy);
                    int rand_d = Random.Range(1, 5);
                    rand_d *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, 0, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Enemy = Instantiate(Object_Enemy[d], Rand_Pos[Count_num], quaternion);
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[3] = false;
            }
        }
    }
    void Random_Respawn_E1()
    {
        while (Trigger[3] == true)
        {
            if (Trigger[0] == true)
            {
                for (int i = 0; i < Count_Big; i++)
                {
                    int a = Random.Range(0, Catridge_Big);
                    int rand_a = Random.Range(1, 5);
                    rand_a *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_a, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Big = Instantiate(Object_Big[a], Rand_Pos[Count_num], quaternion);
                    /*
                    Manage[Count_num].Collect = Object_Big[a];
                    Manage[Count_num].x = (int)Rand_Pos[Count_num].x;
                    Manage[Count_num].y = (int)Rand_Pos[Count_num].z;
                    */
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[0] = false;
            }
            else if (Trigger[1] == true)
            {
                for (int i = 0; i < Count_Medium; i++)
                {
                    int b = Random.Range(0, Catridge_Medium);
                    int rand_b = Random.Range(1, 5);
                    rand_b *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_b, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Medium = Instantiate(Object_Medium[b], Rand_Pos[Count_num], quaternion);
                    manage[0, i] = Instant_Medium;
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[1] = false;
            }
            else if (Trigger[2] == true)
            {
                for (int i = 0; i < Count_Small; i++)
                {
                    int c = Random.Range(0, Catridge_Small);
                    int rand_c = Random.Range(1, 5);
                    rand_c *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_c, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Small = Instantiate(Object_Small[c], Rand_Pos[Count_num], quaternion);
                    manage[1, i] = Instant_Small;
                    Count_num++;
                    //yield return new WaitForSeconds(1f);

                }
                Trigger[2] = false;
            }
            else if (Trigger[3] == true)
            {
                for (int i = 0; i < Count_Enemy; i++)
                {
                    int d = Random.Range(0, Catridge_Enemy);
                    int rand_d = Random.Range(1, 5);
                    rand_d *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, 0, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Enemy = Instantiate(Object_Enemy[d], Rand_Pos[Count_num], quaternion);
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[3] = false;
            }
        }
    }
    void Random_Respawn_E2()
    {
        while (Trigger[3] == true)
        {
            if (Trigger[0] == true)
            {
                for (int i = 0; i < Count_Big; i++)
                {
                    int a = Random.Range(0, Catridge_Big);
                    int rand_a = Random.Range(1, 5);
                    rand_a *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_a, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Big = Instantiate(Object_Big[a], Rand_Pos[Count_num], quaternion);
                    /*
                    Manage[Count_num].Collect = Object_Big[a];
                    Manage[Count_num].x = (int)Rand_Pos[Count_num].x;
                    Manage[Count_num].y = (int)Rand_Pos[Count_num].z;
                    */
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[0] = false;
            }
            else if (Trigger[1] == true)
            {
                for (int i = 0; i < Count_Medium; i++)
                {
                    int b = Random.Range(0, Catridge_Medium);
                    int rand_b = Random.Range(1, 5);
                    rand_b *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_b, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Medium = Instantiate(Object_Medium[b], Rand_Pos[Count_num], quaternion);
                    manage[0, i] = Instant_Medium;
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[1] = false;
            }
            else if (Trigger[2] == true)
            {
                for (int i = 0; i < Count_Small; i++)
                {
                    int c = Random.Range(0, Catridge_Small);
                    int rand_c = Random.Range(1, 5);
                    rand_c *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_c, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Small = Instantiate(Object_Small[c], Rand_Pos[Count_num], quaternion);
                    manage[1, i] = Instant_Small;
                    Count_num++;
                    //yield return new WaitForSeconds(1f);

                }
                Trigger[2] = false;
            }
            else if (Trigger[3] == true)
            {
                for (int i = 0; i < Count_Enemy; i++)
                {
                    int d = Random.Range(0, Catridge_Enemy);
                    int rand_d = Random.Range(1, 5);
                    rand_d *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, 0, 0));
                    Rand_Pos[Count_num] = Return_RandomPosition();
                    Instant_Enemy = Instantiate(Object_Enemy[d], Rand_Pos[Count_num], quaternion);
                    Count_num++;
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[3] = false;
            }
        }
    }
    public Vector3 Return_RandomPosition()
    {
        Vector3 Pos = Object_Ground.transform.position;
        Pos.x -= 10;
        Pos.z -= 10;

        range_X = Random.Range(1, (MAP_X - 2));
        range_Z = Random.Range(1, (MAP_Z - 2));
        RandomPostion = new Vector3(range_X, 5.0f, range_Z);

        Vector3 respawnPosition = Pos + RandomPostion;
        respawnPosition = Pointing(respawnPosition);
        return respawnPosition;
    }
    Vector3 Pointing(Vector3 RS_Pos)
    {
        int x = (int)RS_Pos.x; // 1 ~ 18
        int z = (int)RS_Pos.z; // 1 ~ 18
        if (Trigger[0] == true)
        {
        BACK_1:
            for (int j = -2; j < 3; j++)
            {
                for (int i = -2; i < 3; i++)
                {
                    if (x + i <= 0 || z + j <= 0 || x + i >= 19 || z + j >= 19)
                    {
                        x = Random.Range(1, (MAP_X - 2));
                        z = Random.Range(1, (MAP_Z - 2));
                        goto BACK_1;
                    }
                }
            }
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        MAP[z, x] = 8;
                    }
                    else
                    {
                        MAP[z + j, x + i] = 7;
                    }
                }
            }

            RS_Pos.x = x;
            RS_Pos.z = z;

            return RS_Pos;
        }
        else if(Trigger[1]==true)
        {
        BACK_2:
            for (int j = -1; j < 2; j++)
            {
                for (int i = -1; i < 2; i++)
                {
                    if ((x + i <= 0 || z + j <= 0 || x + i >= 19 || z + j >= 19) || (MAP[z + j, x + i] == 7 || MAP[z + j, x + i] == 8 || MAP[z + j, x + i] == 9))
                    {
                        x = Random.Range(1, (MAP_X - 2));
                        z = Random.Range(1, (MAP_Z - 2));
                        goto BACK_2;
                    }
                }
            }
            for (int j = -1; j < 2; j++)
            {
                for (int i = -1; i < 2; i++)
                {
                    if (i == 0 && j == 0)
                    {
                        MAP[z, x] = 8;
                    }
                    else
                    {
                        MAP[z + j, x + i] = 7;
                    }
                }
            }
            RS_Pos.x = x;
            RS_Pos.z = z;

            return RS_Pos;
        }
        else if (Trigger[2] == true)
        {
        BACK_3:
            if ((x <= 0 || z <= 0 || x >= 19 || z >= 19) || (MAP[z, x] == 7 || MAP[z, x] == 8 || MAP[z, x] == 9))
            {
                x = Random.Range(1, (MAP_X - 2));
                z = Random.Range(1, (MAP_Z - 2));
                goto BACK_3;
            }
            else
            {
                MAP[z, x] = 8;
            }
            RS_Pos.x = x;
            RS_Pos.z = z;

            return RS_Pos;
        }
        else if (Trigger[3] == true)
        {
        BACK_4:
            if ((x <= 0 || z <= 0 || x >= 19 || z >= 19) || (MAP[z, x] == 7 || MAP[z, x] == 8 || MAP[z, x] == 9))
            {
                x = Random.Range(1, (MAP_X - 2));
                z = Random.Range(1, (MAP_Z - 2));
                goto BACK_4;
            }
            else
            {
                MAP[z, x] = 9;
            }
            RS_Pos.x = x;
            RS_Pos.z = z;

            return RS_Pos;
        }
        return RS_Pos;
    }
    /*
     * 11111111111111111111
     * 10000000000000000001
     * 10000000000000000001
     * 10000000000000000001
     * 10000011111000020001
     * 10000011111000000001
     * 10000011211000000001
     * 10000011111000000001
     * 10000011111000020001
     * 10000000000000000001
     * 10002000000000000001
     * 10000000000011100001
     * 10000000000012100001
     * 10000000000011100001
     * 10009000000000000001
     * 10000000000000000001
     * 10000000011100000001
     * 10000000012100000001
     * 10000000011100000001
     * 11111111111111111111
     */
    /*로그로 맵 확인
    string a1 = "";
    for (int i = 0; i < 20; i++)
    {
        for (int j = 0; j < 20; j++)
        {
            a1 = a1 + MAP[j,i];
        }
        Debug.Log(a1);
        a1 = "";
    }
    */
    void Making_Map()
    {
        for (int i = 0; i < MAP_Z; i++)
        {
            for (int j = 0; j < MAP_X; j++)
            {
                if (j == 0 || j == (MAP_X - 1) || i == 0 || i == (MAP_Z - 1)) MAP[i, j] = 9;
                else MAP[i, j] = 0;
            }
        }
    }
}
