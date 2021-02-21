using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject BackgroundPrefab;
    private Queue<GameObject> Backgrounds;
    private Vector3 NextPos;
    private float BackgroundHeight;
    public Vector3 StartPos;

    // Start is called before the first frame update
    void Start()
    {
        NextPos = StartPos;
        BackgroundHeight = BackgroundPrefab.GetComponent<SpriteRenderer>().size.y;
        Backgrounds = new Queue<GameObject>();        
    }

    private void CalcNextPos() { NextPos = NextPos + Vector3.up * BackgroundHeight * 2; }

    public void NextBackground() 
    {
        Backgrounds.Enqueue(Instantiate(BackgroundPrefab, NextPos, Quaternion.identity));

        if (Backgrounds.Count > 3) 
        {
           Destroy(Backgrounds.Dequeue());
        }

        CalcNextPos();
    }

    public void Replay() 
    {
        foreach (var bg in Backgrounds) 
        {
            Destroy(bg);
        }
        
        Backgrounds = new Queue<GameObject>();
        NextPos = StartPos;

    }


}
