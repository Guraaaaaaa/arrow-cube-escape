using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public enum ArrowDirection
{
    Up,
    Down,
    Left,
    Right
}
public class ArrrowController : MonoBehaviour
{
    public ArrowDirection direction;
    public Vector2Int Gridposition;
    public bool isMoving = false;
    public int lenght = 2;

    public List<Vector2Int> occupiedCells = new List<Vector2Int>();

    public void SetupLogic()
    {
        occupiedCells.Clear();
            for(int i = 0; i < lenght; i++ )
        {
            Vector2Int BodyPartPos = CalculateBodyPart(Gridposition, direction, i);
            occupiedCells.Add(BodyPartPos);
        }    
    }

    private Vector2Int CalculateBodyPart(Vector2Int head, ArrowDirection dir, int index)
    {
        // Nếu index = 0, trả về chính nó (đầu mũi tên)
        // Nếu index > 0, tính toán các ô phía sau đuôi
        switch (dir)
        {
            case ArrowDirection.Up: return new Vector2Int(head.x, head.y - index);
            case ArrowDirection.Down: return new Vector2Int(head.x, head.y + index);
            case ArrowDirection.Left: return new Vector2Int(head.x + index, head.y);
            case ArrowDirection.Right: return new Vector2Int(head.x - index, head.y);
            default: return head;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
