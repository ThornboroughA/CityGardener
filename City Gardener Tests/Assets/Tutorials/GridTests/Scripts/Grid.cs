using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    public float waterLevel = 0.4f;
    public int size = 100;
    public float scale = 0.1f;


    public MeshRenderer meshRenderer;
    public Material terrainMaterial;
    public Color greenCol, blueCol;
    public Color fieldCol, forestCol, marshCol;
    private Cell[,] grid;


    private void Start()
    {
        float[,] noiseMap = GenerateNoiseMap(10000);
        float[,] terrainNoise = GenerateNoiseMap(500);

        float[,] falloffMap = GenerateFalloffMap();

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = new Cell();
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                cell.isWater = noiseValue < waterLevel;

                // Assign terrain
                if (cell.isWater)
                {
                    cell.terrainType = Cell.TerrainType.water;
                } else
                {
                    float terrainValue = terrainNoise[x, y];
                    cell.terrainType = cell.AssignTerrainType(terrainValue);
                    Debug.Log(terrainValue);
                }

                // Cell data

                Vector2 cellCoords = new Vector2(x, y);
                cell.cellCoordinates = cellCoords;

                // end cell data

                grid[x, y] = cell;
            }
        }

        DrawTerrainMesh(grid);

        // Assign materials
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;

        DrawTexture(grid);
    }

    public void GetCellData(int x, int y)
    {
        Cell localCell = grid[x, y];

        //Debug.Log("Cell terrain type is: " + localCell.terrainType);



        if (localCell.isWater == false)
        {
            localCell.terrainType = Cell.TerrainType.marsh;
            DrawTexture(grid);

            Debug.Log(localCell.pop.name);
        }

    }

    private void DrawTerrainMesh(Cell[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
            Cell cell = grid[x, y];
                
                // Mesh points
                Vector3 a = new Vector3(x -0.5f, 0, y + 0.5f);
                Vector3 b = new Vector3(x + 0.5f, 0, y + 0.5f);
                Vector3 c = new Vector3(x - 0.5f, 0, y - 0.5f);
                Vector3 d = new Vector3(x + 0.5f, 0, y - 0.5f);
                Vector3[] v = new Vector3[] { a, b, c, b, d, c };

                // UVs
                Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
                Vector2 uvB = new Vector2((x + 1) / (float)size, y / (float)size); 
                Vector2 uvC = new Vector2(x / (float)size, (y + 1) / (float)size);
                Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
                Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };


                for (int k = 0; k < 6; k++)
                {
                    vertices.Add(v[k]);
                    triangles.Add(triangles.Count);
                    uvs.Add(uv[k]);
                }
            }
            
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

    }
    private void DrawTexture(Cell[,] grid)
    {
        // Generate the textures
        // Assign colors based on the type of terrain.

        Texture2D texture = new Texture2D(size, size);
        Color[] colorMap = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                {
                    colorMap[y * size + x] = blueCol;
                }
                else
                {
                    if (cell.terrainType == Cell.TerrainType.field)
                    {
                        colorMap[y * size + x] = fieldCol;
                    } else if (cell.terrainType == Cell.TerrainType.forest)
                    {
                        colorMap[y * size + x] = forestCol;
                    } else if (cell.terrainType == Cell.TerrainType.marsh)
                    {
                        colorMap[y * size + x] = marshCol;
                    } else
                    {
                        colorMap[y * size + x] = greenCol;
                    }
                } 


                    
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();


        meshRenderer.material.mainTexture = texture;
    }

    private float[,] GenerateFalloffMap()
    {
        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        return falloffMap;
    }

    private float[,] GenerateNoiseMap(float offset)
    {
        float[,] noiseMap = new float[size, size];
        float xOffset = Random.Range(-offset, offset);
        float yOffset = Random.Range(-offset, offset);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        return noiseMap;
    }

    /*private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    Gizmos.color = Color.blue;
                else
                    Gizmos.color = Color.green;
                Vector3 pos = new Vector3(x, 0, y);
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }*/
}
