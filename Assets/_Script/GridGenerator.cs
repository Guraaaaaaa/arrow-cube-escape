using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefabs;
    public int Gridsize = 5;
    public float cellSize = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid(); 
    }
    void GenerateGrid()
    {
        float offset = (Gridsize - 1) * cellSize / 2;
        for (int x = 0; x < Gridsize; x++)
        {
            for (int y = 0; y < Gridsize; y++)
            {
                GameObject cell = Instantiate(cellPrefabs,transform);
                float posX = (x * cellSize) - offset;
                float posY = (y * cellSize) - offset;
                cell.transform.localPosition = new Vector3(posX, posY, -0.01f);
                cell.name = $"Cell_{x}_{y}";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
