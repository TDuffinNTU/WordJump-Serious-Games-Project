using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class GameController : MonoBehaviour
{
    public PlayerController _Plr;
    public LevelGenerator _Gen;
    public ScoreController _Scr;
    public CameraController _Cmr;
    public Slider _TimeSlider;

    //[SerializeField]
    Canvas _GameUI, _PausedUI, _GameOverUI, _PregameUI, _ScoreUI;
    
    

    [SerializeField]
    private string State = "UNPAUSED";

    private const float MAXTIME = 20f;
    private const float COINTIME = 3f;
    private float TimeLeft = MAXTIME;
    
    private float killY;

    // Start is called before the first frame update
    void Start()
    {       
        _PausedUI = GameObject.Find("PausedUI").GetComponent<Canvas>();
        _GameUI = GameObject.Find("GameUI").GetComponent<Canvas>();
        _GameOverUI = GameObject.Find("GameOverUI").GetComponent<Canvas>();
        _PregameUI = GameObject.Find("PregameUI").GetComponent<Canvas>();
        _ScoreUI = GameObject.Find("ScoreUI").GetComponent<Canvas>();
        

        SetGamestate("PREGAME");
    }

    // Update is called once per frame
    void Update()
    {

        // timer decrements while playing
        if (State == "UNPAUSED")
        {
            TimeLeft -= Time.deltaTime;
            _TimeSlider.value = TimeLeft / MAXTIME;

            // Gameover when timer below 0
            if (TimeLeft <= 0) 
            {
                SetGamestate("PAUSED");
            }
        }

        // player loses if they fall off the bottom of screen, calculated here
        killY = CameraExtensions.GetEdges(Camera.main)[3];
        if (_Plr.transform.position.y < killY) 
        {
            SetGamestate("PAUSED");
        }

        // starting the game
        if (Input.GetKeyDown(KeyCode.Space) && State == "PREGAME") 
        {            
            _ScoreUI.enabled = true;
            _PregameUI.enabled = false;            
            SetGamestate("UNPAUSED");
        }
        
    }

    public void TogglePause() 
    {
        if (State == "PAUSED") {
            SetGamestate("UNPAUSED");
        } 
        else if (State == "UNPAUSED") 
        {
            SetGamestate("PAUSED");
        }
    }

    public void SetGamestate(string state) // Finite State Machine
    {
        state = state.ToUpper();
        switch (state) 
        {
            case "PAUSED":
                Time.timeScale = 0;
                _GameUI.enabled = false;
                _PausedUI.enabled = true;
                _GameOverUI.enabled = false;                
                break;
            case "UNPAUSED":
                Time.timeScale = 1;
                _GameUI.enabled = true;
                _PausedUI.enabled = false;
                _GameOverUI.enabled = false;
                break;                       
            case "GAMEOVER":
                _Plr.enabled = false;
                _GameUI.enabled = false;
                _GameOverUI.enabled = true;
                _PausedUI.enabled = false;
                break;
            case "PREGAME":
                Time.timeScale = 0;
                _PregameUI.enabled = true;
                _PausedUI.enabled = false;
                _GameUI.enabled = false;
                _GameOverUI.enabled = false;
                _ScoreUI.enabled = false;                
                break;
            default:
                // invalid states do not trigger change in state var
                return;                
        }

        State = state; 
    }


    public void AddTime() { TimeLeft += COINTIME; }


    public void Replay() // reset gameplay field (doesn't require scene reload)
    {
        TimeLeft = MAXTIME;
        _Plr.Replay();
        _Gen.Replay();
        _Scr.Replay();
        _Cmr.Replay();
        SetGamestate("PREGAME");
    }
}
