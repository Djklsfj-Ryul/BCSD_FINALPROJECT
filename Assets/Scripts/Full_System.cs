using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Full_System : MonoBehaviour
{
    public static int Stamina_Player = 30;
    public static int Stamina_Enemy  = 50;

    public int Phase = 3;
    public int Turn = 3;

    Respawn_Player res_P;
    Respawn_Enemy res_E;
    Enemy sys_e;

    void Start()
    {
        Debug.Log("L을 눌러서 게임 시작");
    }
    // Update is called once per frame
    void Update()
    {
        Turn_End();
    }

    void Turn_End()
    {
        if (Stamina_Player == 0 && Turn != 0)
        {
            sys_e.Enemy_Move();
            Turn--;
        }
        else if (Stamina_Player == 0 && Turn == 0)
        {
            Debug.Log("페이즈를 종료합니다.");
            Phase--;
        }
    }
}
