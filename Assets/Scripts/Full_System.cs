using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Full_System : MonoBehaviour
{
    public static int Stamina_Player = 30;
    public static int Stamina_Enemy  = 30;

    public int Phase = 3;
    public int Turn = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Turn_End();
    }

    public void Turn_End()
    {
        if(Stamina_Player == 0 && Turn != 0)
        {
            Debug.Log("턴을 종료합니다.");
            Turn--;
        }
        else if(Stamina_Player == 0 && Turn == 0)
        {
            Debug.Log("페이즈를 종료합니다.");
            Phase--;
        }
        Judgement();
    }
    public void Judgement()
    {

    }
    public static void Stamina(int STM)
    {
        STM--;
    }
}
