using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject BackgroundPrefab;
    private List<GameObject> Backgrounds;
    private Vector3 NextPos;
    private float BackgroundHeight;
    public Vector3 StartPos;
    //public bool MainMenuMode = false;

    // Start is called before the first frame update
    void Start()
    {
        NextPos = StartPos;
        BackgroundHeight = BackgroundPrefab.GetComponent<SpriteRenderer>().size.y;
        Backgrounds = new List<GameObject>();
        NextBackground();
        NextBackground();
    }

    private void CalcNextPos() { NextPos = NextPos + Vector3.up * BackgroundHeight * 2; }

    public void NextBackground() 
    {
        Backgrounds.Add(Instantiate(BackgroundPrefab, NextPos, Quaternion.identity));

        if (Backgrounds.Count > 3) 
        {            
            Destroy(Backgrounds[0]);           
            Backgrounds.RemoveAt(0);
        }

        CalcNextPos();
    }

    public void Replay() 
    {
        foreach (var bg in Backgrounds) 
        {
            Destroy(bg);
        }
        

        Backgrounds = new List<GameObject>();
        NextPos = StartPos;
        NextBackground();
        NextBackground();

    }

    public void Update()
    {        
        if (Backgrounds[0].transform.position.y + BackgroundHeight < CameraExtensions.GetEdges(Camera.main)[3]) 
        {
            Destroy(Backgrounds[0]);
            Backgrounds.RemoveAt(0);
            NextBackground();

        }
    }

}
