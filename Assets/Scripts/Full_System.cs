using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Full_System : MonoBehaviour
{
    public static int Stamina_Player = 30;
    public static int Stamina_Enemy  = 50;

    public int Phase = 3;
    public int Turn = 3;

    public int player_point = 0;
    public int enemy_point = 0;

    public static bool player_move = true;

    private static Respawn_Player res_P;
    private static Respawn_Enemy res_E;

    public GameObject player;
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
            Debug.Log("턴을 종료합니다.");
            player_move = false;
            sys_e.Enemy_Move();
            Turn--;
            Point_Check();
            Stamina_Player = 30;
            Stamina_Enemy  = 50;
        }
        else if (Stamina_Player == 0 && Turn == 0)
        {
            Debug.Log("페이즈를 종료합니다.");
            Phase--;
            Turn = 3;
        }
        else if(Phase == 0)
        {
            Debug.Log("게임을 종료합니다.");
        }
    }
    void Point_Check()
    {
        if (res_P.MAP[(int)res_E.Instant_Enemy.gameObject.transform.position.z, (int)res_E.Instant_Enemy.gameObject.transform.position.x] != 0)
            player_point += 1;
        if (res_E.MAP[(int)Mathf.Ceil(player.gameObject.transform.position.z), (int)Mathf.Ceil(player.gameObject.transform.position.x)] != 0)
            enemy_point += 1;
    }
}
