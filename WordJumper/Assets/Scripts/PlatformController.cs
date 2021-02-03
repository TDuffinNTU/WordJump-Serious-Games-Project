using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformController : MonoBehaviour
{
    private List<GameObject> Platforms;
    public GameObject PlatformPrefab;
    private Rigidbody2D PlayerRB;
    private float NextSpawn = 5f;
    private const float NS_INTERVAL = 80f;

    [SerializeField]
    private float TotalClimbed = 0f;
    [SerializeField]
    private float HighY = -10f;
    

    // Start is called before the first frame update
    void Start()
    {
        Platforms = new List<GameObject>();
        PlayerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        

        // Spawn platform below player
        Platforms.Add(Instantiate(PlatformPrefab, 
            new Vector3(0, PlayerRB.transform.position.y - 10, 0), Quaternion.identity));

        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        float py = PlayerRB.transform.position.y;
        if (py > HighY) 
        {
            TotalClimbed += (py - HighY);
            HighY = py;

            if (HighY > NextSpawn) 
            {
                Spawn();
                NextSpawn += NS_INTERVAL;
            }
        }
    }

    void Spawn()
    {
        float px = PlayerRB.transform.position.x;
        float py = PlayerRB.transform.position.y;

        int LayersPerSpawn = 4;

        for (int i = 0; i < LayersPerSpawn; i++)
        {
            for (int j = 0; j < Random.Range(1, 2); j++)
            {
                float x = Random.Range(px - 20, px + 20);
                float y = NextSpawn + (NS_INTERVAL / (i+1));
                float scale = Random.Range(0.8f, 1.5f);

                var newPlatform = Instantiate(PlatformPrefab, new Vector3(x, y, 0),
                    Quaternion.identity);

                newPlatform.transform.localScale = new Vector3(scale, 0.2f, 1);

                Platforms.Add(newPlatform);
            }
        }
    }

    void Despawn() { }
    
}
