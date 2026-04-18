using UnityEngine;

public class CubeControler : MonoBehaviour
{
    [Header("Rotation setting")]
    [SerializeField] private float Sentivity = 5f;
    [SerializeField] private float LerpValue = 0.1f;
    [SerializeField] private float TapThreshold = 10f; // Ngưỡng pixel để phân biệt tap/drag

    public Vector3 RotationVelocity;
    
    public static bool IsDragging { get; private set; }
    public static bool WasDraggingThisClick { get; private set; } // Giữ trạng thái để InputManager đọc
    
    private Vector2 fingerDownPos;

    void Start()
    {
         
    }

    void HandleRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fingerDownPos = Input.mousePosition;
            IsDragging = false;
            WasDraggingThisClick = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(fingerDownPos, Input.mousePosition) >= TapThreshold)
            {
                IsDragging = true;
                WasDraggingThisClick = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            IsDragging = false;
        }

        if (IsDragging)
        {
            float axisX = Input.GetAxis("Mouse X");
            float axisY = Input.GetAxis("Mouse Y");
            
            float rotX = axisX * Sentivity;
            float rotY = axisY * Sentivity;

            // Đã sửa lỗi lưu sai dấu cho quán tính
            // Khi kéo, ta xoay quanh trục Up một góc là -rotX, và trục Right một góc là rotY.
            // Do đó, ta phải lưu đúng góc này vào RotationVelocity.
            RotationVelocity = new Vector3(rotY, -rotX, 0);
            
            transform.Rotate(Vector3.up, -rotX, Space.World);
            transform.Rotate(Vector3.right, rotY, Space.World);
        }
        else
        {
            RotationVelocity = Vector3.Lerp(RotationVelocity, Vector3.zero, LerpValue);
            transform.Rotate(Vector3.up, RotationVelocity.y, Space.World);
            transform.Rotate(Vector3.right, RotationVelocity.x, Space.World);
        }
    }    

    void Update()
    {
        HandleRotation();
    }
}
