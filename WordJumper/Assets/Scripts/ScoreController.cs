using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{

    private int Score;
    private float HighPoint;
    private const float HIGHPOINTINTERVAL = 0.5f;
    private GameObject Player;
    private Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText = GetComponent<Text>();
        ScoreText.text = "0";
        Score = 0;        
        Player = GameObject.FindGameObjectWithTag("Player");
        HighPoint = Player.transform.position.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
     // TODO fix this scoring system (buggered mate)    
        float halfYPos = Player.transform.position.y / 2;        

        if (halfYPos > HighPoint + HIGHPOINTINTERVAL) {
            HighPoint += HIGHPOINTINTERVAL;
            AddScore(1);
        }
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
