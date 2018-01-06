using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public int width = 30, height = 30;
    public GameObject ground;

    public string seed;
    public bool useRandomSeed = true;

    [Range(0, 100)]
    public int randomFillPercentage = 30;
    public int smoothNumber = 4;

    int[,] map;

    // Use this for initialization
    void Start ()
    {
        Generate();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
            Generate();
	}

    void Generate()
    {
        map = new int[width, height];
        RandomMap();

        for (int i = 0; i < smoothNumber; i++)
            SmoothMap();
    }

    void RandomMap()
    {
        if (useRandomSeed)
            seed = Time.time.ToString();

        System.Random prng = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    map[x, y] = 1;
                else
                    map[x, y] = (prng.Next(0, 100) < randomFillPercentage) ? 1 : 0;
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWallCount = GetNeighborWallCount(x, y);

                if (neighborWallCount > 4)
                    map[x, y] = 1;
                else if (neighborWallCount < 4)
                    map[x, y] = 0;
            }
        }
    }

    int GetNeighborWallCount(int xCoord, int yCoord)
    {
        int counter = 0;
        for (int x = xCoord - 1; x <= xCoord + 1; x++)
        {
            for (int y = yCoord - 1; y <= yCoord + 1; y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (x != xCoord || y != yCoord)
                        counter += map[x, y];
                }
                else
                    counter++;
            }
        }

        return counter;
    }

    void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
