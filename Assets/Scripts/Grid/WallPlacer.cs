using FirerusUtilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallPlacer : MonoBehaviour
{
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _startPointPrefab;
    [SerializeField] private GameObject _finishPointPrefab;

    [SerializeField] private float _placeRadius;
    [SerializeField] private float _cornerDecreaseFactor;

    public readonly List<GameObject> Walls = new List<GameObject>();
    private GameObject _startPoint;
    private GameObject _finishPoint;

    public Vector3 StartPosition => _startPoint.transform.position;
    public Vector3 FinishPosition => _finishPoint.transform.position;

    private TypeForPlacing _selectedType;

    [SerializeField] private TapInput _input;

    public void SelectEmpty() => SelectType(TypeForPlacing.Empty);
    public void SelectWall() => SelectType(TypeForPlacing.Wall);
    public void SelectStartPoint() => SelectType(TypeForPlacing.StartPoint);
    public void SelectEndPoint() => SelectType(TypeForPlacing.EndPoint);

    public void SelectType(TypeForPlacing type) => _selectedType = type;

    public void Place(Vector3 position)
    {
        switch (_selectedType)
        {
            case TypeForPlacing.Empty:
                RemoveTile(position);
                break;

            case TypeForPlacing.Wall:
                PlaceWall(position);
                break;

            case TypeForPlacing.StartPoint:
                PlaceStartPoint(position);
                break;

            case TypeForPlacing.EndPoint:
                PlaceEndPoint(position);
                break;
        }
    }

    private void RemoveTile(Vector3 position)
    {
        Vector3 removePosition = GetSidePosition(position, out Vector3 cellPosition, out Vector3 roundedDifference);
        GameObject wall = FindWallAtPosition(removePosition);
        
        if (wall != null)
        {
            Destroy(wall);
            Walls.Remove(wall);
        }
    }

    private void PlaceWall(Vector3 position)
    {
        Vector3 placePosition = GetSidePosition(position, out Vector3 cellPosition, out Vector3 roundedDifference);

        if (CantPlace() || PositionIsntFree())
            return;

        Quaternion rotation = GetRotation(roundedDifference);
        Walls.Add(Instantiate(_wallPrefab, placePosition, rotation, transform));


        bool CantPlace() => placePosition == cellPosition;
        bool PositionIsntFree() => FindWallAtPosition(placePosition) != null;
    }

    private Vector3 GetSidePosition(Vector3 tapPosition, out Vector3 cellPosition, out Vector3 roundedDifference)
    {
        cellPosition = _input.Grid.PositionToCell(tapPosition);
        Cell cell = _input.Grid.GetCell(cellPosition);

        if (cell != null)
        {
            roundedDifference = (-(cellPosition - tapPosition).normalized / _cornerDecreaseFactor).Round();

            Vector3 corner = cell.GetCorner(roundedDifference);
            if (Vector3.Distance(corner, tapPosition) <= _placeRadius)
            {
                return corner;
            }
            return cell.GetCorner(Vector2.zero);
        }

        throw new System.Exception($"Cell at position {cellPosition} not found");
    }

    private GameObject FindWallAtPosition(Vector3 position) => Walls.FirstOrDefault(w => w.transform.position == position);

    private Quaternion GetRotation(Vector3 roudedDifference) => Quaternion.Euler(0, roudedDifference.z * 90, 0);

    private void PlaceStartPoint(Vector3 position)
    {
        _startPoint = PlacePoint(_startPointPrefab, _startPoint, position);
    }

    private void PlaceEndPoint(Vector3 position)
    {
        _finishPoint = PlacePoint(_finishPointPrefab, _finishPoint, position);
    }

    private GameObject PlacePoint(GameObject prefab, GameObject current, Vector3 position)
    {
        Vector3 cellPosition = _input.Grid.PositionToCell(position);

        if (current != null)
        {
            current.transform.position = cellPosition;
            return current;
        }

        return Instantiate(prefab, cellPosition, Quaternion.identity);
    }
}

public enum TypeForPlacing
{
    Empty,
    Wall,
    StartPoint,
    EndPoint
}