using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Respawn : MonoBehaviour
{
    static int Catridge_Big = 2;
    static int Catridge_Medium = 2;
    static int Catridge_Small = 1;
    [SerializeField]
    static int Count_Big = 1;
    [SerializeField]
    static int Count_Medium = 2;
    [SerializeField]
    static int Count_Small = 3;

    public GameObject[] Object_Big = new GameObject[Catridge_Big];
    public GameObject[] Object_Medium = new GameObject[Catridge_Medium];
    public GameObject[] Object_Small = new GameObject[Catridge_Small];
    public GameObject Object_Ground;
    BoxCollider Range_Collider;


    static int MAP_X = 20;
    static int MAP_Z = 20;
    int[,] MAP = new int[MAP_X,MAP_Z];

    int range_X = 0;
    int range_Z = 0;

    bool[] Trigger = new bool[] { true, true, true };

    private void Awake()
    {
        Range_Collider = Object_Ground.GetComponent<BoxCollider>();
    }

    public void Start()
    {
        Making_Map();
        StartCoroutine(Random_Respawn());
    }

    public Vector3 Return_RandomPosition()
    {
        Vector3 Pos = Object_Ground.transform.position;
        Pos.x -= 10;
        Pos.z -= 10;

        range_X = Random.Range(1, (MAP_X - 2));
        range_Z = Random.Range(1, (MAP_Z - 2));
        Vector3 RandomPostion = new Vector3(range_X, 5.0f, range_Z);

        Vector3 respawnPosition = Pos + RandomPostion;
        respawnPosition = Pointing(respawnPosition);
        return respawnPosition;
    }

    IEnumerator Random_Respawn()
    {
        while (Trigger[2] == true)
        {
            //1초마다 랜덤으로 생성하도록 설정
            //추후 매페이즈 마다 생성하도록 트리거를 넣을 예정

            if (Trigger[0] == true)
            {
                int a = Random.Range(0, Catridge_Big);
                for (int i = 0; i < Count_Big; i++)
                {
                    GameObject Instant_Big = Instantiate(Object_Big[a], Return_RandomPosition(), Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }
                Trigger[0] = false;
            }
            else if (Trigger[1] == true)
            {
                int b = Random.Range(0, Catridge_Medium);
                for (int i = 0; i < Count_Medium; i++)
                {
                    GameObject Instant_Medium = Instantiate(Object_Medium[b], Return_RandomPosition(), Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }
                Trigger[1] = false;
            }
            else if (Trigger[2] == true)
            {
                int c = Random.Range(0, Catridge_Small);
                for (int i = 0; i < Count_Small; i++)
                {
                    GameObject Instant_Small = Instantiate(Object_Small[c], Return_RandomPosition(), Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }
                Trigger[2] = false;
            }
        }
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
    }
    Vector3 Pointing(Vector3 RS_Pos)
    {
        int x = (int)RS_Pos.x; // 1 ~ 18
        int z = (int)RS_Pos.z; // 1 ~ 18
        if (Trigger[0] == true)
        {
            while (true)
            {
                for (int j = -2; j < 3; j++)
                {
                    for (int i = -2; i < 3; i++)
                    {
                        if (x + i <= 0 || z + j <= 0 || x + i >= 19 || z + j >= 19)
                        {
                            x = Random.Range(1, (MAP_X - 2));
                            z = Random.Range(1, (MAP_Z - 2));
                            continue;
                        }
                        else
                        {
                            if (i == 0 && j == 0)
                            {
                                MAP[z , x] = 2;
                            }
                            else
                            {
                                MAP[z + j, x + i] = 1;
                            }
                        }
                    }
                }
                break;
            }
            RS_Pos.x = x;
            RS_Pos.z = z;

            return RS_Pos;
        }
        else if(Trigger[1]==true)
        {
            BACK:
            for (int j = -1; j < 2; j++)
            {
                for (int i = -1; i < 2; i++)
                {
                    if ((x + i <= 0 || z + j <= 0 || x + i >= 19 || z + j >= 19) || (MAP[z + j, x + i] == 1 || MAP[z + j, x + i] == 2 || MAP[z + j, x + i] == 9))
                    {
                        x = Random.Range(1, (MAP_X - 2));
                        z = Random.Range(1, (MAP_Z - 2));
                        goto BACK;
                    }
                }
            }
            for (int j = -1; j < 2; j++)
            {
                for (int i = -1; i < 2; i++)
                {
                    if (i == 0 && j == 0)
                    {
                        MAP[z, x] = 2;
                    }
                    else
                    {
                        MAP[z + j, x + i] = 1;
                    }
                }
            }
            RS_Pos.x = x;
            RS_Pos.z = z;

            return RS_Pos;
        }
        else if (Trigger[2] == true)
        {
            while (true)
            {
                if ((x <= 0 || z <= 0 || x >= 19 || z >= 19) || (MAP[z, x] == 1 || MAP[z, x] == 2 || MAP[z, x] == 9))
                {
                    x = Random.Range(1, (MAP_X - 2));
                    z = Random.Range(1, (MAP_Z - 2));
                    continue;
                }
                else
                {
                    MAP[z, x] = 2;
                }
                break;
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
    void Making_Map()
    {
        for (int i = 0; i < MAP_Z; i++)
        {
            for (int j = 0; j < MAP_X; j++)
            {
                if (j == 0 || j == (MAP_X - 1) || i == 0 || i == (MAP_Z - 1)) MAP[i, j] = 3;
                else MAP[i, j] = 0;
            }
        }
    }
}
