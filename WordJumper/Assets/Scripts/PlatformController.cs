using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformController : MonoBehaviour
{    
    private List<GameObject> PlatformList;
    private Queue<List<GameObject>> ScreenQueue;
    public GameObject PlatformPrefab;
    public Vector3 FirstPlatform;

    private GameObject Player;
    private BackgroundController _BackgroundController;

    private int CurrentScreen; // number of screens that have been generated

    private Vector2 ScreenDims; // Screen dimensions, depends on camera and resolution
    private Vector2 HalfDims;

    public Vector2 Resolution = new Vector2(5, 5);
    private int NextSpawnInterval;

    
    // Start is called before the first frame update
    public void Start()
    {        
        CurrentScreen = 0;
        PlatformList = new List<GameObject>();
        ScreenQueue = new Queue<List<GameObject>>();
        Player = GameObject.FindGameObjectWithTag("Player");

        _BackgroundController = GameObject.FindObjectOfType<BackgroundController>();

        ScreenDims = new Vector2(CameraExtensions.OrthographicBounds(Camera.main).size.x, 
            CameraExtensions.OrthographicBounds(Camera.main).size.y);
        HalfDims = ScreenDims / 2;

        Instantiate(PlatformPrefab, FirstPlatform, Quaternion.identity);

        // generate first two screens
        GenerateScreen();
        GenerateScreen();
    }

    public void GenerateScreen() 
    {
        _BackgroundController.NextBackground();
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
            if (blocksOnRow == 0)   // Ensures theres at least one platform per row.
            {
                float px = (Random.Range(0,Resolution.x) * xStep) - (HalfDims.x) + 0.11f;
                float py = ((y * yStep) + (CurrentScreen * ScreenDims.y)) - (HalfDims.y);
                Spawn(px, py);
            }
        }

        ScreenQueue.Enqueue(PlatformList);
        PlatformList = new List<GameObject>();

        if (CurrentScreen > 2) 
        {
            DeleteScreen();
        }

        // load/despawn screen platforms out of player view
        NextSpawnInterval = (int)(((CurrentScreen) * ScreenDims.y) - (HalfDims.y));        
        
        CurrentScreen++;
    }

    void Spawn(float x, float y)
    {
        PlatformList.Add(Instantiate(PlatformPrefab, new Vector3(x, y, 0), Quaternion.identity));
    }

    void DeleteScreen() 
    {
        List<GameObject> toKill = ScreenQueue.Dequeue();
        foreach (var obj in toKill) 
        {
            GameObject.Destroy(obj);
        }

        toKill.Clear();
    }
        

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position.y > NextSpawnInterval) 
        {
            GenerateScreen();
        }
    }

    public void Replay() 
    {
        GameObject[] plats = GameObject.FindGameObjectsWithTag("Platform");
        foreach (var plat in plats) { Destroy(plat); }
        _BackgroundController.Replay();
        Start();
    }
}
