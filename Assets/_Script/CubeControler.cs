using UnityEngine;

public class CubeControler : MonoBehaviour
{
    [Header("Rotation setting")]
    [SerializeField] private float Sentivity = 5f;
    [SerializeField] private float LerpValue = 0.1f;

    public Vector3 RotationVelocity;
    private bool isDragging = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void HandleRotation()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if(isDragging)
        {
            float mouseX = Input.GetAxis("Mouse X") * Sentivity;
            float mouseY = Input.GetAxis("Mouse Y") * Sentivity;
            RotationVelocity = new Vector3(-mouseY, mouseX, 0);
            transform.Rotate(   Vector3.up, -mouseX * Sentivity, Space.World);
            transform.Rotate(   Vector3.right, mouseY * Sentivity, Space.World);
        }
        else
        {
            RotationVelocity = Vector3.Lerp(RotationVelocity, Vector3.zero, LerpValue);
            transform.Rotate(Vector3.up,RotationVelocity.y, Space.World);
            transform.Rotate(Vector3.right, RotationVelocity.x, Space.World);
        }
    }    
    // Update is called once per frame
    void Update()
    {
        HandleRotation();
    }
}
