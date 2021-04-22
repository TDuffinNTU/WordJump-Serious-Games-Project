using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{        
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController plr = collision.collider.GetComponent<PlayerController>();       
        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();

        if (rb != null && plr != null) 
        {
            if (collision.relativeVelocity.y < 0f) 
            {
                float YSpeed = plr.YSpeed;
                Vector2 vel = rb.velocity;
                vel.y = YSpeed;
                rb.velocity = vel;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y + 1 < CameraExtensions.GetEdges(Camera.main)[3]) 
        {
            Destroy(gameObject);
        }
    }
}
