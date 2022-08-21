using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Player : MonoBehaviour
{
    Vector3[,] Rand_Pos = new Vector3[20, 2];

    static public int Catridge_Big = 2;
    static public int Catridge_Medium = 2;
    static public int Catridge_Small = 1;
    static public int Catridge_Enemy = 1;
    [SerializeField]
    static protected int Count_Big = 1;
    [SerializeField]
    static protected int Count_Medium = 2;
    [SerializeField]
    static protected int Count_Small = 3;
    [SerializeField]
    static protected int Count_Player = 1;

    [SerializeField]
    public GameObject[] Object_Big = new GameObject[Catridge_Big];
    [SerializeField]
    public GameObject[] Object_Medium = new GameObject[Catridge_Medium];
    [SerializeField]
    public GameObject[] Object_Small = new GameObject[Catridge_Small];
    [SerializeField]
    public GameObject[] Object_Player = new GameObject[Catridge_Enemy];
    [SerializeField]
    private GameObject Object_Ground;
    BoxCollider Range_Collider;

    public GameObject CoverImage;

    static public int MAP_X = 20;
    static public int MAP_Z = 20;
    public int[,] MAP = new int[MAP_Z, MAP_X];

    int range_X = 0;
    int range_Z = 0;
    private Vector3 RandomPostion = new Vector3(0f, 5.0f, 0f);

    protected bool[] Trigger = new bool[] { true, true, true, true };

    private void Awake()
    {
        Range_Collider = Object_Ground.GetComponent<BoxCollider>();
    }

    public void Start()
    {
        Making_Map();
    }

    public void Update()
    {
        if (!CoverImage.activeSelf)
            Random_Respawn_P();

    }
    void Random_Respawn_P()
    {
        while (Trigger[3] == true)
        {
            //1초마다 랜덤으로 생성하도록 설정
            //추후 매페이즈 마다 생성하도록 트리거를 넣을 예정

            if (Trigger[0] == true)
            {
                for (int i = 0; i < Count_Big; i++)
                {
                    int a = Random.Range(0, Catridge_Big);
                    int rand_a = Random.Range(1, 5);
                    rand_a *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, rand_a, 0));
                    GameObject Instant_Big = Instantiate(Object_Big[a], Return_RandomPosition(), quaternion);
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
                    GameObject Instant_Medium = Instantiate(Object_Medium[b], Return_RandomPosition(), quaternion);
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
                    GameObject Instant_Small = Instantiate(Object_Small[c], Return_RandomPosition(), quaternion);
                    //yield return new WaitForSeconds(1f);
                }
                Trigger[2] = false;
            }
            else if (Trigger[3] == true)
            {
                for (int i = 0; i < Count_Player; i++)
                {
                    int d = Random.Range(0, Catridge_Enemy);
                    int rand_d = Random.Range(1, 5);
                    rand_d *= 90;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, 0, 0));
                    GameObject Instant_Enemy = Instantiate(Object_Player[d], Return_RandomPosition(), quaternion);
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
        else if (Trigger[1] == true)
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
    //로그로 맵 확인
    /*
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
