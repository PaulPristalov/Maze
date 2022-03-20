using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructorSave : MonoBehaviour
{
    [SerializeField] private WallPlacer _placer;
    [SerializeField] private Grid _grid;

    [SerializeField] private InputField _levelNameField;

    public void Save()
    {
        List<Wall> walls = GetWalls();
        var level = new Level(_grid.Size, _placer.StartPosition, _placer.FinishPosition, walls);
        SaveManager.Save(_levelNameField.text, level);

        Debug.Log($"Succsessfully saved as {_levelNameField.text}");
    }

    private List<Wall> GetWalls()
    {
        var walls = new List<Wall>(_placer.Walls.Count);
        foreach (var wall in _placer.Walls)
        {
            walls.Add(new Wall(wall.transform.position, wall.transform.eulerAngles.y > 0));
        }

        return walls;
    }
}

[System.Serializable]
public class Level
{
    public Vector2Int GridSize;
    public Vector3 StartPosition;
    public Vector3 FinishPosition;
    public List<Wall> Walls;

    public Level(Vector2Int gridSize, Vector3 startPosition, Vector3 finishPosition, List<Wall> walls)
    {
        GridSize = gridSize;
        StartPosition = startPosition;
        FinishPosition = finishPosition;
        Walls = walls;
    }
}

[System.Serializable]
public class Wall
{
    public Vector3 Position;
    public bool Rotated;

    public Wall(Vector3 position, bool rotated)
    {
        Position = position;
        Rotated = rotated;
    }
}
