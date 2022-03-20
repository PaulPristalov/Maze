using UnityEngine;
using FirerusUtilities;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2 _scale = Vector2.one;
    [SerializeField] private Transform _object;

    private Cell[,] _cells;

    public bool Initialized => _cells != null && _cells.Length != 0;
    public Vector2Int Size => _size;

    private void Start()
    {
        if (Initialized)
            return;

        Initialize();
    }

    private void Initialize()
    {
        _cells = new Cell[_size.x, _size.y];
        if (_object != null)
        {
            _object.localScale = (Vector3)(_size * _scale) + new Vector3(0, 0, 1);
        }

        CreateGrid();
    }

    #region Creating
    private void CreateGrid()
    {
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            BuildColumn(x);
        }
    }

    private void BuildColumn(int x)
    {
        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            CreateCell(x, y);
        }
    }

    private void CreateCell(int x, int y)
    {
        Vector3 position = new Vector3(CalculateX(), 0, CalculateY());
        _cells[x, y] = new Cell(position, _scale);

        float CalculateX() => _scale.x * x - (_size.x * _scale.x - 1) / 2;
        float CalculateY() => _scale.y * y - (_size.y * _scale.y - 1) / 2;
    }
    #endregion

    #region Showing
    private void OnDrawGizmos() => ShowGrid();

    private void ShowGrid()
    {
        if (_cells == null)
            return;

        foreach (var cell in _cells)
        {
            DrawCellBorder(cell);
        }

        DrawMissingLines();
    }

    private void DrawCellBorder(Cell cell)
    {
        Gizmos.DrawLine(cell.GetCorner(Corners.LeftBottom), cell.GetCorner(Corners.LeftUpper));
        Gizmos.DrawLine(cell.GetCorner(Corners.LeftBottom), cell.GetCorner(Corners.RightBottom));
    }

    private void DrawMissingLines()
    {
        Gizmos.DrawLine(_cells.LastYElement(0).GetCorner(Corners.LeftUpper), _cells.LastElement().GetCorner(Corners.RightUpper));
        Gizmos.DrawLine(_cells.LastElement().GetCorner(Corners.RightUpper), _cells.LastXElement(0).GetCorner(Corners.RightBottom));
    }
    #endregion

    #region PositionToCell
    public Vector3 PositionToCell(Vector3 position)
    {
        Vector3 cellPosition = CalculateXY(position);
        if (InBounds(cellPosition))
            return cellPosition;

        throw new System.ArgumentException("Position isn't inside grid bounds.", nameof(position));
    }

    private Vector3 CalculateXY(Vector3 position)
    {
        float x = Mathf.Floor(position.x) * _scale.x + _scale.x / 2;
        float y = Mathf.Floor(position.z) * _scale.y + _scale.y / 2;
        return new Vector3(x, 0, y);
    }

    private bool InBounds(Vector3 position)
    {
        return CheckX() && CheckY();

        bool CheckX() => position.x >= _cells[0, 0].Position.x && position.x <= _cells.LastElement().Position.x;
        bool CheckY() => position.y >= _cells[0, 0].Position.y && position.y <= _cells.LastElement().Position.y;
    }
    #endregion

    public Cell GetCell(Vector3 cellPosition)
    {
        foreach (var cell in _cells)
        {
            if (cell.Position == cellPosition)
                return cell;
        }

        return null;
    }

    public void ChangeSize(Vector2Int newSize)
    {
        _size = newSize;
        Initialize();
    }
}

public class Cell
{
    public Vector3 Position { get; private set; }
    public Vector2 Scale { get; private set; }

    public Cell(Vector3 position, Vector2 scale)
    {
        Position = position;
        Scale = scale;
    }

    public Vector3 GetCorner(Corners corner)
    {
        switch (corner)
        {
            case Corners.LeftBottom:
                return GetCorner(new Vector2(-1, -1));

            case Corners.LeftUpper:
                return GetCorner(new Vector2(-1, 1));

            case Corners.RightBottom:
                return GetCorner(new Vector2(1, -1));

            case Corners.RightUpper:
                return GetCorner(new Vector2(1, 1));

            default:
                throw new System.ArgumentException("There are no such corner", nameof(corner));
        }
    }

    public Vector3 GetCorner(Vector3 cornerVector)
    {
        return GetCorner(new Vector2(cornerVector.x, cornerVector.z));
    }

    public Vector3 GetCorner(Vector2 cornerVector)
    {
        return new Vector3(Position.x + Scale.x / 2 * cornerVector.x, 0, Position.z + Scale.y / 2 * cornerVector.y);
    }
}

public enum Corners
{
    LeftBottom,
    LeftUpper,
    RightUpper,
    RightBottom,
}
