using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //GetComponent<ScoreController>().AddScore(1);
        Destroy(gameObject);
        GameObject.FindObjectOfType<ScoreController>().AddScore(1);
    }

    // Update is called once per frame
    void Update()
    {
        // remove from game if off screen
        if (transform.position.y + 1 < CameraExtensions.GetEdges(Camera.main)[3]) { Destroy(gameObject); }
    }
}
