using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public float ScrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //scrolling up every frame
        var cpos = Camera.main.transform.position;
        cpos.y += ScrollSpeed * Time.deltaTime;
        Camera.main.transform.position = cpos;
    }
}
