using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float fillPercent = 0.45f;
    public int smoothingIterations = 5;
    public string seed;
    public bool useRandomSeed = true;

    private int[,] map;
    private Vector3 entrancePosition = new Vector3(0, 0, 50); // Fixed entrance

    void Start()
    {
        GenerateCave();
    }

    void GenerateCave()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smoothingIterations; i++)
        {
            SmoothMap();
        }

        EnsureEntrance();
        DrawCave();
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random rand = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1; // Border walls
                }
                else
                {
                    map[x, y] = (rand.NextDouble() < fillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        int[,] newMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWalls = GetSurroundingWallCount(x, y);
                newMap[x, y] = (neighborWalls > 4) ? 1 : (neighborWalls < 4) ? 0 : map[x, y];
            }
        }

        map = newMap;
    }

    void EnsureEntrance()
    {
        // Convert world space (14,0,50) to grid space
        int entranceX = 14;  // Directly using 14 since tileSize is unknown
        int entranceY = height - 1; // Ensure it's at the top edge

        Debug.Log($"Ensuring entrance at: ({entranceX}, {entranceY}) in grid space");

        // Carve out the entrance area
        for (int dy = 0; dy < 5; dy++) // Depth into the cave
        {
            for (int dx = -2; dx <= 2; dx++) // Width of entrance
            {
                int x = entranceX + dx;
                int y = entranceY - dy;

                // Ensure within bounds before modifying
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    map[x, y] = 0; // Set tile to empty (carving entrance)
                }
            }
        }

        Debug.Log($"Entrance successfully created at grid: ({entranceX}, {entranceY})");
    }

    int GetSurroundingWallCount(int x, int y)
    {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    count += map[nx, ny];
                }
                else
                {
                    count++; // Treat out-of-bounds as walls
                }
            }
        }
        return count;
    }

    void DrawCave()
    {
        Vector3 offset = new Vector3(-width / 2, 0, 50); // Offset to align entrance at border

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, 0, y) + offset;
                if (map[x, y] == 1)
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = pos;
                    wall.transform.localScale = new Vector3(1, 2, 1);
                }
            }
        }
    }
}