using UnityEngine;

public class Inputmanganer : MonoBehaviour
{
    private Vector2 FingerDownPos;
    private float TapThreshold = 10f; // Adjust this value to set the maximum distance for a tap
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetMouseButtonDown(0))
        {
            FingerDownPos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            Vector2 FingerUpPos = Input.mousePosition;
            if(Vector2.Distance(FingerDownPos, FingerUpPos) < TapThreshold)
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
                    
                    Debug.Log("Arrow selected: " + arrow.direction);
                }
            }
        }
    }
}
