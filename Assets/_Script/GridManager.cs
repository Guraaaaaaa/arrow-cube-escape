using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSize = 5;
    public float cellSize = 0.2f;
    
    public Dictionary<Vector2Int, ArrrowController> cells = new Dictionary<Vector2Int, ArrrowController>();
    public HashSet<ArrrowController> activeArrows = new HashSet<ArrrowController>();
    public bool isAnimating = false;
    public int stepCount { get; private set; } = 0;

    public event Action<ArrrowController> OnArrowRemoved;
    public event Action OnAllCleared;
    
    public event Action<ArrrowController, List<Vector3>, Action> OnSlideRequested;
    public event Action<ArrrowController> OnInvalidMove;

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

    /// Kiểm tra xem tọa độ có nằm trong phạm vi Grid không
    public bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
    }

    /// Kiểm tra xem mũi tên có thể trượt ra khỏi grid không (không bị chặn)
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

    /// Trả về danh sách vị trí thế giới (world position) mà mũi tên sẽ đi qua
    public List<Vector3> GetSlidePath(ArrrowController arrow)
    {
        List<Vector3> path = new List<Vector3>();
        if (arrow.occupiedCells == null || arrow.occupiedCells.Count == 0) return path;

        Vector2Int currentPos = arrow.occupiedCells[0];
        Vector2Int delta = GetDirectionDelta(arrow.direction);

        while (true)
        {
            currentPos += delta;

            if (IsInsideGrid(currentPos))
            {
                path.Add(GridToWorldPosition(currentPos, arrow.transform.localPosition.z));
            }
            else
            {
                path.Add(GridToWorldPosition(currentPos, arrow.transform.localPosition.z));
                Vector2Int extraPos = currentPos + delta * 2;
                path.Add(GridToWorldPosition(extraPos, arrow.transform.localPosition.z));
                break;
            }
        }

        return path;
    }

    /// <summary>
    /// Chuyển đổi tọa độ grid thành vị trí thực tế trong thế giới 3D
    /// </summary>
    private Vector3 GridToWorldPosition(Vector2Int gridPos, float localZ)
    {
        float offset = (gridSize - 1) * cellSize / 2f;
        float localX = (gridPos.x * cellSize) - offset;
        float localY = (gridPos.y * cellSize) - offset;
        
        Vector3 localPosition = new Vector3(localX, localY, localZ);
        
        return transform.TransformPoint(localPosition);
    }

    /// <summary>
    /// Entry point khi người chơi click vào mũi tên
    /// </summary>
    public void TrySlide(ArrrowController arrow)
    {
        if (isAnimating) return;
        if (!activeArrows.Contains(arrow)) return;
        if (IsPathClear(arrow))
        {
            isAnimating = true;
            stepCount++;
            
            List<Vector3> path = GetSlidePath(arrow);

            OnSlideRequested?.Invoke(arrow, path, () => 
            {
                RemoveArrow(arrow);
                isAnimating = false;
            });
        }
        else
        {
            OnInvalidMove?.Invoke(arrow);
        }
    }

    /// <summary>
    /// Xóa mũi tên khỏi bộ nhớ sau khi nó đã bay ra khỏi grid
    /// </summary>
    private void RemoveArrow(ArrrowController arrow)
    {
        foreach (var cellPos in arrow.occupiedCells)
        {
            if (cells.TryGetValue(cellPos, out ArrrowController occupant) && occupant == arrow)
            {
                cells.Remove(cellPos);
            }
        }
        
        activeArrows.Remove(arrow);
        
        OnArrowRemoved?.Invoke(arrow);

        // Win Condition
        if (activeArrows.Count == 0)
        {
            OnAllCleared?.Invoke();
        }
    }
}
