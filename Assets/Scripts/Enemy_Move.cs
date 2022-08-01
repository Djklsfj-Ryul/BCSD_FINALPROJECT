using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    List<int> Ground_Enemy = new List<int>();
    public GameObject cube;

    [Range(0, 100)] public int min;
    [Range(0, 100)] public int max;

    // Start is called before the first frame update
    void Start()
    {
        CreateRandom(min, max);
        for (int i = 0; i < Ground_Enemy.Count; i++)
        {
            Debug.Log(Ground_Enemy[i]);
            Instantiate(cube, new Vector3(i, 0, Ground_Enemy[i]), Quaternion.identity);
        }
    }

    void CreateRandom(int min, int max)
    {
        for (int i = 0; i < max; i++)
        {
            Ground_Enemy.Add(Random.Range(min, max));
        }
    }

}
