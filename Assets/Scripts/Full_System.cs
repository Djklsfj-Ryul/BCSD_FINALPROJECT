using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Full_System : MonoBehaviour
{
    public static int Stamina_Player = 30;
    public static int Stamina_Enemy  = 50;

    public static int Phase = 3;
    public int Turn = 2;

    public int player_point = 0;
    public int enemy_point = 0;

    public static bool player_move = true;

    public static Respawn_Player res_P;
    public static Respawn_Enemy res_E;

    Enemy sys_e;

    void Start()
    {
        //player = new GameObject();
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
            player_move = false;
            if(Enemy.em)
            {
                Debug.Log("턴을 종료합니다.");
                Turn--;
                Point_Check();
                Stamina_Player = 30;
                Stamina_Enemy = 50;
                Debug.Log("내 점수 : " + player_point);
                Debug.Log("적 점수 : " + enemy_point);
                player_move = true;
            }
        }
        else if (Stamina_Player == 0 && Turn == 0)
        {
            Debug.Log("페이즈를 종료합니다.");
            Phase--;
            Turn = 2;
        }
        else if(Phase == 0)
        {
            Debug.Log("게임을 종료합니다.");
        }
    }
    void Point_Check()
    {
        Debug.Log("플레이어 맵");
        string a1 = "";
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                a1 = a1 + Respawn_Player.MAP[j, i];
            }
            Debug.Log(a1);
            a1 = "";
        }
        Debug.Log("적 맵");
        string a2 = "";
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                a2 = a2 + Respawn_Enemy.MAP[j, i];
            }
            Debug.Log(a2);
            a2 = "";
        }
        if (Respawn_Player.MAP[Enemy.Pos_z, Enemy.Pos_x] != 0)
            player_point += 1;
        if (Respawn_Enemy.MAP[Player.posz, Player.posx] != 0)
            enemy_point += 1;
    }
}
