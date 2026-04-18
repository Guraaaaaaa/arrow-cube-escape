using UnityEngine;

public class GridFaceManager :  MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ArrrowController[,] grid = new ArrrowController[5, 5];

    public void InitializeGrid()
    {
        // Lấy tất cả mũi tên con của mặt này
        ArrrowController[] arrowsOnFace = GetComponentsInChildren<ArrrowController>();

        foreach (var arrow in arrowsOnFace)
        {
            arrow.SetupLogic();
            foreach (var cellPos in arrow.occupiedCells)
            {
                // Kiểm tra nếu ô nằm trong phạm vi 5x5
                if (IsInsideGrid(cellPos))
                {
                    grid[cellPos.x, cellPos.y] = arrow;
                }
            }
        }
    }

    bool IsInsideGrid(Vector2Int pos) => pos.x >= 0 && pos.x < 5 && pos.y >= 0 && pos.y < 5;
}
