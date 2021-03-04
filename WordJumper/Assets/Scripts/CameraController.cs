using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{    
    private PlayerController Player;
    private Camera ThisCam;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        Player = null; //(PlayerController)GameObject.FindGameObjectWithTag("Player");
        ThisCam = GetComponent<Camera>();
    }

    // Uplatedate is called once per frame
    void LateUpdate()
    {
        if (Player.transform.position.y > transform.position.y) { transform.position = new Vector3(0, Player.transform.position.y, transform.position.z); }


        // assume camera is aligned at x = 0
        float rightBound = CameraExtensions.OrthographicSize(ThisCam) / 2;
        float leftBound = rightBound * -1;

        //wrap around
        float playerWidth = Player.GetSize().x;
        if (Player.transform.position.x + playerWidth < leftBound)
        {
            Player.transform.position = new Vector3(rightBound + playerWidth, Player.transform.position.y, 0);
        }
        else if (Player.transform.position.x - playerWidth > rightBound)
        {
            Player.transform.position = new Vector3(leftBound - playerWidth, Player.transform.position.y, 0);
        }                
    }

    public void Replay()
    {
        Start();
    }
}

public static class CameraExtensions
{
    public static Bounds OrthographicBounds(this Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public static float OrthographicSize(this Camera camera) 
    {
        return camera.orthographicSize;
    }

    // returns single float positions of each edge in order: LEFT, TOP, RIGHT, BOTTOM
    public static List<float> GetEdges(this Camera camera) 
    {
        Bounds bounds = OrthographicBounds(camera);
        Vector2 halfsize = bounds.size / 2f;

        List<float> edges = new List<float>();
        
        edges.Add(camera.transform.position.x - halfsize.x);
        edges.Add(camera.transform.position.y + halfsize.y);
        edges.Add(camera.transform.position.x + halfsize.x);
        edges.Add(camera.transform.position.y - halfsize.y);

        return edges;
    }
}