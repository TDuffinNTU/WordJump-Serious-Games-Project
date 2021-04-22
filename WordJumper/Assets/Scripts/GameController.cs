using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using AnotherFileBrowser.Windows;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{
    public PlayerController _Plr;
    public LevelGenerator _Gen;
    public ScoreController _Scr;
    public CameraController _Cmr;
    public Slider _TimeSlider;
    public Slider _QuestionSlider;

    private Canvas _GameUI, _PausedUI, _GameOverUI, _PregameUI, _ScoreUI, _QuestionUI;

    private QAContainer _QAContainer;

    // questions and answers
    //private StreamReader _InFile;
    //private List<string> _Questions;
    //private List<string> _Answers;
    private List<GameObject> _QButtons;
    private int _QIndex = -1;
    private int _CorrectBtn = -1;

    [SerializeField]
    private STATE CurrentState = STATE.PREGAME;

    private const float MAXTIME = 20f;
    private const float QUESTIONMAXTIME = 6f;
    private const float COINTIME = 3f;
    private const float ANSWERMAXTIME = 1.2f;
    private const int COINSCORE = 3;

    private float TimeLeft = MAXTIME;
    private float QuestionTimeLeft = QUESTIONMAXTIME;
    private float AnswerTimeLeft = ANSWERMAXTIME;

    private float KillY;

    public enum STATE {
        PREGAME,
        PAUSED,
        UNPAUSED,
        QUESTION,
        ANSWER,
        GAMEOVER
    };

    // Start is called before the first frame update
    void Start()
    {
        _PausedUI = GameObject.Find("PausedUI").GetComponent<Canvas>();
        _GameUI = GameObject.Find("GameUI").GetComponent<Canvas>();
        _GameOverUI = GameObject.Find("GameOverUI").GetComponent<Canvas>();
        _PregameUI = GameObject.Find("PregameUI").GetComponent<Canvas>();
        _ScoreUI = GameObject.Find("ScoreUI").GetComponent<Canvas>();
        _QuestionUI = GameObject.Find("QuestionUI").GetComponent<Canvas>();

        // QAContiner class stores our questions and matching answers
        _QAContainer = GameObject.FindGameObjectWithTag("QAContainer").GetComponent<QAContainer>();
        
        // buttons on question screen
        _QButtons = new List<GameObject>()
        {
            GameObject.Find("QB1"),
            GameObject.Find("QB2"),
            GameObject.Find("QB3"),
            GameObject.Find("QB4")
        };


        SetGamestate(STATE.PREGAME);
    }

   

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case STATE.UNPAUSED:
                // timer decrements while playing
                TimeLeft -= Time.deltaTime;
                _TimeSlider.value = TimeLeft / MAXTIME;

                // Gameover when timer below 0
                if (TimeLeft <= 0)
                {
                    SetGamestate(STATE.GAMEOVER);
                }

                // player loses if they fall off the bottom of screen
                KillY = CameraExtensions.GetEdges(Camera.main)[3] - 0.1f;
                if (_Plr.transform.position.y < KillY)
                {
                    SetGamestate(STATE.GAMEOVER);
                }
                break;

            case STATE.PREGAME:
                // starting the game
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _ScoreUI.enabled = true;
                    _PregameUI.enabled = false;
                    SetGamestate(STATE.UNPAUSED);
                }
                break;

            case STATE.QUESTION:
                // similar deal with above code
                QuestionTimeLeft -= Time.deltaTime;
                _QuestionSlider.value = QuestionTimeLeft / QUESTIONMAXTIME;

                // submit wrong answer when timer below 0
                if (QuestionTimeLeft <= 0)
                {
                    TimeLeft -= COINTIME;
                    QuestionAnswer(-1);
                }
                break;
            case STATE.ANSWER:
                // similar deal with above code
                AnswerTimeLeft -= Time.deltaTime;                

                // transition to gameplay again when timer below 0
                if (AnswerTimeLeft <= 0)
                {
                    foreach (GameObject b in _QButtons) 
                    {
                        // reset colors
                        b.GetComponent<Image>().color = new Color(255, 255, 255, 255);
                    }

                    SetGamestate(STATE.UNPAUSED);
                }
                break;
            default:
                break;
        }

    }


    public void TogglePause()
    {
        if (CurrentState == STATE.PAUSED) {
            SetGamestate(STATE.UNPAUSED);
        }
        else if (CurrentState == STATE.UNPAUSED)
        {
            SetGamestate(STATE.PAUSED);
        }
    }

    public void QuestionMode()
    {
        SetGamestate(STATE.QUESTION);

        Text Title = GameObject.Find("QText").GetComponent<Text>();

        // index of our question and matching answer
        KeyValuePair<int, string> qipair = _QAContainer.GetRandomQuestionIndexPair();

        _QIndex =   qipair.Key; 
        _CorrectBtn =   Random.Range(0, _QButtons.Count);

        Title.text = _QAContainer.GetQuestionAt(_QIndex);

        for(int i = 0; i < _QButtons.Count; i++) 
        {
            var txtcomponent = _QButtons[i].GetComponentInChildren<Text>();
            // correct answer on correct button
            if (i == _CorrectBtn)
            {
                txtcomponent.text = _QAContainer.GetAnswerAt(_QIndex);
            }
            // random answer on other button
            else 
            {               
                // ensures right answer is always unique and not blank                
                txtcomponent.text = "";
                while (txtcomponent.text == "") 
                {
                    txtcomponent.text = _QAContainer.GetAnswerExcluding(_QIndex);
                }
                
            }
        }       

    } 

    public void QuestionAnswer(int answer) 
    {
        // Assume question is incorrect, as correct answer will overwrite it
        if (answer >= 0)
            // and avoid iob exceptions!
            _QButtons[answer].GetComponent<Image>().color = new Color(255, 0, 0, 255);
        _QButtons[_CorrectBtn].GetComponent<Image>().color = new Color(0, 255, 0, 255);

        // right/wrong answer
        if (answer == _CorrectBtn)
        {
            // correct
            _QAContainer.DelQuestion(_QIndex);
            if (_QAContainer.CountQuestions() < 2) 
                {
                    _Gen.GenerateCoins = false;   
                }

            TimeLeft += COINTIME;
            _Scr.AddScore(COINSCORE); 
        }
        else 
        {
            //incorrect
            TimeLeft -= COINTIME;  
        }

        _CorrectBtn = -1;
        SetGamestate(STATE.ANSWER);
    }

    public void SetGamestate(STATE state) 
    {
        // State Transitions
        switch (state) 
        {
            case STATE.PAUSED:
                _Plr.Frozen = true;
                _GameUI.enabled = false;
                _PausedUI.enabled = true;
                _GameOverUI.enabled = false;                
                break;
            case STATE.UNPAUSED:
                _Plr.Frozen = false;
                _GameUI.enabled = true;
                _PausedUI.enabled = false;
                _GameOverUI.enabled = false;
                _QuestionUI.enabled = false;                
                break;                       
            case STATE.GAMEOVER:
                _Plr.Frozen = true;
                _GameUI.enabled = false;
                _GameOverUI.enabled = true;
                _PausedUI.enabled = false;
                break;
            case STATE.PREGAME:
                _Plr.Frozen = true;
                _PregameUI.enabled = true;
                _PausedUI.enabled = false;
                _GameUI.enabled = false;
                _GameOverUI.enabled = false;
                _ScoreUI.enabled = false;
                _QuestionUI.enabled = false;
                break;
            case STATE.QUESTION:
                _Plr.Frozen = true;
                _QuestionUI.enabled = true;
                _GameUI.enabled = false;
                _GameOverUI.enabled = false;
                QuestionTimeLeft = QUESTIONMAXTIME;
                foreach (var b in _QButtons) { b.GetComponent<Button>().enabled = true; }
                break;
            case STATE.ANSWER:
                AnswerTimeLeft = ANSWERMAXTIME;
                foreach (var b in _QButtons) { b.GetComponent<Button>().enabled = false; }
                break;
            default:
                // invalid states log an error
                Debug.LogError("INVALID STATE: " + state.ToString());
                return;                
        }

        CurrentState = state; 
    }

    public void CoinCollected() 
    {
        //Debug.Log(_QAContainer.CountQuestions());
        if (_QAContainer.CountQuestions() > 1)
            QuestionMode();        
        else
            _Scr.AddScore(COINSCORE);
    }

    public void Replay() 
    {
        // reset gameplay field (doesn't require scene reload)
        TimeLeft = MAXTIME;
        _Plr.Replay();
        _Gen.Replay();
        _Scr.Replay();
        _Cmr.Replay();
        _QAContainer.ReloadValues();
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (var coin in coins) { Destroy(coin); }
        SetGamestate(STATE.PREGAME);
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
