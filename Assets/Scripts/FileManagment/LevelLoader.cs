using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private string _levelName;
    [SerializeField] private Grid _grid;
    [SerializeField] private PointManager _pointManager;
    [SerializeField] private GameObject _startPointPrefab;
    [SerializeField] private GameObject _finishPointPrefab;
    [SerializeField] private GameObject _wallPrefab;

    private Level _level;

    private void Awake()
    {
        _level = (Level)SaveManager.Load(_levelName);

        if (_level == null)
            return;

        _grid.ChangeSize(_level.GridSize);
        InstantiatePoints(_level.StartPosition, _level.FinishPosition);
        InstantiateWalls(_level.Walls);
    }

    private void InstantiatePoints(Vector3 startPointPosition, Vector3 finishPointPosition)
    {
        GameObject startPoint = Instantiate(_startPointPrefab, startPointPosition, Quaternion.identity);
        GameObject finishPoint = Instantiate(_finishPointPrefab, finishPointPosition, Quaternion.identity);
        _pointManager.SetPoints(startPoint, finishPoint);
    }

    private void InstantiateWalls(List<Wall> walls)
    {
        foreach (var wall in walls)
        {
            Quaternion rotation = Quaternion.Euler(0, wall.Rotated ? 90 : 0, 0);
            Instantiate(_wallPrefab, wall.Position, rotation);
        }
    }
}
