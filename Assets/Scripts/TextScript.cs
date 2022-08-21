using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public Text TextScore;
    public Text PlayerSc;
    public Text EnemySc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Full_System.player_point > Full_System.enemy_point)
        {
            EnemySc.text = string.Format($"{ Full_System.enemy_point}");
            PlayerSc.text = string.Format($"{ Full_System.player_point}");
            TextScore.text = "Player";
        }
        else if (Full_System.player_point < Full_System.enemy_point)
        {
            EnemySc.text = string.Format($"{ Full_System.enemy_point}");
            PlayerSc.text = string.Format($"{ Full_System.player_point}");
            TextScore.text = "Enemy";
        }
        else
        {
            EnemySc.text = string.Format($"{ Full_System.enemy_point}");
            PlayerSc.text = string.Format($"{ Full_System.player_point}");
            TextScore.text = "Draw";
        }
    }
}
