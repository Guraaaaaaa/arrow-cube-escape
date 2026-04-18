using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
  public int levelID;
  public int gridSize = 6;
  public List<FaceData> faces; 

}
[System.Serializable]
public class FaceData
{
  public Vector2Int position;
  public List<ArrowData> arrows;
}
