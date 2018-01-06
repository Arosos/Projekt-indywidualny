using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width = 30, height = 30;
    public float nodeSize = 1f;
    public string seed;
    public bool useRandomSeed = true;
    public Biome[] biomes;

    [Range(0, 100)]
    public int randomFillPercentage = 30;
    public int smoothNumber = 4;

    int[,] map;
    int[,] biomeMap;
    GameObject[] tiles;
    int seedHashCode;

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
        biomeMap = new int[width, height];
        if (tiles != null)
            foreach (GameObject t in tiles)
                Destroy(t);
        tiles = new GameObject[width * height];
        RandomMap();
        for (int i = 0; i < smoothNumber; i++)
            SmoothMap();
        PerlinNoiseMap();
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

    void PerlinNoiseMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    float noiseValue = Mathf.PerlinNoise((float)x / (width - 1), (float)y / (height - 1));
                    for (int i = 0; i < biomes.Length; i++)
                    {
                        if (biomes[i].threshold < noiseValue)
                        {
                            biomeMap[x, y] = i;
                            tiles[x * width + y] = Instantiate(biomes[i].tilePrefab, new Vector3(x * nodeSize, y * nodeSize, 1f), new Quaternion(), transform);
                            break;
                        }
                    }
                }
            }
        }
    }
}
