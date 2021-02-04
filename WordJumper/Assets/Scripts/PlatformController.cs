using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformController : MonoBehaviour
{
    private List<GameObject> Platforms;
    public GameObject PlatformPrefab;
    private Rigidbody2D PlayerRB;

    private int CurrentScreen; // number of screens that have been generated
    [SerializeField]
    private Vector2 ScreenDims; // Screen dimensions, depends on camera and resolution
    private Vector2 HalfDims;

    public int Scale = 1;
    public int Magnitude = 1;
    public int Exponent = 1;
    public int Min = 0;
    public Vector2 Resolution = new Vector2(5, 5);
    private int Seed;
    private int NextSpawnInterval;

    // Start is called before the first frame update
    public void Start()
    {
        CurrentScreen = 0;
        Seed = Random.Range(1, 99999);
        Platforms = new List<GameObject>();
        PlayerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        ScreenDims = new Vector2(CameraExtensions.OrthographicBounds(Camera.main).size.x, CameraExtensions.OrthographicBounds(Camera.main).size.y);
        HalfDims = ScreenDims / 2;

        // Spawn platform below player
        //Spawn(0, PlayerRB.transform.position.y - 0.2f);

        // generate first two screens
        GenerateScreen();
        GenerateScreen();
    }

    //rect PlatformBounds() { 

    //    return SpriteRenderer
    //}

    public void GenerateScreen() 
    {
        float xStep = (ScreenDims.x / Resolution.x);  
        float yStep = (ScreenDims.y / Resolution.y);  

        for (int y = 0; y < Resolution.y; y++) 
        {
            if (y == 0 && CurrentScreen == 0) continue; // ignore first row


            int blocksOnRow = 0;
            for (int x = 0; x < Resolution.x; x++) 
            {
                // convert to gamespace coordinates
                float px = (x*xStep) - (HalfDims.x) + 0.11f;       
                float py = ((y*yStep) + (CurrentScreen * ScreenDims.y)) - (HalfDims.y);

                if (Random.Range(0, (int)Resolution.x/2 + 1) == 1)
                {
                    Spawn(px, py);
                    blocksOnRow++;
                }          
            }
                                    // TODO -- We can make this less dorky using a while loop with multiple exit clauses.
            if (blocksOnRow == 0)   // if by random chance the previous algorithm failed, we'll do it once more for certain
            {
                float px = (Random.Range(0,Resolution.x) * xStep) - (HalfDims.x) + 0.11f;
                float py = ((y * yStep) + (CurrentScreen * ScreenDims.y)) - (HalfDims.y);
                Spawn(px, py);
            }
        }

        NextSpawnInterval = (int)(((CurrentScreen) * ScreenDims.y) - (HalfDims.y));
        CurrentScreen++;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform.position.y > NextSpawnInterval) 
        {
            GenerateScreen();
        }
    }

    void Spawn(float x, float y)
    {
        Platforms.Add(Instantiate(PlatformPrefab, new Vector3(x, y, 0), Quaternion.identity));
    }

    void Despawn() { }
    
}
