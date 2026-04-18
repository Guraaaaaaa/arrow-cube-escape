using UnityEngine;

public class Inputmanganer : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        // Chỉ xử lý tap khi người dùng nhả chuột và KHÔNG phải là đang drag khối cube
        if(Input.GetMouseButtonUp(0))
        {
            if(!CubeControler.WasDraggingThisClick)
            {
                CheckSelectArrow();
            }
        }
    }
    
    void CheckSelectArrow()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.CompareTag("Arrow"))
            {
                ArrrowController arrow = hit.collider.GetComponent<ArrrowController>();
                if(arrow != null)
                {
                    GridManager grid = arrow.GetComponentInParent<GridManager>();
                    if (grid != null)
                    {
                        bool isClear = grid.IsPathClear(arrow);
                        Debug.Log($"[Test] Bạn vừa click mũi tên {arrow.name} (Hướng: {arrow.direction}). Path thông thoáng? -> {isClear}");
                    }
                    else
                    {
                        Debug.LogWarning("Không tìm thấy GridManager ở object cha của mũi tên! Hãy gắn GridManager vào object chứa các mũi tên.");
                    }
                }
            }
        }
    }
}
