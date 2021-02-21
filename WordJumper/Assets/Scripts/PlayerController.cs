using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mc;
    private SpriteRenderer sr;
    private Animator an;

    public int XSpeed = 5;
    public int YSpeed = 14;
    public float GravityScale = 2.2f;
    public Vector3 StartPos;
    public Vector3 CamStartPos = new Vector3(0,-.29f, -10f);
    
    private bool onFloor = false;

    // Start is called before the first frame update
    void Start()
    {       
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
        mc = Camera.main;
        mc.transform.position = CamStartPos;       
        
        transform.position = StartPos;
    }

    // Update is called once per frame
    void Update()
    {        
        rb.gravityScale = GravityScale;
        onFloor = rb.GetContacts(new List<ContactPoint2D>()) > 0 ? true : false;

        Vector3 InputX = new Vector3(Input.GetAxis("Horizontal"), 0, 0);     
        transform.Translate(InputX * XSpeed * Time.deltaTime);

        bool InputY = Input.GetKeyDown(KeyCode.Space);
        if (rb.velocity.y <= 0 && onFloor)
        {            
            rb.velocity = new Vector2(rb.velocity.x, YSpeed);
        }

        sr.flipX = InputX.x < 0 ? true : false;

        an.SetFloat("YSpeed", rb.velocity.y);

        UpdateCamera();
    }

    void UpdateCamera() 
    {
        if (transform.position.y > mc.transform.position.y) 
        { 
            mc.transform.position = new Vector3(0, transform.position.y, mc.transform.position.z); 
        }        
        
        // assume camera is aligned at x = 0
        float rightBound = mc.orthographicSize / 2;
        float leftBound = rightBound * -1;

        //wrap around
        float playerWidth = sr.bounds.size.x;
        if (transform.position.x + playerWidth < leftBound)
        {
            transform.position = new Vector3(rightBound + playerWidth, transform.position.y, 0);
        }
        else if (transform.position.x - playerWidth > rightBound) 
        {
            transform.position = new Vector3(leftBound - playerWidth, transform.position.y, 0);
        }
    }

    public Vector3 GetSize()
    {
        return sr.bounds.size;
    }

    public Rigidbody2D GetRB() 
    {
        return rb;
    }

    public void Replay() 
    {
        Start();
    }

}
