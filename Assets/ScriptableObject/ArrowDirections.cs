using UnityEngine;

[CreateAssetMenu(fileName = "ArrowDirections", menuName = "Scriptable Objects/ArrowDirections")]
public class ArrowDirections : ScriptableObject
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
