using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSize = 5;
    
    public Dictionary<Vector2Int, ArrrowController> cells = new Dictionary<Vector2Int, ArrrowController>();
    public HashSet<ArrrowController> activeArrows = new HashSet<ArrrowController>();
    public bool isAnimating = false;
    public event Action<ArrrowController> OnArrowRemoved;
    public event Action OnAllCleared;


    void Start()
    {

        LoadFace(null);
    }
    // ---------------------------------

    /// <summary>
    /// Khởi tạo và load dữ liệu mặt cube
    /// </summary>
    public void LoadFace(FaceData data)
    {
        cells.Clear();
        activeArrows.Clear();

        ArrrowController[] arrowsOnFace = GetComponentsInChildren<ArrrowController>();

        foreach (var arrow in arrowsOnFace)
        {
            // Thiết lập vị trí lưới dựa trên inspector hoặc data nếu có
            arrow.SetupLogic();

            foreach (var cellPos in arrow.occupiedCells)
            {
                if (IsInsideGrid(cellPos))
                {
                    cells[cellPos] = arrow;
                }
                else
                {
                    Debug.LogWarning($"Mũi tên {arrow.name} ở vị trí {cellPos} nằm ngoài Grid {gridSize}x{gridSize}");
                }
            }
            activeArrows.Add(arrow);
        }
    }

    /// <summary>
    /// Kiểm tra xem tọa độ có nằm trong phạm vi Grid không
    /// </summary>
    public bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
    }

    /// <summary>
    /// Kiểm tra xem mũi tên có thể trượt ra khỏi grid không (không bị chặn)
    /// </summary>
    public bool IsPathClear(ArrrowController arrow)
    {
        if (arrow.occupiedCells == null || arrow.occupiedCells.Count == 0) return true;

        Vector2Int currentPos = arrow.occupiedCells[0];

        Vector2Int delta = GetDirectionDelta(arrow.direction);

        while (true)
        {
            currentPos += delta;

            if (!IsInsideGrid(currentPos))
            {
                return true;
            }

            if (cells.TryGetValue(currentPos, out ArrrowController occupant))
            {
                if (occupant != arrow)
                {
                  
                    return false;
                }
            }
        }
    }

    public Vector2Int GetDirectionDelta(ArrowDirection dir)
    {
        switch (dir)
        {
            case ArrowDirection.Up: return new Vector2Int(0, 1);
            case ArrowDirection.Down: return new Vector2Int(0, -1);
            case ArrowDirection.Left: return new Vector2Int(-1, 0);
            case ArrowDirection.Right: return new Vector2Int(1, 0);
            default: return Vector2Int.zero;
        }
    }
}
