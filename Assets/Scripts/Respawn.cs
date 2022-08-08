using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Respawn : MonoBehaviour
{

    [SerializeField]
    static int Catridge_Big = 2;
    [SerializeField]
    static int Catridge_Medium = 1;
    [SerializeField]
    static int Catridge_Small = 1;
    [SerializeField]
    static int Count_Big = 1;
    [SerializeField]
    static int Count_Medium = 2;
    [SerializeField]
    static int Count_Small = 3;

    int range_X = 0;
    int range_Z = 0;

    public GameObject[] Object_Big = new GameObject[Catridge_Big];
    public GameObject[] Object_Medium = new GameObject[Catridge_Medium];
    public GameObject[] Object_Small = new GameObject[Catridge_Small];
    public GameObject Object_Ground;
    BoxCollider Range_Collider;

    bool[] Trigger = new bool[] { true, true, true };

    private void Awake()
    {
        Range_Collider = Object_Ground.GetComponent<BoxCollider>();
    }

    public void Start()
    {
        range_X = (int)Range_Collider.bounds.size.x;
        range_Z = (int)Range_Collider.bounds.size.z;
        int[,] MAP = new int[range_Z, range_X];
        Making_Map(MAP);
        StartCoroutine(Random_Respawn());
    }

    public Vector3 Return_RandomPosition()
    {
        Vector3 Pos = Object_Ground.transform.position;

        range_X = (int)Range_Collider.bounds.size.x;
        range_Z = (int)Range_Collider.bounds.size.z;

        range_X = Random.Range(((range_X / 2) * -1) + 1, (range_X / 2) - 1);
        range_Z = Random.Range(((range_Z / 2) * -1) + 1, (range_Z / 2) - 1);
        Vector3 RandomPostion = new Vector3(range_X, 5.0f, range_Z);

        Vector3 respawnPosition = Pos + RandomPostion;
        return respawnPosition;
    }

    IEnumerator Random_Respawn()
    {
        while (Trigger[2] == true)
        {
            //1초마다 랜덤으로 생성하도록 설정
            //추후 매페이즈 마다 생성하도록 트리거를 넣을 예정
            int a = Random.Range(0, Catridge_Big);
            int b = Random.Range(0, Catridge_Medium);
            int c = Random.Range(0, Catridge_Small);

            if (Trigger[0] == true)
            {
                for (int i = 0; i < Count_Big; i++)
                {
                    GameObject Instant_Big = Instantiate(Object_Big[a], Return_RandomPosition(), Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }
                Trigger[0] = false;
            }
            else if (Trigger[1] == true)
            {
                for (int i = 0; i < Count_Medium; i++)
                {
                    GameObject Instant_Medium = Instantiate(Object_Medium[b], Return_RandomPosition(), Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }
                Trigger[1] = false;
            }
            else if (Trigger[2] == true)
            {
                for (int i = 0; i < Count_Small; i++)
                {
                    GameObject Instant_Small = Instantiate(Object_Small[c], Return_RandomPosition(), Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }
                Trigger[2] = false;
            }
        }
    }
    void Making_Map(int[,] MAP)
    {
        for (int i = 0; i < range_Z; i++)
        {
            for (int j = 0; j < range_X; j++)
            {
                if( j == 0 || j == (range_X - 1) || i == 0 || i == (range_Z - 1))
                {
                    MAP[i, j] = 1;
                }
                else
                {
                    MAP[i, j] = 0;
                }
            }
        }
    }
}
