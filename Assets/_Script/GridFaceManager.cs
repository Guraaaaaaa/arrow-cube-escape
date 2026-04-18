using UnityEngine;

public class GridFaceManager :  MonoBehaviour
{
    public int gridSize = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ArrrowController[,] grid;

    public void InitializeGrid()
    {
        grid = new ArrrowController[gridSize, gridSize];
        // Lấy tất cả mũi tên con của mặt này
        ArrrowController[] arrowsOnFace = GetComponentsInChildren<ArrrowController>();

        foreach (var arrow in arrowsOnFace)
        {
            arrow.SetupLogic();
            foreach (var cellPos in arrow.occupiedCells)
            {
                // Kiểm tra nếu ô nằm trong phạm vi
                if (IsInsideGrid(cellPos))
                {
                    grid[cellPos.x, cellPos.y] = arrow;
                }
            }
        }
    }

    bool IsInsideGrid(Vector2Int pos) => pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
}
