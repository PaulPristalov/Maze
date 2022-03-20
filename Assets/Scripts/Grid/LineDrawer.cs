using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private Vector3 _offset = new Vector3(0, 0.5f);
    [SerializeField] private float _maxNextDistance = 1f;
    private int _currentVertex = 0;

    [SerializeField] private TapInput _input;
    [SerializeField] private Character _character;

    private void Start()
    {
        _input.OnTapUp.AddListener(FinishLine);
        _character.OnClicked.AddListener(StartLine);
    }

    public void DrawLine(Vector3 position)
    {
        if (NotAbleToDraw(position))
            return;

        DrawPart(position);
    }

    private bool NotAbleToDraw(Vector3 position)
    {
        if (_currentVertex == 0)
            return true;

        bool notMoving = position == _renderer.GetPosition(_currentVertex - 1) - _offset;
        bool positionOutOfRange = (_renderer.GetPosition(_currentVertex - 1) - _offset - position).magnitude > _maxNextDistance;
        bool wall = CheckForWall(position);
        bool movingBack = TryMoveBack(position);

        return notMoving || positionOutOfRange || wall || movingBack;
    }

    private bool TryMoveBack(Vector3 position)
    {
        Vector3[] positions = new Vector3[_currentVertex];
        for (int i = 0; i < _renderer.GetPositions(positions) - 1; i++)
        {
            if (position == positions[i] - _offset)
            {
                SetVertexCount(i);
                return true;
            }
        }

        return false;
    }

    private bool CheckForWall(Vector3 newPosition)
    {
        Vector3 currentPosition = _renderer.GetPosition(_currentVertex - 1);
        Vector3 direction = (newPosition + _offset) - currentPosition;

        Ray ray = new Ray(currentPosition, direction);
        Debug.DrawRay(currentPosition, direction.normalized * _maxNextDistance, Color.white);

        return Physics.Raycast(ray, _maxNextDistance);
    }

    private void DrawPart(Vector3 position)
    {
        _renderer.positionCount++;
        _renderer.SetPosition(_currentVertex++, position + _offset);
    }

    public void StartLine(Vector3 position)
    {
        Vector3 roundedPosition = _input.Grid.PositionToCell(position);
        DrawPart(new Vector3(roundedPosition.x, 0, roundedPosition.z));
    }

    private void FinishLine()
    {
        Vector3[] positions = new Vector3[_currentVertex];
        _renderer.GetPositions(positions);

        _character.SetPath(positions);
        DeleteLine();
    }

    private void DeleteLine()
    {
        SetVertexCount(0);
    }

    private void SetVertexCount(int count)
    {
        _renderer.positionCount = count;
        _currentVertex = count;
    }
}
