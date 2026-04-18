using UnityEngine;

[CreateAssetMenu(fileName = "ArrowData", menuName = "Scriptable Objects/ArrowData")]
public class ArrowData : ScriptableObject
{
    public ArrowDirections.Direction direction;
    public Vector2Int position;
}
