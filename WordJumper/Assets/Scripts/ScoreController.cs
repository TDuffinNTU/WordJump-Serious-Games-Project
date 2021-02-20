using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    private int Score;
    private GameObject Player;
    private Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText = GetComponent<Text>();
        ScoreText.text = "0";
        Score = 0;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = Player.transform.position.y;

        if (yPos > Score) {
            AddScore(1);
        }
    }

    int CoordinatesToScore() 
    {
        return 1;    
    }

    public void AddScore(int score) 
    {
        Score += score;
        ScoreText.text = Score.ToString();
    }

    public void Replay() 
    {
        Start();
    }
}
