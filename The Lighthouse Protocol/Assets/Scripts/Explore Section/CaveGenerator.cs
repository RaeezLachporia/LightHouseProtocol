using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public int depth = 5; // Number of levels on the Y-axis
    public float fillPercent = 0.45f;
    public int smoothingIterations = 5;
    public string seed;
    public bool useRandomSeed = true;

    private int[,,] map;

    void Start()
    {
        GenerateCave();
    }

    void GenerateCave()
    {
        map = new int[width, depth, height];
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
            for (int y = 0; y < depth; y++)
            {
                for (int z = 0; z < height; z++)
                {
                    if (x == 0 || x == width - 1 || z == 0 || z == height - 1 || y == 0 || y == depth - 1)
                    {
                        map[x, y, z] = 1; // Border walls
                    }
                    else
                    {
                        map[x, y, z] = (rand.NextDouble() < fillPercent) ? 1 : 0;
                    }
                }
            }
        }
    }

    void SmoothMap()
    {
        int[,,] newMap = new int[width, depth, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                for (int z = 0; z < height; z++)
                {
                    int neighborWalls = GetSurroundingWallCount(x, y, z);
                    newMap[x, y, z] = (neighborWalls > 13) ? 1 : (neighborWalls < 13) ? 0 : map[x, y, z];
                }
            }
        }

        map = newMap;
    }

    void EnsureEntrance()
    {
        int entranceX = 14;
        int entranceY = depth / 2; // Middle level for easy access
        int entranceZ = height - 1;

        Debug.Log($"Ensuring entrance at: ({entranceX}, {entranceY}, {entranceZ}) in grid space");

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -2; dx <= 2; dx++)
            {
                for (int dz = 0; dz < 5; dz++)
                {
                    int x = entranceX + dx;
                    int y = entranceY + dy;
                    int z = entranceZ - dz;

                    if (x >= 0 && x < width && y >= 0 && y < depth && z >= 0 && z < height)
                    {
                        map[x, y, z] = 0;
                    }
                }
            }
        }
    }

    int GetSurroundingWallCount(int x, int y, int z)
    {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dz = -1; dz <= 1; dz++)
                {
                    if (dx == 0 && dy == 0 && dz == 0) continue;

                    int nx = x + dx;
                    int ny = y + dy;
                    int nz = z + dz;

                    if (nx >= 0 && nx < width && ny >= 0 && ny < depth && nz >= 0 && nz < height)
                    {
                        count += map[nx, ny, nz];
                    }
                    else
                    {
                        count++; // Out of bounds treated as wall
                    }
                }
            }
        }
        return count;
    }

    void DrawCave()
    {
        Vector3 offset = new Vector3(-width / 2, 0, -height / 2);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                for (int z = 0; z < height; z++)
                {
                    Vector3 pos = new Vector3(x, y, z) + offset;
                    if (map[x, y, z] == 1)
                    {
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.position = pos;
                        wall.transform.localScale = Vector3.one;
                    }
                }
            }
        }
    }
}