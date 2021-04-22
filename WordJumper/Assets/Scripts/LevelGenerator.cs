using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelGenerator : MonoBehaviour
{
   
    public GameObject PlatformPrefab;    
    public Vector3 FirstPlatform;
    
    private BackgroundController _BackgroundController;   

    private Vector2 ScreenDims; 
    private Vector2 HalfDims;   

    
    public int cols = 5;
    public float intervalY = 2.2f;

    private float nextY; 
    private List<List<GameObject>> Rows;

    // coin generation
    public float CoinChance = 0.1f;
    public GameObject CoinPrefab;

    public bool GenerateCoins = true;

    // Start is called before the first frame update
    public void Start()
    {             
        nextY = FirstPlatform.y;    
        _BackgroundController = GameObject.FindObjectOfType<BackgroundController>();

        // getting size of the screen in in-game units
        ScreenDims = new Vector2(CameraExtensions.OrthographicBounds(Camera.main).size.x,
            CameraExtensions.OrthographicBounds(Camera.main).size.y);        

        HalfDims = ScreenDims / 2;


        // initial platform
        Instantiate(PlatformPrefab, FirstPlatform, Quaternion.identity);       

        // generate rows of platforms
        GenerateRows(5);               
    }  


    void GenerateRows(int count)
    {
        // left and right edges are constant in our case so maybe not necessary to call this every loop?
        // not too taxing so can be left for now
        List<float> cameraEdges = CameraExtensions.GetEdges(Camera.main);                       

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
            for (int i = 0; i < rand; i++) // remove between 1 and col minus 1 platforms per row
            {
                int index = Random.Range(0, row.Count);
                Destroy(row[index]);                
                row.RemoveAt(index);
            }

            if (Random.value < CoinChance && GenerateCoins) 
            {
                // spawn coin based on random chance per row generated                
                Instantiate(CoinPrefab, new Vector3(Random.Range(cameraEdges[0], cameraEdges[2]), nextY + 0.5f, 0), Quaternion.identity);
            }

            row.Clear();
        }
        
    }   


    // Update is called once per frame
    void Update()
    {       
        if (nextY < CameraExtensions.GetEdges(Camera.main)[1]) 
        {
            GenerateRows(1);
        } 
    }

    public void Replay() 
    {
        GameObject[] plats = GameObject.FindGameObjectsWithTag("Platform");
        foreach (var plat in plats) { Destroy(plat); }
        _BackgroundController.Replay();
        GenerateCoins = true;
        Start();
    }
}
