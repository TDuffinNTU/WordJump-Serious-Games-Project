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



    // Newgen stuff
    public int cols = 5;

    public float intervalY = 2.2f;
    [SerializeField]
    private float nextY;
    [SerializeField]
    private List<List<GameObject>> Rows;

    // Start is called before the first frame update
    public void Start()
    {
        CurrentScreen = 0;
        PlatformList = new List<GameObject>();
        Rows = new List<List<GameObject>>();
        nextY = FirstPlatform.y;
        ScreenQueue = new Queue<List<GameObject>>();
        Player = GameObject.FindGameObjectWithTag("Player");

        _BackgroundController = GameObject.FindObjectOfType<BackgroundController>();

        ScreenDims = new Vector2(CameraExtensions.OrthographicBounds(Camera.main).size.x,
            CameraExtensions.OrthographicBounds(Camera.main).size.y);        
        HalfDims = ScreenDims / 2;


        Rows.Add(new List<GameObject>()
        {
            Instantiate(PlatformPrefab, FirstPlatform, Quaternion.identity)
        });

        // generate rows of platforms
        GenerateRows(5);
        _BackgroundController.NextBackground();
        _BackgroundController.NextBackground();        
    }  


    void GenerateRows(int count)
    {             
        float cameraTopY = CameraExtensions.GetEdges(Camera.main)[1]; // position of top edge in y axis               

        for (int c = 0; c < count; c++)  
        {
            nextY += intervalY;
            List<GameObject> row = new List<GameObject>();
            for (int x = 0; x < cols; x++) 
            {
                float xStep = (ScreenDims.x / cols);
                float px = (x * xStep) - (HalfDims.x) + .6f ;
                row.Add(Instantiate(PlatformPrefab, new Vector3(px, nextY, 0), Quaternion.identity));
            }

            int rand = Random.Range(cols/2, cols);
            Debug.Log(rand);
            for (int i = 0; i < rand; i++) // remove between 1 and col minus 1 platforms per row
            {
                int index = Random.Range(0, row.Count);
                Destroy(row[index]);                
                row.RemoveAt(index);
            }

            Rows.Add(row);
        }
        
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
        if (Rows[0][0].transform.position.y < CameraExtensions.GetEdges(Camera.main)[3]) 
        {           
            var row = Rows[0];
            foreach (var plat in row) 
            {
                Destroy(plat);
            }
            Rows.RemoveAt(0);
            GenerateRows(1);
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
