using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;    

    public int XSpeed = 20;
    public int YForce = 900;
    public float GravityScale = 4.5f;

    private int Deathplane = -60;


    [SerializeField]
    private bool onFloor = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();       
        
        rb.position = Vector3.zero;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rb.gravityScale = GravityScale;
        onFloor = rb.GetContacts(new List<ContactPoint2D>()) > 0 ? true : false;

        Vector3 InputX = new Vector3(Input.GetAxis("Horizontal"), 0, 0);     
        transform.Translate(InputX * XSpeed * Time.deltaTime);

        bool InputY = Input.GetKeyDown(KeyCode.Space);
        if (InputY && onFloor) 
        {
            rb.AddForce(new Vector2(0, YForce));            
        }

        if (transform.position.y < Deathplane) 
        {
            rb.position = Vector3.zero;
            rb.velocity = Vector3.zero;
        } 
    }

    
}
